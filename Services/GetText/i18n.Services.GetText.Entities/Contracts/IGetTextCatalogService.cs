using Karambolo.Common.Collections;
using Karambolo.PO;
using System.Collections.Generic;
using System.Text;

namespace i18n.Services.GetText.Entities.Contracts
{
    /// <summary>
    /// Service for handling operations for a the GetText Catalog
    /// </summary>
    public interface IGetTextCatalogService
    {
        /// <summary>
        /// Creates a catalog for a translations dictionary for a given encoding and language  
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextTranslations">Translations dictionary</param>
        /// <param name="translationsLanguage">Translations language</param>
        /// <returns></returns>
        POCatalog CreateCatalog(Encoding fileEncoding, IOrderedDictionary<string, string> htmlTextTranslations, string translationsLanguage);

        /// <summary>
        /// Creates a catalog for a translations dictionary for a given encoding and language  
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextKeys">A list of translations</param>
        /// <param name="translationsLanguage">Translations language</param>
        /// <returns></returns>
        POCatalog CreateCatalog(Encoding fileEncoding, IList<string> htmlTextKeys, string translationsLanguage);
    }
}
