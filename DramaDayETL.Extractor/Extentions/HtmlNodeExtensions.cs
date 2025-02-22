using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Extentions
{
    public static class HtmlNodeExtensions
    {
        public static bool IsTableRow(this HtmlNode node)
        {
            return node.NodeType == HtmlNodeType.Element &&
                   node.Name.Equals("tr", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsHeader(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var cells = tr.SelectNodes(".//td");

            if (cells == null)
                return false;

            return cells[0].InnerText.Contains("ep", StringComparison.OrdinalIgnoreCase) ||
                   cells[1].InnerText.Contains("quality", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsEmptyRow(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var cells = tr.SelectNodes(".//td");

            return cells != null && cells.All(cell => string.IsNullOrEmpty(cell.InnerText));
        }

        public static bool IsPasswordRow(this HtmlNode tr)
        {
            if (!tr.IsTableRow())
                return false;

            var cells = tr.SelectNodes(".//td");

            if (cells == null)
                return false;

            string passwordText = "Password: dramaday.net";

            foreach (var cell in cells)
            {
                if (cell.InnerText.Contains(passwordText, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAdditionalInfoRow(this HtmlNode tr, out string key)
        {
            key = string.Empty;
            if (!tr.IsTableRow())
                return false;

            var cells = tr.SelectNodes(".//td");

            if (cells == null)
                return false;

            foreach (var cell in cells)
            {
                if (!string.IsNullOrEmpty(cell.GetAttributeValue("data-colspan", string.Empty)))
                {
                    key = cell.InnerText.Trim();
                    return true;
                }
            }

            return false;
        }
    }
}
