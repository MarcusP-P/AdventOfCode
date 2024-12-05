using System.Data;
using System.Globalization;

#pragma warning disable IDE0060 // Remove unused parameter

// while wordgrid is an array of arrays, rather than a 2 dimensional array, it's assumed that all rows have equal length.
var wordGridRows = new List<char[]>();

var input = File.ReadLines("input.txt");

foreach (var line in input) {
    if (line.Length > 0) {
        var wordGridLine = line.Select(x => char.ToLower(x, CultureInfo.InvariantCulture)).ToArray();
        wordGridRows.Add(wordGridLine);
    }
}

var wordGrid = wordGridRows.ToArray();

var startLetterLocations = wordGrid
    .SelectMany((array, rowIndex) => array
            .Select((value, columnIndex) => new {
                value,
                columnIndex
            })
            .Where(value => value.value == 'a')
            .Select(position => (row: rowIndex, col: position.columnIndex)),
        (_, location) => location)
    .ToList();

var found = 0;

foreach (var location in startLetterLocations) {
    if (FindWord(wordGrid, location)) {
        found++;
    }
}

Console.WriteLine($"There are {startLetterLocations.Count} As, and {found} X-MAS");

static bool FindWord(char[][] wordGrid, (int row, int col) start) {
    if (start.row <= 0 || start.row >= wordGrid.Length - 1
        || start.col <= 0 || start.col >= wordGrid[0].Length - 1) {
        return false;
    }

    var topLeft = wordGrid[start.row - 1][start.col - 1];
    var topRight = wordGrid[start.row - 1][start.col + 1];
    var bottomLeft = wordGrid[start.row + 1][start.col - 1];
    var bottomRight = wordGrid[start.row + 1][start.col + 1];

    return ((topLeft == 'm' && bottomRight == 's')
            || (topLeft == 's' && bottomRight == 'm'))
        && ((topRight == 'm' && bottomLeft == 's')
            || (topRight == 's' && bottomLeft == 'm'));
}
#pragma warning restore IDE0060 // Remove unused parameter

