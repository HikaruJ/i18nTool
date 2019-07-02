using i18n.Services.GetText.Entities.Contracts;
using Karambolo.Common.Collections;
using Karambolo.PO;
using System;
using System.Collections.Generic;
using System.Text;

namespace i18n.Services.GetText
{
    /// <summary>
    /// Service for handling operations for a the GetText Catalog
    /// </summary>
    public class GetTextCatalogService : IGetTextCatalogService
    {
        #region Public Methods 

        /// <summary>
        /// Creates a catalog for a translations dictionary for a given encoding and language  
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextTranslations">Translations dictionary</param>
        /// <param name="translationsLanguage">Translations language</param>
        /// <returns></returns>
        public POCatalog CreateCatalog(Encoding fileEncoding, IOrderedDictionary<string, string> htmlTextTranslations, string translationsLanguage)
        {
            var catalog = new POCatalog
            {
                // Setting Required Headers
                Encoding = fileEncoding.HeaderName,
                Language = translationsLanguage,

                // Setting Custom Headers
                Headers = CreateHeaders()
            };

            foreach (var htmlTextTranslation in htmlTextTranslations)
            {
                var poKey = new POKey(htmlTextTranslation.Key);
                var poEntry = new POSingularEntry(poKey) { Translation = htmlTextTranslation.Value };
                catalog.Add(poEntry);
            }

            return catalog;
        }

        /// <summary>
        /// Creates a catalog for a translations dictionary for a given encoding and language  
        /// </summary>
        /// <param name="fileEncoding">File encoding</param>
        /// <param name="htmlTextKeys">A list of translations</param>
        /// <param name="translationsLanguage">Translations language</param>
        /// <returns></returns>
        public POCatalog CreateCatalog(Encoding fileEncoding, IList<string> htmlTextKeys, string translationsLanguage)
        {
            var catalog = new POCatalog
            {
                // Setting Required Headers
                Encoding = fileEncoding.WebName,
                Language = translationsLanguage,

                // Setting Custom Headers
                Headers = CreateHeaders()
            };

            foreach (var htmlTextKey in htmlTextKeys)
            {
                var poKey = new POKey(htmlTextKey);
                var poEntry = new POSingularEntry(poKey);
                catalog.Add(poEntry);
            }

            return catalog;
        }

        #endregion

        #region Private Methods

        private OrderedDictionary<string, string> CreateHeaders()
        {
            return new OrderedDictionary<string, string>
            {
                { "POT-Creation-Date", DateTime.UtcNow.ToString() }
            };
        }

        #endregion
    }
}
