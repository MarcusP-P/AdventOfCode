using System.Text.RegularExpressions;

internal static partial class Program
{
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"(?<instruction>(mul\((?<firstMult>\d{1,3}),(?<secondMult>\d{1,3})\)|do\(\)|don't\(\)))")]
    private static partial Regex lineRegex();


    private static async Task Main(string[] args)
    {
        var accumulator=0;

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        var enabled=true;

        await foreach (var line in input)
        {
            if (!lineRegex().IsMatch(line))
            {
                Console.WriteLine($"Line: \"{line}\".");
                continue;
            }
            else
            {
                var matches = lineRegex().Matches(line);
                foreach (Match match in matches)
                {
                    switch (match.Groups["instruction"].Value) {
                        case "do()":
                            enabled=true;
                            break;
                        case "don't()":
                            enabled=false;
                            break;
                        default:
                            accumulator += (enabled?1:0) * int.Parse(match.Groups["firstMult"].Value) * int.Parse(match.Groups["secondMult"].Value);
                            break;
                    }

                }
            }
        }
        Console.WriteLine($"Sum of multiplications: {accumulator}");
    }
}