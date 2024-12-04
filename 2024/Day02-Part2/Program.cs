using System.Text.RegularExpressions;

internal static partial class Program
{
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"\d+")]
    private static partial Regex lineRegex();


    private static async Task Main(string[] args)
    {
        // gaps that are consisdered safe
        var validDistance = Enumerable.Range(1, 3);

        var validReports = 0;

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        await foreach (var line in input)
        {
            var reportCombinations = new List<List<int>>();
            var report = new List<int>();

            if (!lineRegex().IsMatch(line))
            {
                Console.WriteLine($"Invalid line: \"{line}\".");
                continue;
            }

            var matches = lineRegex().Matches(line);
            foreach (Match match in matches)
            {
                report.Add(int.Parse(match.Value));
            }

            // Because there may be one value in the list that is wrong, we cheat.
            // We will check the full list, but also create all combinations with an 
            // element removed, and check that list. if we have a success, then we have
            // a safe with a problem dampener
            reportCombinations.Add(report);

            for (int i = 0; i < report.Count; i++)
            {
                reportCombinations.Add(report.ToList());
                reportCombinations[reportCombinations.Count-1].RemoveAt(i);
            }

            bool safeReport=false;
            bool problemDampener=false;
            var safeReportValues=report;

            foreach (var currentReport in reportCombinations)
            {
                // create a list of pairs
                var valuePairs = currentReport.Zip(currentReport.Skip(1), (current, next) => new { current, next });

                if (valuePairs.All(x => validDistance.Contains(Math.Abs(x.current - x.next)))
                    && (valuePairs.All(x => x.current < x.next)
                    || valuePairs.All(x => x.current > x.next)))
                {
                    safeReport=true;
                    safeReportValues=currentReport;
                    break;
                }
                problemDampener=true;
            }

            if (safeReport) 
            {
                    Console.WriteLine($"{String.Concat(report.Select(x => $"{x} "))}: Safe{(problemDampener?$" with Problem Dampener {String.Concat(safeReportValues.Select(x => $"{x} "))}":"")}");
                validReports++;
            }

            else
            {
                Console.WriteLine($"{String.Concat(report.Select(x => $"{x} "))}: Unsafe");
            }
        }
        Console.WriteLine($"Valid reports {validReports}");
    }
}