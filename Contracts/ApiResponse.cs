using System;
using System.Net;
using Newtonsoft.Json;

namespace APIs.Library.Contracts
{
    public class ApiResponse
    {
        [JsonProperty("serverTime")]
        public DateTime ServerTime { get; set; } = DateTime.Now;

        [JsonProperty("status")]
        public HttpStatusCode Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }


        public ApiResponse(
            string message = null
            , HttpStatusCode status = HttpStatusCode.OK)
        {
            Status = status;
            Message = message;
        }
    }


    public class ApiResponse<T> : ApiResponse
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        public ApiResponse(
            T data
            , string message = null
            , HttpStatusCode status = HttpStatusCode.OK) : base(message, status)
        {
            Data = data;
        }
    }
}