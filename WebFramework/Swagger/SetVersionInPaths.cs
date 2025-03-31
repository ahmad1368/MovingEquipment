using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace WebFramework.Swagger
{
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var updatedPaths = new OpenApiPaths(); // Create OpenApiPaths instead of Dictionary

            foreach (var entry in swaggerDoc.Paths)
            {
                // Replace the version placeholder with the actual version in the path
                updatedPaths.Add(
                    entry.Key.Replace("v{version}", swaggerDoc.Info.Version),
                    entry.Value);
            }

            swaggerDoc.Paths = updatedPaths; // Assign the updated paths to the swaggerDoc
        }
    }
}
