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

var counts = diskStructure
        .Select(x => x ?? -1)
        .GroupBy(item => item)
        .ToDictionary(item => item.Key, item => item.Count());

//drawDisk(diskStructure);

var currentFileStart = diskStructure.Length - 1;
var currentFileId = diskStructure[^1];
var fileLength = 0;
int? lastMoved = null;

// Compact disk
for (var currentBlock = diskStructure.Length - 1; currentBlock >= 0; currentBlock--) {

    // Becasue we are working from the end, each subsequent file must be less than the previpus file id. If it's not,
    // then it's a file we've moved before, so we can skip it like nulls.
    if (diskStructure[currentBlock] == null || (lastMoved != null && diskStructure[currentBlock] >= lastMoved)) {
        continue;
    }

    // We have come across a different fileId. That means we can now look for a place for the previous file
    if (currentFileId != diskStructure[currentBlock]) {
        var nextEmptySpace = 0;
        var emptySpaceRun = 0;
        var runStart = 0;
        lastMoved = currentFileId ?? -1;

        // Don't stop searching the for a large enough cluster of free space until we are at the start of the file we are moving.
        // We need to use currentFileStart - (fileLength - 1) because we may have skipped over nulls, making currentBlock unreliable
        for (nextEmptySpace = 0; nextEmptySpace <= currentFileStart - (fileLength - 1) && emptySpaceRun < fileLength; nextEmptySpace++) {
            if (diskStructure[nextEmptySpace] != null) {
                emptySpaceRun = 0;
                runStart = 0;
                continue;
            }

            if (runStart == 0) {
                runStart = nextEmptySpace;
            }

            emptySpaceRun++;
        }

        // If we've come across a large enough run to store the file, move it.
        if (fileLength <= emptySpaceRun) {
            for (var copyBlock = 0; copyBlock < fileLength; copyBlock++) {
                diskStructure[runStart + copyBlock] = diskStructure[currentFileStart - copyBlock];
                diskStructure[currentFileStart - copyBlock] = null;
            }

            //drawDisk(diskStructure);
        }

        // Reset the variables containing file information so we can start tracking the next file.
        fileLength = 1;
        currentFileId = diskStructure[currentBlock];
        currentFileStart = currentBlock;

    }
    // If we've gotten to here, then we are still processing the same file.
    else {
        fileLength++;
        continue;
    }
}

var counts2 = diskStructure
        .Select(x => x ?? -1)
        .GroupBy(item => item)
        .ToDictionary(item => item.Key, item => item.Count());

// verify that we have the same files
var dictionariesEqual =
    counts.Keys.Count == counts2.Keys.Count &&
    counts.Keys.All(k => counts2.ContainsKey(k) && counts2[k] == counts[k]);

// Calculate the checksum
accumulator = diskStructure
    .Select((i, pos) => pos * (i == null ? 0L : (long)i))
    .Sum();

Console.WriteLine($"Result {accumulator}");

/*static void drawDisk(int?[] disk) {
    var len = disk.Max()?.ToString(CultureInfo.InvariantCulture).Length ?? 0;
    Console.WriteLine(string.Join("", disk.Select(x => $"[{(x == null ? ".".PadLeft(len, '.') : x?.ToString(CultureInfo.InvariantCulture)?.PadLeft(len, '0'))}]")));
}*/
