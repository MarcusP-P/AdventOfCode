using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

internal static partial class Program
{
    // look for a string of digits, followed by a equence of spaces, and another string of digits
    [GeneratedRegex(@"^(\d+) +(\d+)$")]
    private static partial Regex lineRegex();


    private static async Task Main(string[] args)
    {
        var list1=new List<int>();
        var list2=new List<int>();

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        // Becasue we sort the lists
        await foreach (var line in input)
        {
            if (lineRegex().IsMatch(line))
            {
                var matches = lineRegex().Match(line);
                list1.Add(int.Parse(matches.Groups[1].Value));
                list2.Add(int.Parse(matches.Groups[2].Value));
            }
            else
            {
                Console.WriteLine($"Line: \"{line}\".");
            }
        }
        list1.Sort();
        list2.Sort();

        int accumulator=0;

        while (list1.Any()) {
            accumulator+=Math.Abs( list1.First()-list2.First());
            list1.RemoveAt(0);
            list2.RemoveAt(0);
        }
        Console.WriteLine($"The difference is {accumulator}");
    }
}