using System.Globalization;
using Location = (int row, int col);

var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

var currentRow = -1;
var currentCol = -1;

var trailHeads = new List<Location>();
var rows = new List<int[]>();

await foreach (var line in input) {
    if (line.Trim().Length > 0) {
        currentRow++;
        currentCol = -1;
        var row = line.Trim().ToArray().Select(x => int.Parse(x.ToString(), CultureInfo.InvariantCulture)).ToArray();
        rows.Add(row);

        foreach (var cellValue in row) {
            currentCol++;
            if (cellValue == 0) {
                trailHeads.Add(new Location(currentRow, currentCol));
            }
        }
    }
    else {
        Console.WriteLine($"Line: \"{line}\".");
    }
}

var map = rows.ToArray();

var validPathCount = 0;

foreach (var trailHead in trailHeads) {
    var pathQueues = new Queue<Location>();

    var trailEnds = new HashSet<Location>();

    pathQueues.Enqueue(trailHead);

    while (pathQueues.Count > 0) {
        var currentLocation = pathQueues.Dequeue();
        if (map[currentLocation.row][currentLocation.col] == 9) {
            _ = trailEnds.Add(currentLocation);
            continue;
        }

        var surroundingCells = new Queue<Location>();
        surroundingCells.Enqueue(new(currentLocation.row - 1, currentLocation.col));
        surroundingCells.Enqueue(new(currentLocation.row, currentLocation.col - 1));
        surroundingCells.Enqueue(new(currentLocation.row, currentLocation.col + 1));
        surroundingCells.Enqueue(new(currentLocation.row + 1, currentLocation.col));
        while (surroundingCells.Count > 0) {
            var nextLocation = surroundingCells.Dequeue();

            // if it's out of bounds, ignore it.
            if (nextLocation.row < 0 || nextLocation.row >= map.Length || nextLocation.col < 0 || nextLocation.col >= map[0].Length) {
                continue;
            }

            if (map[nextLocation.row][nextLocation.col] == map[currentLocation.row][currentLocation.col] + 1) {
                pathQueues.Enqueue(nextLocation);
            }
        }
    }

    validPathCount += trailEnds.Count;
}

Console.WriteLine($"Result {validPathCount}");
