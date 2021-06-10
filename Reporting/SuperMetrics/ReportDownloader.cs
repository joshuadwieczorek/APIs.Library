using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Threading;
using AAG.Global.ExtensionMethods;
using APIs.Library.Common;
using APIs.Library.Reporting.Contracts;
using APIs.Library.Reporting.SuperMetrics.Contracts;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace APIs.Library.Reporting.SuperMetrics
{
    public sealed class ReportDownloader<T> : BaseLogger<ReportDownloader<T>>, IDisposable
    {
        private readonly object _threadLockChannelReports;
        private readonly object _threadLockChannelLogs;
        private readonly IConnection _connection;
        private readonly IModel _channelLogs;
        private readonly IModel _channelReports;
        private readonly HttpClient _client;
        private readonly string _superMetricsBaseApiUrl;
        private readonly string _csvDelimiter;
        private readonly int _reportsToDownloadInParallel;
        private readonly string _rabbitMqQueueNameReports;
        private readonly string _rabbitMqQueueNameLogs;
        private readonly string _exchangeLogs;
        private readonly string _exchangeReports;
        

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="bugSnag"></param>
        /// <param name="configuration"></param>
        public ReportDownloader(
              ILogger<ReportDownloader<T>> logger
            , Bugsnag.IClient bugSnag
            , IConfiguration configuration) : base(logger, bugSnag)
        {
            _threadLockChannelReports = new object();
            _threadLockChannelLogs = new object();
            _client = new HttpClient();
            _superMetricsBaseApiUrl = configuration[StaticText.SuperMetricsBaseApiUrl];
            _csvDelimiter = configuration[StaticText.CsvDelimiter];
            if (!int.TryParse(configuration[StaticText.ReportsToDownloadInParallel], out _reportsToDownloadInParallel))
                _reportsToDownloadInParallel = 1;
            var rabbitMqServer = configuration[StaticText.RabbitMqServer];
            var rabbitMqVHost = configuration[StaticText.RabbitMqVHost];
            var rabbitMqUser = configuration[StaticText.RabbitMqUser];
            var rabbitMqPassword = configuration[StaticText.RabbitMqPassword];
            var rabbitMqPort = configuration[StaticText.RabbitMqPort];
            _rabbitMqQueueNameReports = configuration[StaticText.RabbitMqQueueNameReportsToLoad];
            _rabbitMqQueueNameLogs = configuration[StaticText.RabbitMqQueueNameLogs];
            _exchangeLogs = configuration[StaticText.RabbitMqExchangeLogs];
            _exchangeReports = configuration[StaticText.RabbitMqExchangeReports];
            var factory = new ConnectionFactory
            {
                Uri = new Uri($"amqp://{rabbitMqUser}:{WebUtility.UrlEncode(rabbitMqPassword)}@{rabbitMqServer}:{rabbitMqPort}/{rabbitMqVHost}")
            };
            _connection = factory.CreateConnection();
            _channelLogs = _connection.CreateModel();
            _channelLogs.ConfirmSelect();
            _channelLogs.QueueDeclare(_rabbitMqQueueNameLogs, durable: true, exclusive: false, autoDelete: false);
            _channelReports = _connection.CreateModel();
            _channelReports.ConfirmSelect();
            _channelReports.QueueDeclare(_rabbitMqQueueNameReports, durable: true, exclusive: false, autoDelete: false);
        }


        /// <summary>
        /// Process queue.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="cancellationToken"></param>
        public async Task ProcessQueue(
              ConcurrentQueue<ReportDownloadRequest<T>> queue
            , CancellationToken cancellationToken)
        {
            try
            {
                var tasks = new List<Task>();

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (queue.IsEmpty)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    while (tasks.Count < _reportsToDownloadInParallel)
                    {
                        if (!queue.IsEmpty && queue.TryDequeue(out var queueItem))
                        {
                            var taskId = Guid.NewGuid();
                            //await ProcessQueueRequest(taskId, queueItem);
                            tasks.Add(ProcessQueueRequest(taskId, queueItem));
                        }
                        else 
                            break;
                    }

                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                LogError(e);
                PublishLogs(e);
            }

        }


        /// <summary>
        /// Process queue download request.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task ProcessQueueRequest(
              Guid taskId
            , ReportDownloadRequest<T> request)
        {
            try
            {
                // Get account id for file naming convention.
                var accountId = GetAccountId(taskId, request.RequestPayload.DataSetAccounts);

                // Construct report request url.
                var reportRequestUrl = ConstructReportUrl(request.RequestPayload);

                // Make call to get report data from Super Metrics.
                var response = await _client.GetAsync(reportRequestUrl);

                // If the request failed, bail out with the response message.
                if (!response.IsSuccessStatusCode)
                {
                    var ex = new Exception(await response.Content.ReadAsStringAsync());
                    ex.Data.Add("Request", JsonConvert.SerializeObject(response.RequestMessage));
                    ex.Data.Add("Response", JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync()));
                    ex.Data.Add("CompleteResponse", JsonConvert.SerializeObject(response));
                    throw ex;
                }

                // Start our text reader and read http response message.
                var textReader = new StringReader(await response.Content.ReadAsStringAsync());

                // Open up and start our csv reader.
                var csvReader = new CsvReader(textReader, culture: CultureInfo.InvariantCulture);

                // Let's get our records from the csv string.
                var records = csvReader.GetRecords<T>().ToList();

                // If the record has a report start and end date, let's set them.
                if (records is IEnumerable<IHaveReportStartAndEndDates>)
                    foreach (var record in records)
                    {
                        var rcd = (IHaveReportStartAndEndDates) record;
                        rcd.ReportStartDate = request.RequestPayload.StartDate;
                        rcd.ReportEndDate = request.RequestPayload.EndDate;
                    }
                
                // Construct the path to save the new file.
                var downloadSavePath = Path.Combine(request.ReportConfiguration.DataInboundDirectory,
                    $"report_{accountId}_{request.RequestPayload.StartDate}-{request.RequestPayload.EndDate}_downloadedAt_{GetCurrentTimeStampFormat()}.csv");

                // Set the name of the new report file.
                request.ReportConfiguration.ReportFileName = Path.GetFileName(downloadSavePath);

                // If an existing file exists by this name, delete it.
                if (File.Exists(downloadSavePath))
                    File.Delete(downloadSavePath);

                // Startup our new stream writer.
                var streamWriter = new StreamWriter(downloadSavePath);

                // Construct csv reader configurations.
                var csvWriterConfiguration = ConstructCsvConvConfiguration();

                // Start up our csv writer to write our payload to disk.
                var csvWriter = new CsvWriter(streamWriter, csvWriterConfiguration);

                // Write our records to disk.
                await csvWriter.WriteRecordsAsync<T>(records);

                // Do some memory clean up.
                await csvWriter.FlushAsync();
                csvReader.Dispose();
                textReader.Dispose();
                await csvWriter.DisposeAsync();
                await streamWriter.DisposeAsync();
                records = null;
                GC.Collect();

                // Now... time to push report request to the RabbitMQ bus.
                PublishReport(request.ReportConfiguration);
            }
            catch (Exception e)
            {
                LogError(e);
                PublishLogs(e);
            }
        }


        private void PublishReport(ReportConfiguration reportConfiguration)
        {
            lock (_threadLockChannelReports)
            {
                var jsonString = JsonConvert.SerializeObject(reportConfiguration);
                var bodyBytes = Encoding.UTF8.GetBytes(jsonString);
                _channelReports.BasicPublish(_exchangeReports, routingKey: "reports.to.load", body: bodyBytes);
                _channelReports.WaitForConfirmsOrDie(new TimeSpan(0,0,0,10));
            }
        }


        private void PublishLogs(Exception e)
        {
            lock (_threadLockChannelLogs)
            {
                var jsonString = JsonConvert.SerializeObject(e);
                var bodyBytes = Encoding.UTF8.GetBytes(jsonString);
                _channelLogs.BasicPublish(_exchangeLogs, routingKey: string.Empty, body: bodyBytes);
                _channelLogs.WaitForConfirmsOrDie(new TimeSpan(0, 0, 0, 10));
            }
        }


        /// <summary>
        /// Get account id from data download request.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="accounts"></param>
        /// <returns></returns>
        private static string GetAccountId(
              Guid taskId
            , object accounts)
        {
            if (accounts is not List<string>) return "all";
            var accountIds = (List<string>) accounts;
            return accountIds.Any()
                ? accountIds[0]
                : throw new ArgumentOutOfRangeException($"No account ids found for task '{taskId}'");
        }


        /// <summary>
        /// Construct URL with payload.
        /// </summary>
        /// <param name="requestPayload"></param>
        /// <returns></returns>
        private string ConstructReportUrl(RequestPayload requestPayload)
        {
            var jsonString = JsonConvert.SerializeObject(requestPayload);
            var jsonUrlEncoded = WebUtility.UrlEncode(jsonString);
            return string.Join('=', _superMetricsBaseApiUrl, jsonUrlEncoded);
        }


        /// <summary>
        /// Generate current timestamp with format.
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentTimeStampFormat()
            => DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss.fffffff");


        /// <summary>
        /// Construct csv reader configuration.
        /// </summary>
        /// <returns></returns>
        private CsvConfiguration ConstructCsvConvConfiguration()
            => new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true,
                BadDataFound = null,
                ShouldQuote = (field, context) => true,
                Delimiter = _csvDelimiter
            };


        /// <summary>
        /// Garbage cleanup.
        /// </summary>
        public void Dispose()
        {
            _channelReports?.Close();
            _channelReports?.Dispose();
            _channelLogs?.Close();
            _channelLogs?.Dispose();
            _client?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}