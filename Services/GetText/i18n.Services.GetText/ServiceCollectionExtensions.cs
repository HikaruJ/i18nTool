using i18n.Services.GetText.Entities.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace i18n.Services.GetText
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureGetTextServices(this IServiceCollection services)
        {
            services
                .AddTransient<IGetTextCatalogService, GetTextCatalogService>()
                .AddTransient<IPOFileService, POFileService>()
                .AddTransient<IPOTFileService, POTFileService>();

            return services;
        }
    }
}
