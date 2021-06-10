using Newtonsoft.Json;

namespace APIs.Library.Reporting.Contracts
{
    public class HttpRequestResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }


        /// <summary>
        /// Plain constructor.
        /// </summary>
        public HttpRequestResponse() { }


        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message"></param>
        public HttpRequestResponse(string message)
        {
            Message = message;
        }
    }
}