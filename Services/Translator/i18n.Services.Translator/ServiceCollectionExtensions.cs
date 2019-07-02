using i18n.Services.Translator.Data.Helpers;
using i18n.Services.Translator.Entities.Contracts;
using i18n.Services.Translator.Entities.Contracts.Helpers;
using i18n.Services.Translator.Entities.Contracts.Mock;
using i18n.Services.Translator.Entities.Contracts.Providers;
using i18n.Services.Translator.Mock;
using i18n.Services.Translator.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace i18n.Services.Translator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureTranslatorServices(this IServiceCollection services)
        {
            services
                .AddTransient<IGoogleTranslateMock, GoogleTranslateMock>()
                .AddTransient<IGoogleTranslateProvider, GoogleTranslateProvider>()
                .AddTransient<ITextHelper, TextHelper>()
                .AddTransient<ITranslatorService, TranslatorService>();

            return services;
        }
    }
}
