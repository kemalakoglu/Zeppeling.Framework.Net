using Microsoft.Extensions.DependencyInjection;

namespace Zeppeling.Infrastructure.Presentation.API.Extensions
{
    public static class FluentValidator
    {
        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            //services.AddTransient<IValidator<Dealer>, DealerValidator>();
        }
    }
}