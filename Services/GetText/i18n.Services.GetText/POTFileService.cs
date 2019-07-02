using i18n.Infrastructure.Entities.Contracts.IO;
using i18n.Services.GetText.Data.Enums;
using i18n.Services.GetText.Entities.Contracts;
using Karambolo.PO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace i18n.Services.GetText
{
    /// <summary>
    /// Service for handling operations for a POT (translation) file
    /// </summary>
    public class POTFileService : IPOTFileService
    {
        #region Private Readonly Members

        private readonly IGetTextCatalogService _catalog = null;
        private readonly IDirectoryWrapper _directoryWrapper = null;
        private readonly IFileWrapper _fileWrapper = null;
        private readonly ILogger<POTFileService> _logger = null;

        #endregion

        #region CTOR

        public POTFileService(IGetTextCatalogService catalog, IDirectoryWrapper directoryWrapper, IFileWrapper fileWrapper, ILogger<POTFileService> logger)
        {
            _catalog = catalog;
            _directoryWrapper = directoryWrapper;
            _fileWrapper = fileWrapper;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a POT (translation) file from a list of translations for a specific encoding and language
        /// Returns true if created succesfully and false otherwise
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextKeys">A list of translations</param>
        /// <param name="language">Language used for the translations</param>
        /// <param name="outputFileName">Output file name</param>
        /// <returns>Returns true if created succesfully and false otherwise</returns>
        public bool Create(Encoding fileEncoding, IList<string> htmlTextKeys, string language, string outputFileName)
        {
            var catalog = _catalog.CreateCatalog(fileEncoding, htmlTextKeys, language);
            var generator = new POGenerator(new POGeneratorSettings() { IgnoreEncoding = true });
            var stringBuilder = new StringBuilder();

            var outputPath = $"{_fileWrapper.AssemblyPath()}\\output\\{outputFileName}\\";
            var directoyCreateResult =_directoryWrapper.Create(outputPath);
            if (directoyCreateResult != null)
                return false;

            try
            {
                generator.Generate(stringBuilder, catalog);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create POT file {outputFileName}' with language '{language}'");
                return false;
            }

            var fullFilePath = _fileWrapper.CreatePathFromAssemblyPath($"{outputFileName}_{language}", GetTextFileTypes.POT, $"output\\{outputFileName}\\");
            var fileCreateResult = _fileWrapper.Create(fileEncoding, stringBuilder, fullFilePath);
            if (fileCreateResult != null)
                return false;

            return true;
        }

        #endregion
    }
}
