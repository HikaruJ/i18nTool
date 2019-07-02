using i18n.Infrastructure.Entities.Contracts.IO;
using i18n.Infrastructure.IO;
using Microsoft.Extensions.DependencyInjection;

namespace i18n.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
        {
            services
                .AddTransient<IDirectoryWrapper, DirectoryWrapper>()
                .AddTransient<IFileWrapper, FileWrapper>();

            return services;
        }
    }
}
