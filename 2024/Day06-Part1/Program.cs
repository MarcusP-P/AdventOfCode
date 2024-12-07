using System.Text.RegularExpressions;

using Location = (int row, int col);

internal enum Direction {
    Up = 0,
    Right,
    Down,
    Left,
}

internal static partial class Program {
    [GeneratedRegex(@"(\^+)")]
    private static partial Regex StartField();

#pragma warning disable IDE0060 // Remove unused parameter
    private static async Task Main(string[] args) {
        var mapRows = new List<char[]>();

        var path = new HashSet<Location> { };

        var direction = Direction.Up;

        var location = new Location(-1, -1);

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        // while the data file is in the roder of rules, then pages, we won't assume that, so get all the
        // data first, then process it.
        await foreach (var line in input) {
            if (line.Length > 0) {
                mapRows.Add(line.Trim().ToCharArray());
            }

            if (StartField().IsMatch(line)) {
                var match = StartField().Match(line);
                location = (mapRows.Count - 1, match.Index);
            }
        }

        if (location == (-1, -1)) {
            throw new InvalidDataException("No starting location");
        }

        while (true) {
            _ = path.Add(location);
            var target = direction! switch {
                Direction.Up => new Location(location.row - 1, location.col),
                Direction.Down => new Location(location.row + 1, location.col),
                Direction.Right => new Location(location.row, location.col + 1),
                Direction.Left => new Location(location.row, location.col - 1),
                _ => throw new InvalidDataException("Invalid direction"),
            };
            if (target.row < 0 || target.col < 0 || target.row >= mapRows.Count || target.col >= mapRows[0].Length) {
                break;
            }

            if (mapRows[target.row][target.col] == '#') {
                direction = direction == Direction.Left ? Direction.Up : direction + 1;
            }
            else {
                location = target;
            }
        }

        Console.WriteLine($"Steps taken: {path.Count}");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
