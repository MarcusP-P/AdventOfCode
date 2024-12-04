using System.Globalization;
using System.Text.RegularExpressions;

internal static partial class Program {
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"^(\d+) +(\d+)$")]
    private static partial Regex lineRegex();

#pragma warning disable IDE0060 // Remove unused parameter
    private static async Task Main(string[] args) {
        var list1 = new List<int>();
        var list2 = new List<int>();

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        // Becasue we sort the lists
        await foreach (var line in input) {
            if (lineRegex().IsMatch(line)) {
                var matches = lineRegex().Match(line);
                list1.Add(int.Parse(matches.Groups[1].Value, CultureInfo.InvariantCulture));
                list2.Add(int.Parse(matches.Groups[2].Value, CultureInfo.InvariantCulture));
            }
            else {
                Console.WriteLine($"Line: \"{line}\".");
            }
        }

        var accumulator = list1
            .Select(x => x * list2.Where(y => y == x).Count()).Sum();
        Console.WriteLine($"The difference is {accumulator}");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
