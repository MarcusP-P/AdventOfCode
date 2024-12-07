using System.Text.RegularExpressions;

using Location = (int row, int col);
using History = ((int row, int col) location, Direction direction);

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

        var validBlocks = 0;
        var badBlocks = 0;

        var start = new Location(-1, -1);

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        // while the data file is in the roder of rules, then pages, we won't assume that, so get all the
        // data first, then process it.
        await foreach (var line in input) {
            if (line.Trim().Length > 0) {
                mapRows.Add(line.Trim().ToCharArray());
            }

            if (StartField().IsMatch(line)) {
                var match = StartField().Match(line);
                start = (mapRows.Count - 1, match.Index);
            }
        }

        if (start == (-1, -1)) {
            throw new InvalidDataException("No starting location");
        }

        var origionalLocation = start;
        var origionalDirection = Direction.Up;

        var path = new HashSet<Location> { };

        while (true) {
            _ = path.Add(origionalLocation);
            var target = origionalDirection! switch {
                Direction.Up => new Location(origionalLocation.row - 1, origionalLocation.col),
                Direction.Down => new Location(origionalLocation.row + 1, origionalLocation.col),
                Direction.Right => new Location(origionalLocation.row, origionalLocation.col + 1),
                Direction.Left => new Location(origionalLocation.row, origionalLocation.col - 1),
                _ => throw new InvalidDataException("Invalid direction"),
            };
            if (target.row < 0 || target.col < 0 || target.row >= mapRows.Count || target.col >= mapRows[0].Length) {
                break;
            }

            if (mapRows[target.row][target.col] == '#') {
                origionalDirection = origionalDirection == Direction.Left ? Direction.Up : origionalDirection + 1;
            }
            else {
                origionalLocation = target;
            }
        }

        foreach (var block in path) {
            var direction = Direction.Up;

            var history = new HashSet<History> { };
            var location = start;
            var loop = false;

            while (true) {
                if (!history.Add(new History(location, direction))) {
                    loop = true;
                    break;
                }

                var target = direction! switch {
                    Direction.Up => new Location(location.row - 1, location.col),
                    Direction.Down => new Location(location.row + 1, location.col),
                    Direction.Right => new Location(location.row, location.col + 1),
                    Direction.Left => new Location(location.row, location.col - 1),
                    _ => throw new InvalidDataException("Invalid direction"),
                };

                if (target.row < 0 || target.col < 0 || target.row >= mapRows.Count || target.col >= mapRows[0].Length) {
                    badBlocks++;
                    break;
                }

                if (mapRows[target.row][target.col] == '#' || target == block) {
                    direction = direction == Direction.Left ? Direction.Up : direction + 1;
                }
                else {
                    location = target;
                }
            }

            if (loop) {
                validBlocks++;
            }
        }

        Console.WriteLine($"Blocks causing loops: {validBlocks} (No loop: {badBlocks})");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
