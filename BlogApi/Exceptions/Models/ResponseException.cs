using System;
using Microsoft.AspNetCore.Http;

namespace Blog_Rest_Api.Exceptions{
    public class ResponseException
    {
        public int Status { get; set; } = StatusCodes.Status500InternalServerError;

        public string Message { get; set; }
        public string Source {get;set;}
    }

}