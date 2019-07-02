using HtmlAgilityPack;
using System.Linq;

namespace i18n.Services.Parsers.Data.Extensions
{
    /// <summary>
    /// Extensions for the HTML Agility Pack
    /// </summary>
    public static class HtmlAgilityPackExtensions
    {
        /// <summary>
        /// Extension method for HtmlNode that cleans the scripts and styles from the html document
        /// </summary>
        /// <param name="documentNode">The HTML file document node</param>
        /// <returns>Returns the html document node without scripts and styles</returns>
        public static HtmlNode CleanDocument(this HtmlNode documentNode)
        {
            // Remove scripts and styles
            documentNode.Descendants()
                .Where(n => n.Name == "script" || n.Name == "style")
                .ToList()
                .ForEach(n => n.Remove());

            // Remove comments
            documentNode.Descendants()
                 .Where(n => n.NodeType == HtmlNodeType.Comment)
                 .ToList()
                 .ForEach(n => n.Remove());

            return documentNode;
        }
    }
}
