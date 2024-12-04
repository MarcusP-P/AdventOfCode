using System.Text.RegularExpressions;

internal static partial class Program
{
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"\d+")]
    private static partial Regex lineRegex();


    private static async Task Main(string[] args)
    {
        var validDistance = Enumerable.Range(1, 3);

        var validReports = 0;

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        await foreach (var line in input)
        {
            var report = new List<int>();

            if (lineRegex().IsMatch(line))
            {
                var matches = lineRegex().Matches(line);
                foreach (Match match in matches)
                {
                    report.Add(int.Parse(match.Value));
                }
                // create a list of pairs
                var valuePairs = report.Zip(report.Skip(1), (current, next) => new { current, next });

                if (valuePairs.All(x => validDistance.Contains(Math.Abs(x.current - x.next)))
                    && (valuePairs.All(x => x.current < x.next)
                      || valuePairs.All(x => x.current > x.next)))
                {
                    Console.WriteLine($"{String.Concat(report.Select(x=>$"{x} "))}: Valid");
                    validReports++;
                }
                else
                {
                    Console.WriteLine($"{String.Concat(report.Select(x=>$"{x} "))}: Invalid");
                }
            }
            else
            {
                Console.WriteLine($"Line: \"{line}\".");
            }
        }
        Console.WriteLine($"Valid reports {validReports}");
    }
}