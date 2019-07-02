using HtmlAgilityPack;
using i18n.Infrastructure.Entities.Contracts.IO;
using i18n.Services.Parsers.Data.Enums;
using i18n.Services.Parsers.Data.Extensions;
using i18n.Services.Parsers.Entities.Contracts;
using i18n.Services.Parsers.Entities.Models;
using Karambolo.Common.Collections;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace i18n.Services.Parsers
{
    /// <summary>
    /// Provides methods for parsing and working with HTML
    /// </summary>
    public class HtmlParserService : IHtmlParserService
    {
        #region Private Members

        private readonly IFileWrapper _fileWrapper = null;
        private ILogger<HtmlParserService> _logger = null;

        #endregion

        #region CTOR

        public HtmlParserService(IFileWrapper fileWrapper, ILogger<HtmlParserService> logger)
        {
            _fileWrapper = fileWrapper;
            _logger = logger;
        }

        #endregion

        #region Public Methods

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
        public UniqueIdsResult GenerateUniqueIds(string path)
        {
            Console.WriteLine("Parsing HTML and generating unique Ids..");

            var htmlOriginalValuesDict = new OrderedDictionary<string, string>();
            var htmlTextDict = new OrderedDictionary<string, int>();

            var innerHTML = _fileWrapper.Read(path);
            if (string.IsNullOrEmpty(innerHTML))
                return null;

            var root = GetDocumentNodeFromSource(innerHTML);
            if (root == null)
                return null;

            // Iterate over a reduced version of the HTML that does not contain scripts and styles
            foreach (var node in root.CleanDocument().DescendantsAndSelf())
            {
                if (!node.HasChildNodes)
                {
                    // Find ALT tags 
                    var altAttribute = node.Attributes[HtmlAttributes.ALT];
                    if (!string.IsNullOrEmpty(altAttribute?.Value))
                        AddUniqueIdToDictionary(htmlOriginalValuesDict, htmlTextDict, altAttribute?.Value);

                    // Find Title tags
                    var titleAttribute = node.Attributes[HtmlAttributes.TITLE];
                    if (!string.IsNullOrEmpty(titleAttribute?.Value))
                        AddUniqueIdToDictionary(htmlOriginalValuesDict, htmlTextDict, titleAttribute?.Value);

                    if (IsValidTextNode(node))
                    {
                        string nodeInnerText = node.InnerText?.Trim();
                        AddUniqueIdToDictionary(htmlOriginalValuesDict, htmlTextDict, nodeInnerText);
                    }
                }
            }

            // Update the original HTML that contains scripts and styles with the updated keys
            var htmlDocumentBuilder = new StringBuilder();
            int lastIndex = 0;
            foreach(var htmlOriginalValueItem in htmlOriginalValuesDict)
            {
                var key = $"[[{htmlOriginalValueItem.Key}]]";
                var text = htmlOriginalValueItem.Value;
                var position = innerHTML.IndexOf(text, lastIndex);
                var substringLength = position - lastIndex;
                var htmlToPosition = innerHTML.Substring(lastIndex, substringLength);
                htmlDocumentBuilder.Append(htmlToPosition);
                htmlDocumentBuilder.Append(key);
                lastIndex = text.Length + position;
            }

            var reaminingHTMLLength = innerHTML.Length - lastIndex;
            var reaminingHTML = innerHTML.Substring(lastIndex, reaminingHTMLLength);
            htmlDocumentBuilder.Append(reaminingHTML);

            return new UniqueIdsResult()
            {
                HTMLOriginalValues = htmlOriginalValuesDict,
                HTMLTextKeys = htmlTextDict.Keys.ToList(),
                UpdatedHTML = htmlDocumentBuilder.ToString()
            };
        }

        /// <summary>
        /// Parse the HTML and extract the text to a list
        /// </summary>
        /// <param name="path">HTML file path</param>
        /// <returns>Returns the extracted text from the HTML in a form of a list</returns>
        public IList<string> ParseTextFromHTML(string path)
        {
            var htmlTextKeys = new List<string>();
            Console.WriteLine("Parsing text from HTML..");

            var innerHTML = _fileWrapper.Read(path);
            if (string.IsNullOrEmpty(innerHTML))
                return null;

            var root = GetDocumentNodeFromSource(innerHTML);
            if (root == null)
                return null;

            foreach (var node in root.CleanDocument().DescendantsAndSelf())
            {
                if (!node.HasChildNodes)
                {
                    // Find ALT tags 
                    var altAttribute = node.Attributes[HtmlAttributes.ALT];
                    if (!string.IsNullOrEmpty(altAttribute?.Value))
                        htmlTextKeys.Add(altAttribute?.Value);

                    // Find Title tags
                    var titleAttribute = node.Attributes[HtmlAttributes.TITLE];
                    if (!string.IsNullOrEmpty(titleAttribute?.Value))
                        htmlTextKeys.Add(titleAttribute?.Value);

                    if (IsValidTextNode(node))
                        htmlTextKeys.Add(node.InnerText?.Trim());
                }
            }

            return htmlTextKeys;
        }

        /// <summary>
        /// Updates an HTML file using a given set of translations (OrderedDictionary)
        /// </summary>
        /// <param name="path">HTML file path</param>
        /// <param name="textTranslationsDict">Translations OrderedDictionary (uniqueId, value)</param>
        /// <returns>Returns the updated HTML</returns>
        public string UpdateHTML(string path, IOrderedDictionary<string, string> textTranslationsDict)
        {
            Console.WriteLine("Updating the generated HTML text..");

            var innerHTML = _fileWrapper.Read(path);
            if (string.IsNullOrEmpty(innerHTML))
                return null;

            // Update the original HTML that contains scripts and styles with the updated keys
            var htmlDocumentBuilder = new StringBuilder();
            int lastIndex = 0;
            foreach (var textTranslationItem in textTranslationsDict)
            {
                var key = $"[[{textTranslationItem.Key}]]";
                var text = textTranslationItem.Value;
                var position = innerHTML.IndexOf(key, lastIndex);
                var substringLength = position - lastIndex;
                var htmlToPosition = innerHTML.Substring(lastIndex, substringLength);
                htmlDocumentBuilder.Append(htmlToPosition);
                htmlDocumentBuilder.Append(text);
                lastIndex = key.Length + position;
            }

            var reaminingHTMLLength = innerHTML.Length - lastIndex;
            var reaminingHTML = innerHTML.Substring(lastIndex, reaminingHTMLLength);
            htmlDocumentBuilder.Append(reaminingHTML);

            return htmlDocumentBuilder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create a unique id for each text value in the HTML being parsed and save in a dictionary.
        /// This method is designed to collect identified text in an HTML
        /// and track the number of occurences on the page.
        /// This is done to prevent a case where we will have the same text appearing multiple times,
        /// where however, we would like to have a different interpretation per appeareance. 
        /// </summary>
        /// <param name="htmlOriginalValuesDict"></param>
        /// <param name="htmlTextDict"></param>
        /// <param name="text">The text used for generating the unique Id</param>
        /// <returns>Returns eithe the text as is or with the number of encounters that have been detected previously on the page</returns>
        private void AddUniqueIdToDictionary(IOrderedDictionary<string, string> htmlOriginalValuesDict, IOrderedDictionary<string, int> htmlTextDict, string text)
        {
            if (!htmlTextDict.ContainsKey(text))
            {
                htmlOriginalValuesDict.Add(text, text); // Save the text to a OrderedDictionary that keeps the original values
                htmlTextDict.Add(text, 1); // Default to one enocunter
                return;
            }

            htmlTextDict[text] = htmlTextDict[text] + 1; // Increase encounters
            var textEncountersCount = htmlTextDict[text];

            htmlOriginalValuesDict.Add($"{text}{textEncountersCount}", text); // Save the text to a OrderedDictionary that keeps the original values
            htmlTextDict.Add($"{text}{textEncountersCount}", 1); // Save new encounter to the OrderedDictionary with a default of 1
        }

        /// <summary>
        /// Loads an HTML document from a given source and returns the root document node
        /// </summary>
        /// <param name="htmlSource">HTML source</param>
        /// <returns>Returns the root document node</returns>
        private HtmlNode GetDocumentNodeFromSource(string htmlSource)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlSource);
                var root = htmlDocument.DocumentNode;

                return root;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load HTML from provided source");
                Console.WriteLine($"Failed to load HTML from provided source. Check log for additional information");
                return null;
            }
        }

        /// <summary>
        /// Validate if a given HTML node is a node that contains text that can be used for translation
        /// </summary>
        /// <param name="node">HTML node</param>
        /// <returns>Returns true if the node is valid for use in translation and false otherwise</returns>
        private bool IsValidTextNode(HtmlNode node)
        {
            string nodeInnerText = node.InnerText?.Trim();

            // Filter numbers only text
            var isNumeric = int.TryParse(nodeInnerText, out int n);
            if (isNumeric)
                return false;

            // Filter URL
            Uri uriResult;
            bool isURL = Uri.TryCreate(nodeInnerText, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (isURL)
                return false;

            // Filter empty spaces or empty text
            if (string.IsNullOrEmpty(nodeInnerText) || nodeInnerText == "\r\n" || nodeInnerText == "&lt;")
                return false;

            // Filter text which is only one character
            if (nodeInnerText.Length == 1)
                return false;

            // Filter XML
            if (nodeInnerText.StartsWith("<?xml"))
                return false;

            return true;
        }

        /// <summary>
        /// Updates the attribute's text with a matching translation
        /// If no translation can be found, the attribute's text will not be translated
        /// </summary>
        /// <param name="attribute">HTML attribute</param>
        /// <param name="textTranslationsDict">Translations OrderedDictionary</param>
        private void UpdateAttributeWithTranslation(HtmlAttribute attribute, IOrderedDictionary<string, string> textTranslationsDict)
        {
            var key = attribute?.Value;
            if (string.IsNullOrEmpty(key))
                return;

            if (textTranslationsDict.ContainsKey(key))
                attribute.Value = textTranslationsDict[key];
        }

        /// <summary>
        /// Updates the node's text by replacing the existing node with a new node that contains the matching translation
        /// If no translation can be found, the node will be kept as is and will not be translated
        /// </summary>
        /// <param name="node">HTML node</param>
        /// <param name="textTranslationsDict">Translations OrderedDictionary</param>
        private void UpdateNodeWithTranslation(HtmlNode node, IOrderedDictionary<string, string> textTranslationsDict)
        {
            var key = node.InnerText;
            if (string.IsNullOrEmpty(key))
                return;

            if (textTranslationsDict.ContainsKey(key))
                node.ParentNode.ReplaceChild(HtmlNode.CreateNode(textTranslationsDict[key]), node);
        }

        #endregion
    }
}
