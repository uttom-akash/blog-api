using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog_Rest_Api{
    public static class ServiceConfiguration
    {
        public static void ConfigureDatabaseInfo(this IServiceCollection services,IConfiguration configuration){
            services.Configure<DatabaseInfo>(configuration.GetSection("DatabaseInfo"));
        }

        public static void ConfigureJwtInfo(this IServiceCollection services,IConfiguration configuration){
            services.Configure<JwtInfo>(configuration.GetSection("JwtInfo"));
        }
    }
}