using i18n.Infrastructure.Entities.Contracts.IO;
using i18n.Services.GetText.Data.Enums;
using i18n.Services.GetText.Entities.Contracts;
using i18n.Services.Parsers.Data.Enums;
using i18n.Services.Parsers.Entities.Contracts;
using i18n.Services.Translator.Entities.Contracts;
using i18n.Services.Translator.Entities.Contracts.Providers;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace i18n.Services.Translator
{
    /// <summary>
    /// Service that provides functionality for translation
    /// </summary>
    public class TranslatorService : ITranslatorService
    {
        #region Private Readonly Members

        private readonly IFileWrapper _fileWrapper = null;
        private readonly IGoogleTranslateProvider _googleTranslateProvider = null;
        private readonly IHtmlParserService _htmlParser = null;
        private readonly ILogger<TranslatorService> _logger = null;
        private readonly IPOFileService _poFileService = null;

        #endregion

        #region CTOR

        public TranslatorService(IFileWrapper fileWrapper, IGoogleTranslateProvider googleTranslateProvider, IHtmlParserService htmlParser, 
                                 ILogger<TranslatorService> logger, IPOFileService poFileService)
        {
            _fileWrapper = fileWrapper;
            _googleTranslateProvider = googleTranslateProvider;
            _htmlParser = htmlParser;
            _logger = logger;
            _poFileService = poFileService;
        }

        #endregion

        /// <summary>
        /// Creates a translated HTML from using an i18n HTML template, from a selected source language to target language.
        /// If a translation file cannot be found locally (PO file), the HTML file will be translated
        /// using those translations, otherwise the translation will be made using Google Translate.
        /// In case Google Translate will be used, additionaly a translation file (PO file) will be created
        /// for future use.
        /// </summary>        
        /// <param name="i18nHTMLFilePath">The i18n HTML file path</param>
        /// <param name="outputName">Output folder name (Does not include path at the moment)</param>
        /// <param name="sourceHTMLFileName">The source HTML file name</param>
        /// <param name="sourceLanguage">Current language used in the HTML file</param>
        /// <param name="targetLanguage">The desired translation language</param>
        public void CreateTranslatedHTMLFromI18n(string i18nHTMLFilePath, string outputName, string sourceHTMLFileName, string sourceLanguage, string targetLanguage)
        {
            var poFullFilePath = _fileWrapper.CreatePathFromAssemblyPath($"{sourceHTMLFileName}_{targetLanguage}", GetTextFileTypes.PO, $"{outputName}\\{sourceHTMLFileName}\\");

            string updatedHTML;
            if (_fileWrapper.Exists(poFullFilePath))
            {
                var poContent = _poFileService.Parse(poFullFilePath);
                if (poContent == null)
                {
                    Console.WriteLine($"Failed to parse PO file in path '{poFullFilePath}' with language '{targetLanguage}'. Check log for additional information");
                    return;
                }

                updatedHTML = _htmlParser.UpdateHTML(i18nHTMLFilePath, poContent);
            }
            else
            {
                poFullFilePath = _fileWrapper.CreatePathFromAssemblyPath($"{sourceHTMLFileName}_{sourceLanguage}", GetTextFileTypes.PO, $"{outputName}\\{sourceHTMLFileName}\\");

                Console.WriteLine($"Parsing translations from a translation (PO) file in path '{poFullFilePath}' with language '{targetLanguage}'..");
                var poContent = _poFileService.Parse(poFullFilePath);
                var translatedTexts = _googleTranslateProvider.TranslateTexts(sourceLanguage, targetLanguage, poContent);

                Console.WriteLine($"Creating a translation (PO) file with name '{sourceHTMLFileName}' for language '{targetLanguage}'..");
                var poFileResult = _poFileService.Create(Encoding.UTF8, translatedTexts, targetLanguage, sourceHTMLFileName);
                if (!poFileResult)
                    Console.WriteLine($"Failed to create POT file '{sourceHTMLFileName}' with language '{sourceLanguage}. Check log for additional information");

                updatedHTML = _htmlParser.UpdateHTML(i18nHTMLFilePath, translatedTexts);
            }

            var htmlFullFilePath = _fileWrapper.CreatePathFromAssemblyPath($"{sourceHTMLFileName}_{targetLanguage}", ParserFileTypes.HTML, $"{outputName}\\{sourceHTMLFileName}\\");
            var fileCreateResult = _fileWrapper.Create(Encoding.UTF8, updatedHTML, htmlFullFilePath);
            if (fileCreateResult != null)
                _logger.LogError($"Failed to create file '{sourceHTMLFileName}_{targetLanguage} in path '{htmlFullFilePath}'");

            Console.WriteLine("Finished translating the HTML page");
        }
    }
}
