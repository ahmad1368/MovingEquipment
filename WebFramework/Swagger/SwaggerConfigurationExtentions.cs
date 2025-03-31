using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebFramework.Configuration;

namespace WebFramework.Swagger
{
    public static class SwaggerConfigurationExtentions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var xmlDocPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Contractor.xml";
                options.IncludeXmlComments(xmlDocPath, true);

                

                options.DescribeAllParametersInCamelCase();

                // تعریف نسخه‌های مختلف API
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "1.0" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "My API", Version = "2.0" });

                // تنظیم فیلتر برای نمایش نسخه‌های مختلف در Swagger
                options.OperationFilter<ApiVersioningOperationFilter>();
            });
        }
        public static void UseSwaggerAndUI(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "First Version");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Second Version");
            });
        }

    }
}
