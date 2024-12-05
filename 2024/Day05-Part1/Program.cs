using System.Globalization;
using System.Text.RegularExpressions;

internal static partial class Program {
    [GeneratedRegex(@"(\d+)\|(\d+)")]
    private static partial Regex pageRules();

    [GeneratedRegex(@"(\d+)")]
    private static partial Regex pageSequence();

#pragma warning disable IDE0060 // Remove unused parameter
    private static async Task Main(string[] args) {
        var pageRuleIndex = new SortedDictionary<int, HashSet<int>>();

        var updates = new List<List<int>>();

        var accumulator = 0;

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        // while the data file is in the roder of rules, then pages, we won't assume that, so get all the
        // data first, then process it.
        await foreach (var line in input) {
            if (pageRules().IsMatch(line)) {
                var match = pageRules().Match(line);
                var firstPage = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var secondPage = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                if (!pageRuleIndex.TryGetValue(firstPage, out var followingPages)) {
                    followingPages = [];
                    pageRuleIndex.Add(firstPage, followingPages);
                }

                _ = followingPages.Add(secondPage);
            }
            else if (pageSequence().IsMatch(line)) {
                var matches = pageSequence().Matches(line);

                var updateSequence = new List<int>();

                foreach (Match match in matches) {
                    updateSequence.Add(int.Parse(match.Value, CultureInfo.InvariantCulture));
                }

                updates.Add(updateSequence);
            }
        }

        foreach (var update in updates) {
            var validUpdate = true;
            for (var pageIndex = 0; pageIndex < update.Count - 1; pageIndex++) {
                if (!pageRuleIndex.TryGetValue(update[pageIndex], out var pageRule)) {
                    validUpdate = false;
                    break;
                }

                for (var checkPageIndex = pageIndex + 1; checkPageIndex < update.Count; checkPageIndex++) {
                    if (!pageRule.Contains(update[checkPageIndex])) {
                        validUpdate = false;
                        break;
                    }
                }

                if (!validUpdate) {
                    break;
                }
            }

            if (validUpdate) {
                var midPage = update.Count / 2;
                accumulator += update[midPage];
            }
        }

        Console.WriteLine($"Sum of middle pages of valid results: {accumulator}");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
