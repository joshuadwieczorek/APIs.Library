using Newtonsoft.Json;

namespace APIs.Library.Reporting.SuperMetrics.Contracts
{
    public sealed class RequestPayloadField
    {
        [JsonProperty("id")]
        public string Id { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"></param>
        public RequestPayloadField(string id)
        {
            Id = id;
        }
    }
}