using Bogus;
using i18n.Services.Translator.Entities.Contracts.Mock;
using i18n.Services.Translator.Entities.Models.GoogleTranslate;
using System;
using System.Collections.Generic;

namespace i18n.Services.Translator.Mock
{
    /// <summary>
    /// A mock service that imitates a Google Translate response
    /// Results are based according to the Google Translate API documentation
    /// </summary>
    public class GoogleTranslateMock : IGoogleTranslateMock
    {
        #region Public Methods 

        /// <summary>
        /// A mock for the "TranslateText" method of Google Translate
        /// The method recevies a source language, target languages and a list of texts
        /// and returns a list of translations for the given texts
        /// The mock uses a lorem ipsum library for faking data
        /// </summary>
        /// <param name="sourceLanguage">Current language used in the HTML file</param>
        /// <param name="targetLanguage">The desired translation language</param>
        /// <param name="texts">A list of texts for translations</param>
        /// <returns>Returns a list of translations for the given texts</returns>
        public TranslateTextResponseList TranslateText(string sourceLanguage, string targetLanguage, IList<string> texts)
        {
            Randomizer.Seed = new Random(texts.Count);
            var lorem = new Bogus.DataSets.Lorem(locale: "ko"); // Using korean, since the library cannot fake Japanese data sets
            var translations = new List<TranslateTextResponseTranslation>();
            for (int idx = 0; idx < texts.Count; idx++)
            {
                translations.Add(new TranslateTextResponseTranslation()
                {
                    DetectedSourceLanguage = targetLanguage,
                    TranslatedText = lorem.Sentence()
                });
            }

            return new TranslateTextResponseList()
            {
                Translations = translations
            };
        }

        #endregion
    }
}
