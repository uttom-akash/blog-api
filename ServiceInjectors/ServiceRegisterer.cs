using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Blog_Rest_Api.Exceptions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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

        public static void AddJwtBearer(this IServiceCollection services){

            services.AddAuthentication(authOptions=>{
                authOptions.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOptions=>{
                jwtOptions.SaveToken=true;
                jwtOptions.TokenValidationParameters=new TokenValidationParameters{
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateIssuerSigningKey=true,
                    
                    ValidIssuer="cefalo.com",
                    ValidAudience="cefalo",
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2fb3daa887474f18a6ed7b30f2e26b1c"))
                };
            });

        }
    }
}
