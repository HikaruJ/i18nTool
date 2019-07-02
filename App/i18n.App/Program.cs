using CommandLine;
using i18n.App.Entites;
using i18n.Infrastructure;
using i18n.Infrastructure.Entities.Contracts.IO;
using i18n.Services.GetText;
using i18n.Services.Parsers;
using i18n.Services.Parsers.Data.Enums;
using i18n.Services.Parsers.Entities.Contracts;
using i18n.Services.Translator;
using i18n.Services.Translator.Entities.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace i18n
{
    /// <summary>
    /// A sample program in C# for pre-processing HTML files, 
    /// Extracting text into POT and PO files for translators and 
    /// translating the html to a targeted language
    /// </summary>
    class Program
    {
        #region Main 

        /// <summary>
        /// Main execution method for the App
        /// 1. Defines the dependency injection for the system
        /// 2. Defines logging for the system (saving to an external text file as well as in console)
        /// 3. Parses and validates arguments sent to the app through the console, before executing the app
        /// 4. Executes the translation process
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.File("i18n.log")
              .CreateLogger();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogInformation("Started ");

            var result = Parser.Default.ParseArguments<ConsoleArguments>(args)
                  .WithParsed(option =>
                  {
                      Console.WriteLine("i18n Service Started..");

                      // For non-required parameters, a default will be set
                      if (string.IsNullOrEmpty(option.OutputName))
                          option.OutputName = "output";

                      if (string.IsNullOrEmpty(option.SourceLanguage))
                          option.SourceLanguage = "en-us";

                      // TODO: For future versions this can be extended to support multiple HTML files and translation languages
                      // Also this functionality can be separated to its own Utility
                      var i18nParser = serviceProvider.GetService<II18nParserService>();
                      var i18nParserResult = i18nParser.CreateI18nFromHTML(option.HTMLPath, option.OutputName, option.SourceLanguage, option.TargetLanguage);
                      if (!i18nParserResult)
                          return;

                      // TODO: For future versions this can be extended to support multiple HTML files and translation languages
                      // Also this functionality can be separated to its own Utility
                      var translatorService = serviceProvider.GetService<ITranslatorService>();
                      var sourceHTMLFileName = Path.GetFileNameWithoutExtension(option.HTMLPath);
                      var fileWrapper = serviceProvider.GetService<IFileWrapper>();
                      var i18nHTMLPath = fileWrapper.CreatePathFromAssemblyPath($"{sourceHTMLFileName}_i18n", ParserFileTypes.HTML, $"{option.OutputName}\\{sourceHTMLFileName}");
                      translatorService.CreateTranslatedHTMLFromI18n(i18nHTMLPath, option.OutputName, sourceHTMLFileName, option.SourceLanguage, option.TargetLanguage);
                  });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add to the service container dependencies configured across the system
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging(configure => configure.AddSerilog())
                .ConfigureInfrastructure()
                .ConfigureGetTextServices()
                .ConfigureParsersServices()
                .ConfigureTranslatorServices();
        }

        #endregion
    }
}