using System.Collections.Generic;

namespace i18n.Services.Translator.Entities.Models.GoogleTranslate
{
    /// <summary>
    /// Based on Google Translate API response - https://cloud.google.com/translate/docs/reference/rest/v2/translate
    /// </summary>
    public class TranslateTextResponseList
    {
        public IList<TranslateTextResponseTranslation> Translations { get; set; }
    }
}
