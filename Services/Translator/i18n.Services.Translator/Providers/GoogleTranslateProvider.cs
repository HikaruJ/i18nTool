using i18n.Services.Translator.Entities.Contracts.Mock;
using i18n.Services.Translator.Entities.Contracts.Providers;
using i18n.Services.Translator.Entities.Models.GoogleTranslate;
using Karambolo.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace i18n.Services.Translator.Providers
{
    /// <summary>
    /// Provider that defines translation methods that act as a wrapper on top of Google Translate
    /// </summary>
    public class GoogleTranslateProvider : IGoogleTranslateProvider
    {
        #region Private Members

        private int _currentRequestCharacterCount = 0;
        private readonly IGoogleTranslateMock _googleTranslateMock = null;
        private readonly int _maxCharactersPer100Seconds = 100000;
        private readonly int _maxCharactersPerRequest = 30000;
        private IList<string> _request = new List<string>();
        private IList<TranslateTextResponseList> _translateResponses = new List<TranslateTextResponseList>();
        private int _totalRequestsCharacterCount = 0;

        #endregion

        #region CTOR

        public GoogleTranslateProvider(IGoogleTranslateMock googleTranslateMock)
        {
            _googleTranslateMock = googleTranslateMock;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// A wrapper method for Google Translate that translates multiple texts from source language to target language
        /// </summary>
        /// <param name="sourceLanguage">Source language of the texts for translation</param>
        /// <param name="targetLanguage">Target language for translation</param>
        /// <param name="textDict">A dictionary containing texts for translation</param>
        /// <returns>Returns a dictionary of the translated texts
        /// The dictionary keys are the keys from textDict, to help in tracking each translation to its original text</returns>
        public IOrderedDictionary<string, string> TranslateTexts(string sourceLanguage, string targetLanguage, IOrderedDictionary<string, string> textDict)
        {
            TranslateTextResponseList response;
            foreach (var textDictItem in textDict)
            {
                var key = textDictItem.Key;
                var text = textDictItem.Value;
                var isWithinRequestLimit = text.Length + _currentRequestCharacterCount <= _maxCharactersPerRequest ? true : false;
                if (isWithinRequestLimit)
                {
                    _currentRequestCharacterCount += text.Length;
                    _request.Add(text);
                }
                else
                {
                    if (_currentRequestCharacterCount + _totalRequestsCharacterCount >= _maxCharactersPer100Seconds)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(100));
                        _totalRequestsCharacterCount = 0; // Reset the count of requests
                    }

                    response = _googleTranslateMock.TranslateText(sourceLanguage, targetLanguage, _request);
                    _translateResponses.Add(response);
                    _totalRequestsCharacterCount += _currentRequestCharacterCount;

                    // Initiate a new request 
                    _currentRequestCharacterCount = text.Length;
                    _request.Clear();
                    _request.Add(text);
                }
            }

            // Handle any leftover request
            if (_request.Any())
            {
                response = _googleTranslateMock.TranslateText(sourceLanguage, targetLanguage, _request);
                _translateResponses.Add(response);
            }

            var lastIndex = 0;
            IOrderedDictionary<string, string> translatedTextDict = new OrderedDictionary<string, string>();
            foreach(var translateResponse in _translateResponses)
            {
                var translations = translateResponse.Translations;
                for (int translationIdx = 0; translationIdx < translations.Count; translationIdx++)
                {
                    var translation = translations[translationIdx];
                    if (lastIndex > textDict.Count)
                        return translatedTextDict;

                    var textDictItem = textDict.ElementAt(lastIndex);
                    translatedTextDict.Add(textDictItem.Key, translation.TranslatedText);
                    lastIndex += 1;
                }
            }

            return translatedTextDict;
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
