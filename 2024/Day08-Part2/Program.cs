using Location = (int row, int col);

var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

var currentRow = -1;
var currentCol = -1;

var frequencyAntennas = new Dictionary<char, List<Location>>();
var antinodes = new HashSet<Location>();

await foreach (var line in input) {
    if (line.Trim().Length > 0) {
        currentRow++;
        currentCol = -1;
        var linePositions = line.Trim().ToCharArray();
        foreach (var frequency in linePositions) {
            currentCol++;
            if (frequency == '.') {
                continue;
            }

            if (!frequencyAntennas.TryGetValue(frequency, out var frequencyLocations)) {
                frequencyLocations = [];
                frequencyAntennas.Add(frequency, frequencyLocations);
            }

            frequencyLocations.Add(new Location(currentRow, currentCol));
        }
    }
    else {
        Console.WriteLine($"Line: \"{line}\".");
    }
}

foreach (var antennas in frequencyAntennas.Values) {
    var currentAntinodes = antennas
        // Create a struct of an antenna, and all antennas not yet paired with it
        .Select(
            x => new {
                Location = x,
                Pairings = antennas
                    .SkipWhile(y => y != x)
                    .Skip(1)
                    .Select(y => y),
            })
        // Flatten the above lists, so we have all antennas and the gap between them
        .SelectMany(x => x.Pairings,
            (x, y) => (antenna1: x.Location,
                antenna2: y,
                difference: new Location(y.row - x.Location.row, y.col - x.Location.col)))
        // Create a list of all of the antinodes
        .Select(x => {
            var allAntonodes = new List<Location>();
            var current = new Location(x.antenna1.row - x.difference.row, x.antenna1.col - x.difference.col);
            while (current.row >= 0
                && current.row <= currentRow
                && current.col >= 0
                && current.col <= currentCol) {
                allAntonodes.Add(current);
                current = new(current.row - x.difference.row, current.col - x.difference.col);
            }

            current = new(x.antenna2.row + x.difference.row, x.antenna2.col + x.difference.col);
            while (current.row >= 0
                && current.row <= currentRow
                && current.col >= 0
                && current.col <= currentCol) {
                allAntonodes.Add(current);
                current = new(current.row + x.difference.row, current.col + x.difference.col);
            }

            // Also add the two antennas
            allAntonodes.Add(x.antenna1);
            allAntonodes.Add(x.antenna2);

            return allAntonodes;
        })
        // flatten the list into a single list
        .SelectMany(x => x);
    antinodes = antinodes.Union(currentAntinodes).ToHashSet();
}

Console.WriteLine($"Result {antinodes.Count}");
