using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace APIs.Library.Contracts
{
    public class HttpRequestDateRange
    {
        [JsonProperty("startDate"), Required]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate"), Required]
        public DateTime? EndDate { get; set; }
    }
}