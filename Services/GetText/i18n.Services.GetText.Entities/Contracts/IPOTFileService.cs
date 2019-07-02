using System.Collections.Generic;
using System.Text;

namespace i18n.Services.GetText.Entities.Contracts
{
    /// <summary>
    /// Service for handling operations for a POT (translation) file
    /// </summary>
    public interface IPOTFileService
    {
        /// <summary>
        /// Creates a POT (translation) file from a list of translations for a specific encoding and language
        /// Returns true if created succesfully and false otherwise
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextKeys">A list of translations</param>
        /// <param name="language">Language used for the translations</param>
        /// <param name="outputFileName">Output file name</param>
        /// <returns>Returns true if created succesfully and false otherwise</returns>
        bool Create(Encoding fileEncoding, IList<string> htmlTextKeys, string language, string outputFileName);
    }
}
