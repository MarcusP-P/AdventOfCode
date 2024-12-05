using System.Data;
using System.Globalization;

#pragma warning disable IDE0060 // Remove unused parameter

// while wordgrid is an array of arrays, rather than a 2 dimensional array, it's assumed that all rows have equal length.
var wordGridRows = new List<char[]>();

var searchDirections = new List<(int row, int col)> { (1, -1), (1, 0), (1, 1), (0, -1), (0, 1), (-1, -1), (-1, 0), (-1, 1) };

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
            .Where(value => value.value == 'x')
            .Select(position => (row: rowIndex, col: position.columnIndex)),
        (_, location) => location)
    .ToList();

var found = 0;

foreach (var location in startLetterLocations) {
    foreach (var direction in searchDirections) {
        if (FindWord(wordGrid, location, direction)) {
            found++;
        }
    }
}

Console.WriteLine($"There are {startLetterLocations.Count} xes, and {found} words");

static bool FindWord(char[][] wordGrid, (int row, int col) start, (int row, int col) direction) {
    char[] match = { 'x', 'm', 'a', 's' };

    // check to make sure that the word stays in bound
    var end = (row: start.row + (direction.row * 3), col: start.col + (direction.col * 3));
    if (end.row < 0 || end.row >= wordGrid.Length
        || end.col < 0 || end.col >= wordGrid[0].Length) {
        return false;
    }

    for (var i = 0; i < match.Length; i++) {
        var currentPos = (row: start.row + (direction.row * i), col: start.col + (direction.col * i));

        if (wordGrid[currentPos.row][currentPos.col] != match[i]) {
            return false;
        }
    }

    return true;
}
#pragma warning restore IDE0060 // Remove unused parameter

