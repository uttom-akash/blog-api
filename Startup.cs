using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.Middleware;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AutoMapper;

namespace Blog_Rest_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.ConfigureDatabaseInfo(Configuration);

            // BuiltIn 
            services.AddControllers()
                .AddXmlSerializerFormatters();
                
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<BlogContext>();
            services.AddSwagger();

            // Custom
            services.AddScoped<IStoriesRepository,StoriesRepository>();
            services.AddScoped<IStoriesService,StoriesService>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog-Rest-Api"));

            app.UseRouting();
            app.UseEndpoints(endpoints =>endpoints.MapControllers());

            
            //Incase No Matching Route
            app.Run(async context=>await context.Response.WriteAsync("No route!!"));
        }
    }
}
