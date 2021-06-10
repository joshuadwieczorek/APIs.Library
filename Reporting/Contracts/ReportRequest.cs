using APIs.Library.Contracts;
using APIs.Library.Enums;

namespace APIs.Library.Reporting.Contracts
{
    public sealed class ReportRequest
    {
        public Report Report { get; set; }
        public ReportSchedule ReportSchedule { get; set; }
        public HttpRequestDateRange DateRange { get; set; }
    }
}