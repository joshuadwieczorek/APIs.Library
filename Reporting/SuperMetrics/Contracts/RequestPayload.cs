using System.Collections.Generic;
using Newtonsoft.Json;

namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    public sealed class RequestPayload
    {
        [JsonProperty("ds_id")]
        public string DataSetId { get; set; }

        [JsonProperty("ds_accounts")]
        public object DataSetAccounts { get; set; }

        [JsonProperty("ds_user")]
        public string DataSetUser { get; set; }

        [JsonProperty("date_range_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DateRangeType { get; set; }

        [JsonProperty("start_date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StartDate { get; set; }

        [JsonProperty("end_date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EndDate { get; set; }

        [JsonProperty("fields")]
        public List<RequestPayloadField> Fields { get; set; }

        [JsonProperty("max_rows")]
        public int MaxRows { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("settings", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RequestPayloadSettings Settings { get; set; }
    }
}