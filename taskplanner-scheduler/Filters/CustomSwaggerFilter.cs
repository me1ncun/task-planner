using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace taskplanner_scheduler.Filters;

public class CustomSwaggerFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var nonMobileRoutes = swaggerDoc.Paths
            .Where(x => !x.Key.ToLower().Contains("kafka/send"))
            .ToList();
        nonMobileRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
    }
}