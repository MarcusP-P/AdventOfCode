using System.Globalization;
using System.Text.RegularExpressions;

internal static partial class Program {
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"(?<result>\d+):( (?<operand>\d+))+")]
    private static partial Regex lineRegex();

#pragma warning disable IDE0060 // Remove unused parameter
    private static async Task Main(string[] args) {

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        var accumulator = 0L;

        await foreach (var line in input) {
            if (lineRegex().IsMatch(line)) {
                var match = lineRegex().Match(line);

                var result = long.Parse(match.Groups["result"].Value, CultureInfo.InvariantCulture);
                var operands = match
                    .Groups["operand"]
                    .Captures
                    .Select(x => long.Parse(x.Value, CultureInfo.InvariantCulture))
                    .ToArray();

                // We only have two operations, so we jsut use a bit field 0=plus, 1=multiply
                // use -2 rather than -1 becasue we 0 index, and then subtract a second one because we have 1 less
                // operation than operands
                for (var operations = 0L; operations <= (1 << (operands.Length - 1)) - 1; operations++) {
                    var partialResult = operands[0];
                    for (var pos = 1; pos <= operands.Length - 1; pos++) {
                        if ((operations & (1 << (pos - 1))) == 0) {
                            partialResult += operands[pos];
                        }
                        else {
                            partialResult *= operands[pos];
                        }
                    }

                    if (partialResult == result) {
                        accumulator += partialResult;
                        break;
                    }
                }
            }

            else {
                Console.WriteLine($"Line: \"{line}\".");
            }
        }

        Console.WriteLine($"Result {accumulator}");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
