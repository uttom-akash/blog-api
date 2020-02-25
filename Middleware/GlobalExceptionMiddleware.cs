using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Blog_Rest_Api.Middleware{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {   
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
        
    }


} 