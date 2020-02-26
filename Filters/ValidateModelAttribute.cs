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
            string errorMessage = string.Join(", ", context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            context.HttpContext.Response.StatusCode=StatusCodes.Status400BadRequest;
            await context.HttpContext.Response.WriteAsync(errorMessage); 
            }
        }
    }
}