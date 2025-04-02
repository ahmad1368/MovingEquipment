using Common.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WebFramework.Swagger
{
    public static class SwaggerConfigurationExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            Assert.NotNull(services, nameof(services));

            // Add services and configuration to use Swagger
            services.AddSwaggerGen(options =>
            {
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "Contractor.xml");
                // Show controller XML comments like summary
                options.IncludeXmlComments(xmlDocPath, true);

                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "API V1" });
                options.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "API V2" });

                #region Filters
                options.OperationFilter<ApplySummariesOperationFilter>();
                #endregion

                #region Add UnAuthorized to Response
                // Add 401 response and security requirements (Lock icon) to actions that need authorization
                options.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "Bearer");
                #endregion

                #region Add Jwt Authentication
                // Add Lockout icon on top of swagger UI page to authenticate
                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    In = ParameterLocation.Header,
                //});

                var isDeveloapment = services.BuildServiceProvider().GetService<IHostEnvironment>().IsDevelopment();

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                                
                       Password = new OpenApiOAuthFlow
                       {
                           TokenUrl = (isDeveloapment ? new Uri("https://localhost:7247/api/v1/User/Token"): new Uri("https://MyLoadWebsite/api/v1/User/Token")),
                           Scopes = new Dictionary<string, string>
                            {
                                { "api.read", "Read access to protected resources" },
                                { "api.write", "Write access to protected resources" }
                            }
                       }
                    }
                });

                #endregion

                


                #region Versioning
                // Remove version parameter from all Operations
                options.OperationFilter<RemoveVersionParameters>();

                // Set version "api/v{version}/[controller]" from current swagger doc version
                options.DocumentFilter<SetVersionInPaths>();

                // Separate and categorize end-points by doc version
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
                #endregion
            });
        }

        public static void UseSwaggerAndUI(this IApplicationBuilder app)
        {
            Assert.NotNull(app, nameof(app));

            // Swagger middleware for generating "Open API Documentation" in swagger.json
            app.UseSwagger();

            // Swagger middleware for generating UI from swagger.json
            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId("your-client-id");
                options.OAuthClientSecret("your-client-secret");
                options.OAuthUsePkce();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Docs");
            });
        }
    }
}
