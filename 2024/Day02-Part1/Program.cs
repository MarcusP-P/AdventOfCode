using System.Globalization;
using System.Text.RegularExpressions;

internal static partial class Program {
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"\d+")]
    private static partial Regex lineRegex();

#pragma warning disable IDE0060 // Remove unused parameter
    private static async Task Main(string[] args) {
        var validReports = 0;

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        await foreach (var line in input) {
            var report = new List<int>();

            if (lineRegex().IsMatch(line)) {
                var matches = lineRegex().Matches(line);
                foreach (Match match in matches) {
                    report.Add(int.Parse(match.Value, CultureInfo.InvariantCulture));
                }
                // create a list of pairs
                var valuePairs = report.Zip(report.Skip(1), (current, next) => new { current, next })
                    .ToList();

                if (valuePairs.All(x => Math.Abs(x.current - x.next) is >= 1 and <= 3)
                    && (valuePairs.All(x => x.current < x.next)
                      || valuePairs.All(x => x.current > x.next))) {
                    Console.WriteLine($"{string.Concat(report.Select(x => $"{x} "))}: Valid");
                    validReports++;
                }
                else {
                    Console.WriteLine($"{string.Concat(report.Select(x => $"{x} "))}: Invalid");
                }
            }
            else {
                Console.WriteLine($"Line: \"{line}\".");
            }
        }

        Console.WriteLine($"Valid reports {validReports}");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
