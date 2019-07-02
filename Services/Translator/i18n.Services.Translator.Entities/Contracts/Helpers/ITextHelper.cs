using System;
using System.Collections.Generic;

namespace i18n.Services.Translator.Entities.Contracts.Helpers
{
    /// <summary>
    /// Provides methods for manipulating text
    /// </summary>
    public interface ITextHelper
    {
        /// <summary>
        /// Break down a text into multiple sentences according to the language
        /// </summary>
        /// <param name="characterLimit">Language of the text</param>
        /// <param name="text">Selected text for breaking down to sentences</param>
        /// <returns>Returns an Hashset of sentences</returns>
        IList<string> BreakText(int characterLimit, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterLimit"></param>
        /// <param name="mergeKey"></param>
        /// <param name="textDict"></param>
        /// <returns></returns>
        IList<string> MergeText(int characterLimit, char mergeKey, IList<string> texts);
    }
}
