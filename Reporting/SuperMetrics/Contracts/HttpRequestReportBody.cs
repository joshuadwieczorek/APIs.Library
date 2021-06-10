using APIs.Library.Contracts;
using APIs.Library.Enums;
using Newtonsoft.Json;

namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    public class HttpRequestReportBody
    {
        [JsonProperty("schedule")]
        public ReportSchedule Schedule { get; set; }

        [JsonProperty("report")]
        public Report Report { get; set; }

        [JsonProperty("dateRange")]
        public HttpRequestDateRange DateRange { get; set; }
    }
}