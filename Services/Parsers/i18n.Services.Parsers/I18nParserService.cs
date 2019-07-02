using i18n.Infrastructure.Entities.Contracts.IO;
using i18n.Services.GetText.Entities.Contracts;
using i18n.Services.Parsers.Data.Enums;
using i18n.Services.Parsers.Entities.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace i18n.Services.Parsers
{
    /// <summary>
    /// Parsing service for converting a source to an i18n format
    /// </summary>
    public class I18nParserService : II18nParserService
    {
        #region Private Readonly Members

        private readonly IDirectoryWrapper _directoryWrapper = null;
        private readonly IFileWrapper _fileWrapper = null;
        private readonly IHtmlParserService _htmlParser = null;
        private readonly ILogger<I18nParserService> _logger = null;
        private readonly IPOFileService _poFileService = null;
        private readonly IPOTFileService _potFileService = null;

        #endregion

        #region CTOR

        public I18nParserService(IDirectoryWrapper directoryWrapper, IFileWrapper fileWrapper, IHtmlParserService htmlParser, ILogger<I18nParserService> logger, IPOFileService poFileService, IPOTFileService potFileService)
        {
            _directoryWrapper = directoryWrapper;
            _fileWrapper = fileWrapper;
            _htmlParser = htmlParser;
            _logger = logger;
            _poFileService = poFileService;
            _potFileService = potFileService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an I18n HTML template and translation file (PO file) from an HTML page
        /// Returns true if the process was successful and false otherwise
        /// </summary>
        /// <param name="htmlPath">THe HTML file path</param>
        /// <param name="outputName">Output folder name (Does not include path at the moment)</param>
        /// <param name="sourceLanguage">Current language used in the HTML file</param>
        /// <param name="targetLanguage">The desired translation language</param>
        /// <returns>Returns true if the process was successful and false otherwise</returns>
        public bool CreateI18nFromHTML(string htmlPath, string outputName, string sourceLanguage, string targetLanguage)
        {
            var htmlFileName = Path.GetFileNameWithoutExtension(htmlPath);
            var uniqueIdsResult = _htmlParser.GenerateUniqueIds(htmlPath);
            if (uniqueIdsResult == null)
            {
                Console.WriteLine($"Failed to parse HTML in path '{htmlPath}. Check log for additional infromation");
                return false;
            }

            Console.WriteLine($"Creating a translation (PO) file with name '{htmlFileName}' for language '{sourceLanguage}'..");
            var poFileResult = _poFileService.Create(Encoding.UTF8, uniqueIdsResult.HTMLOriginalValues, sourceLanguage, htmlFileName);
            if (!poFileResult)
                Console.WriteLine($"Failed to create PO file '{htmlFileName}' with language '{sourceLanguage}. Check log for additional information");

            Console.WriteLine($"Creating a translation (POT) file with name '{htmlFileName}' for language '{targetLanguage}'..");
            var potFileResult = _potFileService.Create(Encoding.UTF8, uniqueIdsResult.HTMLTextKeys, targetLanguage, htmlFileName);
            if (!potFileResult)
                Console.WriteLine($"Failed to create POT file '{htmlFileName}' with language '{targetLanguage}. Check log for additional information");

            var outputPath = $"{_fileWrapper.AssemblyPath()}\\{outputName}\\{htmlFileName}";
            var directoyCreateResult = _directoryWrapper.Create(outputPath);
            if (directoyCreateResult != null)
            {
                Console.WriteLine($"Failed to create a directory in path '{outputName}'. Check log for additional information");
                return false;
            }

            var fullFilePath = _fileWrapper.CreatePathFromAssemblyPath($"{htmlFileName}_i18n", ParserFileTypes.HTML, $"{outputName}\\{htmlFileName}");
            var fileCreateResult = _fileWrapper.Create(Encoding.UTF8, uniqueIdsResult.UpdatedHTML, fullFilePath);
            if (fileCreateResult != null)
            {
                Console.WriteLine($"Failed to create file in path '{fullFilePath}'. Check log for additional information");
                return false;
            }

            return true;
        }

        #endregion
    }
}
