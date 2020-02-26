using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Blog_Rest_Api.Exceptions;

namespace Blog_Rest_Api{
    public static class ServiceRegisterer
    {
        public static void AddSwagger(this IServiceCollection services){
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog Rest Api", Version = "v1" });
                });
        }

        public static void AddCustomControllers(this IServiceCollection services){
            services.AddControllers(options =>
                        options.Filters.Add(new ResponseExceptionFilter())
                )
                .AddXmlSerializerFormatters();
        }
    }
}
