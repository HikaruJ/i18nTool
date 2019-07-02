using i18n.Services.Parsers.Entities.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace i18n.Services.Parsers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureParsersServices(this IServiceCollection services)
        {
            services
                .AddTransient<IHtmlParserService, HtmlParserService>()
                .AddTransient<II18nParserService, I18nParserService>();

            return services;
        }
    }
}
