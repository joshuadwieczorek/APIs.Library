using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AAG.Global.Contracts;
using APIs.Library.Caching;
using APIs.Library.Common;
using APIs.Library.Enums;
using APIs.Library.Reporting.Contracts;
using APIs.Library.Reporting.SuperMetrics.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace APIs.Library.Reporting.SuperMetrics
{
    public abstract class BaseReportDownloadRequestService<T,R> : BaseLogger<T>, IDisposable
    {
        private readonly Dictionary<Report, ReportConfiguration> _reports;
        private readonly string _superMetricsApiKey;
        private readonly IConnection _connection;
        private readonly IModel _channelReports;
        private readonly string _rabbitMqQueueRoutingKey;
        private readonly string _exchangeReports;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="bugSnag"></param>
        /// <param name="reports"></param>
        /// <param name="superMetricsApiKey"></param>
        protected BaseReportDownloadRequestService(
              ILogger<T> logger
            , Bugsnag.IClient bugSnag
            , IConfiguration configuration  
            , Dictionary<Report, ReportConfiguration> reports
            , string superMetricsApiKey) : base(logger, bugSnag)
        {
            _reports = reports;
            _superMetricsApiKey = superMetricsApiKey;
            var rabbitMqServer = configuration[StaticText.RabbitMqServer];
            var rabbitMqVHost = configuration[StaticText.RabbitMqVHost];
            var rabbitMqUser = configuration[StaticText.RabbitMqUser];
            var rabbitMqPassword = configuration[StaticText.RabbitMqPassword];
            var rabbitMqPort = configuration[StaticText.RabbitMqPort];
            var rabbitMqQueueNameReportsToDownload = configuration[StaticText.RabbitMqQueueNameReportsToDownload];
            _rabbitMqQueueRoutingKey = configuration[StaticText.RabbitMqQueueRoutingKey];
            _exchangeReports = configuration[StaticText.RabbitMqExchangeReports];
            var factory = new ConnectionFactory
            {
                Uri = new Uri($"amqp://{rabbitMqUser}:{WebUtility.UrlEncode(rabbitMqPassword)}@{rabbitMqServer}:{rabbitMqPort}/{rabbitMqVHost}")
            };
            _connection = factory.CreateConnection();
            _channelReports = _connection.CreateModel();
            _channelReports.QueueDeclare(rabbitMqQueueNameReportsToDownload, durable: true, exclusive: false, autoDelete: false);
        }


        /// <summary>
        /// Construct and generate download request.
        /// </summary>
        /// <param name="reportRequest"></param>
        public void GenerateReportDownloadRequest(ReportRequest reportRequest)
        {
            // TODO: Validate report request.

            var reportConfiguration = _reports.ContainsKey(reportRequest.Report)
                ? _reports[reportRequest.Report]
                : throw new ArgumentNullException(
                    "[reportConfiguration] No report was found in ApplicationCache for requested report!");

            var fields = reportRequest.ReportSchedule switch
            {
                ReportSchedule.Daily => reportConfiguration.FieldsDaily,
                ReportSchedule.Monthly => reportConfiguration.FieldsMonthly,
                _ => throw new ArgumentException("[fields] Invalid report schedule!")
            };

            reportConfiguration.Report = reportRequest.Report;
            reportConfiguration.ReportSchedule = reportRequest.ReportSchedule;

            reportConfiguration.DataLoadStoredProcedure = reportRequest.ReportSchedule switch
            {
                ReportSchedule.Daily => reportConfiguration.DataLoadStoredProcedureDaily,
                ReportSchedule.Monthly => reportConfiguration.DataLoadStoredProcedureMonthly,
                _ => throw new ArgumentException("[loaderStoredProcedure] Invalid report schedule!")
            };

            reportConfiguration.NormalizationStoredProcedure = reportRequest.ReportSchedule switch
            {
                ReportSchedule.Daily => reportConfiguration.NormalizationStoredProcedureDaily,
                ReportSchedule.Monthly => reportConfiguration.NormalizationStoredProcedureMonthly,
                _ => throw new ArgumentException("[normalizationStoredProcedure] Invalid report schedule!")
            };

            var dateRange = new DateRange
            {
                StartDate = reportRequest.DateRange.StartDate.Value,
                EndDate = reportRequest.DateRange.EndDate.Value
            };


            foreach (var googleAccount in GoogleAccountsCache.Accounts)
            {
                var report = RequestPayloadGenerator.GenerateRequest(_superMetricsApiKey, dateRange, fields,
                    reportConfiguration.DataSet.Id, reportConfiguration.DataSet.User, googleAccount.ViewId);
                var jsonString = JsonConvert.SerializeObject(new ReportDownloadRequest<R>
                {
                    ReportConfiguration = reportConfiguration,
                    RequestPayload = report
                });
                var bodyBytes = Encoding.UTF8.GetBytes(jsonString);
                _channelReports.BasicPublish(_exchangeReports, routingKey: _rabbitMqQueueRoutingKey, body: bodyBytes);
            }
        }


        /// <summary>
        /// Garbage cleanup.
        /// </summary>
        public void Dispose()
        {
            _channelReports?.Close();
            _channelReports?.Dispose();
            _connection?.Dispose();
        }
    }
}