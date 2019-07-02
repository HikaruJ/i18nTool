using i18n.Infrastructure.Entities.Contracts.IO;
using i18n.Services.GetText.Data.Enums;
using i18n.Services.GetText.Entities.Contracts;
using Karambolo.Common.Collections;
using Karambolo.PO;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;

namespace i18n.Services.GetText
{
    /// <summary>
    /// Service for handling operations for a PO (translation) file
    /// </summary>
    public class POFileService : IPOFileService
    {
        #region Private Readonly Members

        private readonly IGetTextCatalogService _catalog = null;
        private readonly IDirectoryWrapper _directoryWrapper = null;
        private readonly IFileWrapper _fileWrapper = null;
        private readonly ILogger<POFileService> _logger = null;

        #endregion

        #region CTOR

        public POFileService(IGetTextCatalogService catalog, IDirectoryWrapper directoryWrapper, IFileWrapper fileWrapper, ILogger<POFileService> logger)
        {
            _catalog = catalog;
            _directoryWrapper = directoryWrapper;
            _fileWrapper = fileWrapper;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a PO (translation) file from a set of translation keys for a specific encoding and language
        /// Returns true if created succesfully and false otherwise
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextTranslations">Translations dictionary</param>
        /// <param name="language">Language used for the translations</param>
        /// <param name="outputFileName">Output file name</param>
        /// <returns>Returns true if created succesfully and false otherwise</returns>
        public bool Create(Encoding fileEncoding, IOrderedDictionary<string, string> htmlTextTranslations, string language, string outputFileName)
        {
            var catalog = _catalog.CreateCatalog(fileEncoding, htmlTextTranslations, language);
            var generator = new POGenerator(new POGeneratorSettings() { IgnoreEncoding = true });
            var stringBuilder = new StringBuilder();

            var outputPath = $"{_fileWrapper.AssemblyPath()}\\output\\{outputFileName}\\";
            var directoyCreateResult = _directoryWrapper.Create(outputPath);
            if (directoyCreateResult != null)
                return false;

            try
            {
                generator.Generate(stringBuilder, catalog);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create PO file {outputFileName}' with language '{language}'");
                return false;
            }

            var fullFilePath = $"{outputPath}\\{outputFileName}_{language}.{GetTextFileTypes.PO}";
            var fileCreateResult = _fileWrapper.Create(fileEncoding, stringBuilder, fullFilePath);
            if (fileCreateResult != null)
                return false;

            return true;
        }

        /// <summary>
        /// Parses a PO (translation file) in a given path
        /// and returns a dictionary containing the translations in file
        /// </summary>
        /// <param name="fullFilePath">File Path</param>
        /// <returns>Returns a dictionary containing the translations in file</returns>
        public IOrderedDictionary<string, string> Parse(string fullFilePath)
        {
            var parser = new POParser(new POParserSettings());
            string readerInput = _fileWrapper.Read(fullFilePath);
            if (string.IsNullOrEmpty(readerInput))
                return null;

            var result = parser.Parse(readerInput);
            if (result.Success)
            {
                var poFileValues = new OrderedDictionary<string, string>();
                foreach (var poEntry in result.Catalog.Values)
                {
                    var key = poEntry.Key.Id;
                    var value = poEntry.FirstOrDefault();
                    poFileValues.Add(key, value);
                }

                return poFileValues;
            }
            else
            {
                var diagnostics = result.Diagnostics;
                _logger.LogError($"Failed to parse PO file in path '{fullFilePath}'. Diagnostics = '{diagnostics}'");
                return null;
            }
        }

        #endregion
    }
}
