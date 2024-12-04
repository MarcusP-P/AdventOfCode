using System.Globalization;
using System.Text.RegularExpressions;

internal static partial class Program {
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"mul\((?<firstMult>\d{1,3}),(?<secondMult>\d{1,3})\)")]
    private static partial Regex lineRegex();

#pragma warning disable IDE0060 // Remove unused parameter
    private static async Task Main(string[] args) {
        var accumulator = 0;

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        await foreach (var line in input) {
            var report = new List<int>();

            if (!lineRegex().IsMatch(line)) {
                Console.WriteLine($"Line: \"{line}\".");
                continue;
            }
            else {
                var matches = lineRegex().Matches(line);
                foreach (Match match in matches) {
                    accumulator += int.Parse(match.Groups["firstMult"].Value, CultureInfo.InvariantCulture) * int.Parse(match.Groups["secondMult"].Value, CultureInfo.InvariantCulture);
                }
            }
        }

        Console.WriteLine($"Sum of multiplications: {accumulator}");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
