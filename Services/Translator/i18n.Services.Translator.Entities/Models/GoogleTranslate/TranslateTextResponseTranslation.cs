namespace i18n.Services.Translator.Entities.Models.GoogleTranslate
{
    /// <summary>
    /// Based on Google Translate API response - https://cloud.google.com/translate/docs/reference/rest/v2/translate
    /// </summary>
    public class TranslateTextResponseTranslation
    {
        public string DetectedSourceLanguage { get; set; }
        public string TranslatedText { get; set; }
    }
}
