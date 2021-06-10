using Newtonsoft.Json;

namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    public sealed class RequestPayloadSettings
    {
        [JsonProperty("conversion_window", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ConversionWindow { get; set; }

        [JsonProperty("timezone", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Timezone { get; set; }

        [JsonProperty("action_report_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ActionReportTime { get; set; }
    }
}