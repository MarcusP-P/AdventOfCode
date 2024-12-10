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
        .Select(
            x => new {
                Location = x,
                Pairings = antennas
                    .SkipWhile(y => y != x)
                    .Skip(1)
                    .Select(y => y),
            })
        .SelectMany(x => x.Pairings,
            (x, y) => (antenna1: x.Location,
                antenna2: y,
                difference: new Location(y.row - x.Location.row, y.col - x.Location.col)))
        .Select(x => new List<Location> {
            new(x.antenna1.row-x.difference.row, x.antenna1.col-x.difference.col),
            new(x.antenna2.row+x.difference.row, x.antenna2.col+x.difference.col),
        })
        .SelectMany(x => x)
        .Where(x => x.row >= 0 && x.row <= currentRow && x.col >= 0 && x.col <= currentCol);
    antinodes = antinodes.Union(currentAntinodes).ToHashSet();
}

Console.WriteLine($"Result {antinodes.Count}");
