using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackendTask.Shared.ResultDtos
{
    [Serializable]
    public class WrappedResultDto
    {
        [JsonProperty("status")]
        public bool Success { get; set; }

        [JsonProperty("code")]
        public int StatusCode { get; set; }

        [JsonProperty("data")]
        public object Result { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public WrappedResultDto(object result, int statusCode = 200, string message = "")
        {
            StatusCode = statusCode;
            Result = result;
            Success = statusCode >= 200 && statusCode <= 299;
            Message = message;
        }

        public WrappedResultDto(ErrorResultDto error, int statusCode = 500)
        {
            StatusCode = statusCode;
            Success = false;
            Message = error?.Message;
            Result = null;
        }
    }
}
