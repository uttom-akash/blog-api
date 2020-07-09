using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog_Rest_Api.Custom_Attribute{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Console.WriteLine("---------------------------------------akash-----------------------------------------------");
                var a=context.ModelState.Values.ToArray();
                foreach (var item in a)
                {
                    Console.WriteLine(item.Errors);
                }
                Console.WriteLine("------------------------------------------------------------------------------------------");
                
                // string errorMessage = string.Join(", ", context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                // context.HttpContext.Response.StatusCode=StatusCodes.Status400BadRequest;
                string errorMessage="";
                await context.HttpContext.Response.WriteAsync(errorMessage); 
            }
        }
    }
}