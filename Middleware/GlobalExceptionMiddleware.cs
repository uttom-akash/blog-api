using System;
using System.Threading.Tasks;
using Blog_Rest_Api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Blog_Rest_Api.Middleware{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
            logger=LoggerFactory.Create(builder=>builder.AddConsole()).CreateLogger<ResponseExceptionFilter>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception ex)
            {   
                while (ex.InnerException!=null)
                    {
                        ex=ex.InnerException;   
                    }
                logger.LogError(10002,ex.Message);
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
        
    }


} 