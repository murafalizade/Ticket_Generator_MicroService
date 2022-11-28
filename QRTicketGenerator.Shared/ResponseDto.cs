using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QRTicketGenerator.Shared
{
    public class ResponseDto<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccessfull { get; set; }
        public List<string> Errors { get; set; }

        public static ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };
        }

        public static ResponseDto<T> Fail(string error,int statusCode)
        {
            return new ResponseDto<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessfull = false };
        }
    }
}
