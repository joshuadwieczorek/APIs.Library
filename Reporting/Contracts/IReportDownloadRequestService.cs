namespace APIs.Library.Reporting.Contracts
{
    public interface IReportDownloadRequestService<T>
    {
        /// <summary>
        /// Generate download report request from report request.
        /// </summary>
        /// <param name="reportRequest"></param>
        void GenerateReportDownloadRequest(ReportRequest reportRequest);
    }
}