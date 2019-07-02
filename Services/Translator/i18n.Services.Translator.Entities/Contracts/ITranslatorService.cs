using System.Collections.Generic;

namespace i18n.Services.Translator.Entities.Contracts
{
    /// <summary>
    /// Service that provides functionality for translation
    /// </summary>
    public interface ITranslatorService
    {
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
        void CreateTranslatedHTMLFromI18n(string i18nHTMLFilePath, string outputName, string sourceHTMLFileName, string sourceLanguage, string targetLanguage);
    }
}
