namespace APIs.Library.Reporting.Contracts
{
    public interface IHaveReportStartAndEndDates
    {
        string ReportStartDate { get; set; }
        string ReportEndDate { get; set; }
    }
}