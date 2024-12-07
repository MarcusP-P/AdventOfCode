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

                // We have three operations, so we jsut use base 3 fields 0=plus, 1=multiply, 2=concat
                for (var operations = 0L; operations <= ((long)Math.Pow(3, operands.Length - 1)); operations++) {
                    var partialResult = operands[0];
                    var remainingOperations = operations;
                    for (var pos = 1; pos <= operands.Length - 1; pos++) {
                        var currentOperation = remainingOperations % 3;
                        remainingOperations = (remainingOperations - currentOperation) / 3;
                        switch (currentOperation) {
                            case 0:
                                partialResult += operands[pos];
                                break;
                            case 1:
                                partialResult *= operands[pos];
                                break;
                            case 2:
                                partialResult = long.Parse(partialResult.ToString(CultureInfo.InvariantCulture) + operands[pos].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                                break;
                            default:
                                throw new InvalidOperationException("We should not get here.");
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
