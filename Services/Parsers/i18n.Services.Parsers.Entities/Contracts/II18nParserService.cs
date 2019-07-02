namespace i18n.Services.Parsers.Entities.Contracts
{
    /// <summary>
    /// Parsing service for converting a source to an i18n format
    /// </summary>
    public interface II18nParserService
    {
        /// <summary>
        /// Creates an I18n HTML template and translation file (PO file) from an HTML page
        /// Returns true if the process was successful and false otherwise
        /// </summary>
        /// <param name="htmlPath">THe HTML file path</param>
        /// <param name="outputName">Output folder name (Does not include path at the moment)</param>
        /// <param name="sourceLanguage">Current language used in the HTML file</param>
        /// <param name="targetLanguage">The desired translation language</param>
        /// <returns>Returns true if the process was successful and false otherwise</returns>
        bool CreateI18nFromHTML(string htmlPath, string outputName, string sourceLanguage, string targetLanguage);
    }
}
