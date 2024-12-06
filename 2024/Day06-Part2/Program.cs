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
        var lines = new List<char[]>();

        var validBlocks = 0;
        var badBlocks = 0;
        var invalidBlockPlacements = 0;

        var start = new Location(-1, -1);

        var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

        // while the data file is in the roder of rules, then pages, we won't assume that, so get all the
        // data first, then process it.
        await foreach (var line in input) {
            if (line.Trim().Length > 0) {
                lines.Add(line.Trim().ToCharArray());
            }

            if (StartField().IsMatch(line)) {
                var match = StartField().Match(line);
                start = (lines.Count - 1, match.Index);
            }
        }

        if (start == (-1, -1)) {
            throw new InvalidDataException("No starting location");
        }

        for (var block = new Location(0, 0); block.row <= lines.Count - 1; block.row++) {
            for (block.col = 0; block.col <= lines[block.row].Length - 1; block.col++) {
                if (lines[block.row][block.col] != '.') {
                    invalidBlockPlacements++;
                    continue;
                }

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

                    if (target.row < 0 || target.col < 0 || target.row >= lines.Count || target.col >= lines[0].Length) {
                        badBlocks++;
                        break;
                    }

                    if (lines[target.row][target.col] == '#' || target == block) {
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
        }

        Console.WriteLine($"Blocks causing loops: {validBlocks} (Not Tried: {invalidBlockPlacements}, not loop: {badBlocks})");
    }
#pragma warning restore IDE0060 // Remove unused parameter
}
