using HtmlAgilityPack;

namespace App.Html;

// ref: https://chatgpt.com/c/66feb1b8-dd54-8000-98cd-31541b07d833

public static class HtmlToTextConverter
{
    // Configurable list of tags to ignore when adding double carriage returns
    private static readonly HashSet<string> DefaultTagsToIgnore = new HashSet<string> { "span", "strong" };

    public static string ConvertHtmlToPlainText(string htmlContent, HashSet<string>? tagsToIgnore = null)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return string.Empty;

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // Use the provided list of tags to ignore, or default if none are provided
        tagsToIgnore ??= DefaultTagsToIgnore;

        // Process nodes to handle links, line breaks, and plain text
        return ConvertNodeToPlainText(doc.DocumentNode, tagsToIgnore).Trim();
    }

    private static string ConvertNodeToPlainText(HtmlNode node, HashSet<string> tagsToIgnore)
    {
        if (node == null)
            return string.Empty;

        if (node.NodeType == HtmlNodeType.Text)
        {
            // Return text nodes directly, unescaped
            return HtmlEntity.DeEntitize(node.InnerText);
        }
        else if (node.Name == "br")
        {
            // Replace <br/> with a single carriage return
            return "\n";
        }
        else if (node.Name == "a" && node.Attributes["href"] != null)
        {
            // Handle anchor tags (links) to include their href in the text
            string linkText = HtmlEntity.DeEntitize(node.InnerText);
            string href = node.Attributes["href"].Value;
            return $"{linkText}: {href}";
        }

        // Recursively process child nodes
        string result = string.Concat(node.ChildNodes.Select(child => ConvertNodeToPlainText(child, tagsToIgnore)));

        // If the tag is not in the ignore list, append a double carriage return
        if (!tagsToIgnore.Contains(node.Name.ToLower()))
        {
            result += "\n\n"; // Double carriage return
        }

        return result;
    }
}
