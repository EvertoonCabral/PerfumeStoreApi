using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PerfumeStoreApi.Filters
{
    public class OpenApiVersionDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Info.Version = "v1";
            
            if (string.IsNullOrEmpty(swaggerDoc.Info.Title))
            {
                swaggerDoc.Info.Title = "Store API";
            }
        }
    }
}