using System.Globalization;

var input = File.ReadLinesAsync("input.txt", CancellationToken.None);

var accumulator = 0L;

int?[] diskStructure = [];

await foreach (var line in input) {
    if (diskStructure.Length == 0) {
        var inputs = line.ToArray();
        var currentFile = 0;
        var diskLayout = new List<int?> { };
        var freeSpace = false;

        foreach (var currentBlocks in inputs) {
            var blockRun = byte.Parse(currentBlocks.ToString(), CultureInfo.InvariantCulture);
            for (var currentblock = 1; currentblock <= blockRun; currentblock++) {
                diskLayout.Add(freeSpace ? null : currentFile);
            }

            if (!freeSpace) {
                currentFile++;
            }

            freeSpace = !freeSpace;
        }

        diskStructure = [.. diskLayout];
    }

    else {
        Console.WriteLine($"Line: \"{line}\".");
    }
}

var nextEmptySpace = 0;
// Compact disk
for (var currentBlock = diskStructure.Length - 1; currentBlock >= 0; currentBlock--) {

    if (diskStructure[currentBlock] == null) {
        continue;
    }

    while (nextEmptySpace <= currentBlock && diskStructure[nextEmptySpace] != null) {
        nextEmptySpace++;
    }

    if (nextEmptySpace >= currentBlock) {
        break;
    }

    diskStructure[nextEmptySpace] = diskStructure[currentBlock];
    diskStructure[currentBlock] = null;
}

accumulator = diskStructure
    .Select((i, pos) => pos * (i == null ? 0L : (long)i))
    .Sum();

Console.WriteLine($"Result {accumulator}");
