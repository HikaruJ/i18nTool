using Karambolo.Common.Collections;
using System.Text;

namespace i18n.Services.GetText.Entities.Contracts
{
    /// <summary>
    /// Service for handling operations for a PO (translation) file
    /// </summary>
    public interface IPOFileService
    {
        /// <summary>
        /// Creates a PO (translation) file from a set of translation keys for a specific encoding and language
        /// Returns true if created succesfully and false otherwise
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextTranslations">Translations dictionary</param>
        /// <param name="language">Language used for the translations</param>
        /// <param name="outputFileName">Output file name</param>
        /// <returns>Returns true if created succesfully and false otherwise</returns>
        bool Create(Encoding fileEncoding, IOrderedDictionary<string, string> htmlTextTranslations, string language, string outputFileName);

        /// <summary>
        /// Parses a PO (translation file) in a given path
        /// and returns a dictionary containing the translations in file
        /// </summary>
        /// <param name="fullFilePath">File Path</param>
        /// <returns>Returns a dictionary containing the translations in file</returns>
        IOrderedDictionary<string, string> Parse(string fullFilePath);
    }
}
