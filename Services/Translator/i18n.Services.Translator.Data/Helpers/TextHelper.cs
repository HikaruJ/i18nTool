using i18n.Services.Translator.Entities.Contracts.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truncon.Collections;

namespace i18n.Services.Translator.Data.Helpers
{
    /// <summary>
    /// Provides methods for manipulating text
    /// </summary>
    public class TextHelper : ITextHelper
    {
        #region Private Readonly Members 

        private const int DEFAULT_SENTENCE_LIMIT = 300;

        // Based on values from https://docs.microsoft.com/en-us/azure/cognitive-services/translator/request-limits
        // Currently not being used, but was setup as an optional approach for breaking text into smaller sentences
        private readonly OrderedDictionary<string, int> _languageSentenceLimitDict = new OrderedDictionary<string, int>()
        {
            { "de", 290 }, { "es", 280 }, { "it", 280 }, { "jp", 150 }, { "pt", 290 }, { "th", 258 }, { "zh", 132 }
        };

        #endregion

        #region Public Methods

        /// <summary>
        /// Break down a text into multiple sentences according to the language
        /// </summary>
        /// <param name="characterLimit">Characters limit per sentence</param>
        /// <param name="text">Selected text for breaking down to sentences</param>
        /// <returns>Returns an Hashset of sentences</returns>
        public IList<string> BreakText(int characterLimit, string text)
        {
            var sentences = new HashSet<string>();
            BreakSentence(characterLimit, 0, sentences, text);
            return sentences.ToList();
        }

        /// <summary>
        /// Iterate over an array of texts and merge
        /// </summary>
        /// <param name="characterLimit">Characters limit per text</param>
        /// <param name="texts">Array of texts</param>
        /// <returns></returns>
        public IList<string> MergeText(int characterLimit, char mergeKey, IList<string> texts)
        {
            var mergedTexts = new List<string>();
            var mergedText = new StringBuilder();
            for (int textIdx = 0; textIdx < texts.Count; textIdx++)
            {
                var text = texts[textIdx];
                if (text.Length + mergedText.Length >= characterLimit)
                {
                    mergedText.AppendLine(text);
                    var mergedTextStr = mergedText.ToString();
                    var textSplit = BreakText(characterLimit, mergedTextStr);
                    for (int textSplitIdx = 0; textSplitIdx < textSplit.Count; textSplitIdx++)
                    {
                        if (textSplitIdx == 0)
                        {
                            mergedTexts.Add(textSplit[textSplitIdx]);
                            mergedText = new StringBuilder();
                        }
                        else
                            mergedText.AppendLine($"{textSplit[textSplitIdx]}{mergeKey}");
                    }
                }
                else
                    mergedText.AppendLine(textIdx + 1 == texts.Count ? text : $"{text}{mergeKey}");
            }

            mergedTexts.Add(mergedText.ToString());
            return mergedTexts;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Recursive method for breaking down a text into smaller sentences,
        //  according to the limit specificed per sentence.
        /// </summary>
        /// <param name="characterLimit">Characters limit per sentence</param>
        /// <param name="startIndex">Start index for the creating the new sentence</param>
        /// <param name="sentences">Hashset containing the list of the separated sentences</param>
        /// <param name="text">The text being broken down to sentences</param>
        private void BreakSentence(int characterLimit, int startIndex, HashSet<string> sentences, string text)
        {
            // Stop the recursive loop when the start index is bigger then the text length (out of boundries)
            if (startIndex < text.Length)
            {
                var endIndex = startIndex + characterLimit;
                if (endIndex > text.Length)
                {
                    endIndex = text.Length;
                    var sentence = text?.Substring(startIndex, text.Length - startIndex);
                    sentences.Add(sentence);
                }
                else
                {
                    for (int index = endIndex - 1; index > startIndex; index--)
                    {
                        // Search for a dot that ends a sentence
                        if (text[index] == '.')
                        {
                            endIndex = index + 1;
                            break;
                        }
                        // If no dot can be found, look for a space
                        else if (text[index] == ' ')
                        {
                            endIndex = index + 1;
                            break;
                        }
                    }

                    var sentence = text?.Substring(startIndex, endIndex - startIndex);
                    sentences.Add(sentence);
                }

                BreakSentence(endIndex, characterLimit, sentences, text);
            }
        }

        #endregion
    }
}
