using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace AircraftTypeDesignatorScraper;

class Program
{
    static void Main(string[] args)
    {
        var wikiUrl = "https://en.wikipedia.org/wiki/List_of_aircraft_type_designators";
        var baseWikiUrl = "https://en.wikipedia.org";
        var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "aircraft_type_designators.csv");

        Console.WriteLine($"Fetching: {wikiUrl}");

        var web = new HtmlWeb();
        HtmlDocument doc;
        try
        {
            doc = web.Load(wikiUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to load the page: {ex.Message}");
            return;
        }

        var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'wikitable')]");
        if (table == null)
        {
            Console.WriteLine("❌ Table not found.");
            return;
        }

        var rows = table.SelectNodes(".//tr");
        if (rows == null || rows.Count == 0)
        {
            Console.WriteLine("❌ No rows found.");
            return;
        }

        var sb = new StringBuilder();

        // ✅ New 4-column header
        sb.AppendLine("\"ICAO Code\",\"IATA Type Code\",\"Model\",\"Wikipedia Link\"");

        // Process each row, skip header
        foreach (var row in rows.Skip(1))
        {
            var cells = row.SelectNodes("td");
            if (cells == null || cells.Count < 3) continue;

            var icao = Clean(cells[0].InnerText);
            var iata = Clean(cells[1].InnerText);

            var modelNode = cells[2];
            var modelText = Clean(modelNode.InnerText);

            // ✅ Extract href from model cell
            var linkNode = modelNode.SelectSingleNode(".//a[@href]");
            var href = linkNode?.GetAttributeValue("href", null);
            var fullLink = href != null ? baseWikiUrl + href : "";

            var line = string.Join(",", Quote(icao), Quote(iata), Quote(modelText), Quote(fullLink));
            sb.AppendLine(line);
        }

        File.WriteAllText(csvPath, sb.ToString(), Encoding.UTF8);
        Console.WriteLine($"✅ Done! Saved to: {csvPath}");
    }

    static string Clean(string text)
    {
        text = text.Trim();
        text = Regex.Replace(text, "&#91;.*?&#93;", "");
        text = Regex.Replace(text, @"\[\d+\]", "");
        if (text == "—" || text == "-")
            text = "";
        return text;
    }

    static string Quote(string input)
    {
        return "\"" + input.Replace("\"", "\"\"") + "\"";
    }
}
