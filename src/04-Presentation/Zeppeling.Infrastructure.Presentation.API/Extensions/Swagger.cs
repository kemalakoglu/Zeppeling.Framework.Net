using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Zeppeling.Infrastructure.Presentation.API.Extensions
{
    public static class Swagger
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("ZeppelingInfrastructure", new OpenApiInfo
                {
                    Title = "Zeppeling.Infrastructure API",
                    Version = "Zeppeling.Infrastructure",
                    Description = "Zeppeling.Infrastructure Web API Documentation",
                });
            });
        }
    }
}