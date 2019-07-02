using Karambolo.Common.Collections;

namespace i18n.Services.Translator.Entities.Contracts.Providers
{
    /// <summary>
    /// Interface for defining translation methods that can be implemented by different providers
    /// </summary>
    public interface ITranslatorProvider
    {
        /// <summary>
        /// A wrapper method for a translation provider that translates multiple texts from source language to target language
        /// </summary>
        /// <param name="sourceLanguage">Source language of the texts for translation</param>
        /// <param name="targetLanguage">Target language for translation</param>
        /// <param name="textDict">A dictionary containing texts for translation</param>
        /// <returns>Returns a dictionary of the translated texts
        /// The dictionary keys are the keys from textDict, to help in tracking each translation to its original text</returns>
        IOrderedDictionary<string, string> TranslateTexts(string sourceLanguage, string targetLanguage, IOrderedDictionary<string, string> textDict);
    }
}
