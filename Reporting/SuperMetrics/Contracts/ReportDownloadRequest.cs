namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    public class ReportDownloadRequest
    {
        public ReportConfiguration ReportConfiguration { get; set; }
        public RequestPayload RequestPayload { get; set; }
    }


    public sealed class ReportDownloadRequest<T> : ReportDownloadRequest { }
}