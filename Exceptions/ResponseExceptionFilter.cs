using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Blog_Rest_Api.Exceptions{
    public class ResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger logger;

        public ResponseExceptionFilter()
        {
            logger=LoggerFactory.Create(builder=>builder.AddConsole()).CreateLogger<ResponseExceptionFilter>();
        }

        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public  void OnActionExecuted(ActionExecutedContext context)
        {
            Exception exception=context.Exception;
            if(exception==null)
                return;
            while (exception.InnerException!=null)
            {
                exception=exception.InnerException;   
            }

            ResponseException responseException=new ResponseException{
                Message=exception.Message,
                Source=exception.Source
            };
            context.Result = new ObjectResult(responseException)
            {
                StatusCode = responseException.Status,
            };
            context.ExceptionHandled = true;
            

            logger.LogError(10001,responseException.Message);
            context.ExceptionHandled = true;

        }
    }
}