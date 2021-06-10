namespace APIs.Library.Reporting
{
    public class StaticText
    {
        public const string SuperMetricsBaseApiUrl = "SuperMetricsBaseApiUrl";
        public const string SuperMetricsApiKey = "SuperMetricsApiKey";
        public const string CsvDelimiter = "CsvDelimiter";
        public const string RabbitMqServer = "RabbitMQ.Server";
        public const string RabbitMqVHost = "RabbitMQ.VHost";
        public const string RabbitMqPort = "RabbitMQ.Port";
        public const string RabbitMqUser = "RabbitMQ.User";
        public const string RabbitMqPassword = "RabbitMQ.Password";
        public const string RabbitMqQueueNameLogs = "RabbitMQ.QueueName.Logs";
        public const string RabbitMqQueueNameReportsToLoad = "RabbitMQ.QueueName.ReportsToLoad";
        public const string RabbitMqQueueNameReportsToDownload = "RabbitMQ.QueueName.Reports";
        public const string RabbitMqExchangeLogs = "RabbitMQ.Exchange.Logs";
        public const string RabbitMqExchangeReports = "RabbitMQ.Exchange.Reports";
        public const string RabbitMqQueueRoutingKey = "RabbitMQ.Queue.RoutingKey";
        public const string ApplicationName = "AppName";
        public const string ApplicationType = "AppType";
        public const string ApplicationReleaseStage = "AppReleaseStage";
        public const string ApplicationReleaseVersion = "AppReleaseVersion";
        public const string BugSnagApiKey = "BugSnagApiKey";
        public const string ApiResponseSuccess = "success";
        public const string ApiResponseFailed = "failed";
        public const string ReportSuccessfullyQueued = "Report successfully queued!";
        public const string InternalServerErrorOccurred = "An internal error occurred!";
        public const string DirectoryDaily = "Daily";
        public const string DirectoryMonthly = "Monthly";
        public const string InboundDirectory = "Inbound";
        public const string ArchiveDirectory = "Archive";
        public const string FailedDirectory = "Failed";
        public const string ReportsToDownloadInParallel = "ReportsToDownloadInParallel";
    }
}