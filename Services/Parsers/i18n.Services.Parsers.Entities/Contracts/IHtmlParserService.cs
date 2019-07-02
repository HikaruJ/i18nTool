using i18n.Services.Parsers.Entities.Models;
using Karambolo.Common.Collections;
using System.Collections.Generic;

namespace i18n.Services.Parsers.Entities.Contracts
{
    /// <summary>
    /// Provides methods for parsing and working with HTML
    /// </summary>
    public interface IHtmlParserService
    {
        /// <summary>
        /// Scans the HTML from a given path and replaces each text value with a unique value for future translation.
        /// The approach taken for unique Id's is based on the GetText approach, where each translation key is the 
        /// existing phrase in the text. Giving the translator the ability to better understand and translate the text.
        /// 
        /// The method will extract each text value from the HTML, add it to a OrderedDictionary and keep track of the number 
        /// of occurences of the same text. In case a text a text appears more then once, the value will be updated with
        /// the number of occurences, otherwise the value will be kept untouched.
        /// </summary>
        /// <param name="path">HTML file path</param>
        /// <returns>Returns a result containing a list of the unique ids created and the updated HTML</returns>
        UniqueIdsResult GenerateUniqueIds(string path);

        /// <summary>
        /// Parse the HTML and extract the text to a list
        /// </summary>
        /// <param name="path">HTML file path</param>
        /// <returns>Returns the extracted text from the HTML in a form of a list</returns>
        IList<string> ParseTextFromHTML(string path);

        /// <summary>
        /// Updates an HTML file using a given set of translations (OrderedDictionary)
        /// </summary>
        /// <param name="path">HTML file path</param>
        /// <param name="textTranslationsDict">Translations OrderedDictionary (uniqueId, value)</param>
        /// <returns>Returns the updated HTML</returns>
        string UpdateHTML(string path, IOrderedDictionary<string, string> textTranslationsDict);
    }
}
