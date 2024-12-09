using LibAoc;
using static LibAoc.LogUtils;

long SolvePart1Line(string line) {
    var fs = ReadFileSystem(line);
    return CompactedChecksum(fs.FileSizes, fs.FreeSizes);
}

long SolvePart2Line(string line) {
    var fs = ReadFileSystemWithOffsets(line);
    return CompactedChecksumPart2(fs.Files, fs.FreeSpaces);
}

(List<long> FileSizes, List<long> FreeSizes) ReadFileSystem(string diskMap) {
    var fileSizes = new List<long>((diskMap.Length + 1) / 2);
    var freeSizes = new List<long>(diskMap.Length / 2);

    for (var i = 0; 2*i < diskMap.Length; i++) {
        fileSizes.Add(diskMap[2*i] - '0');
        if (2*i + 1 < diskMap.Length)
            freeSizes.Add(diskMap[2*i + 1] - '0');
    }

    return (fileSizes, freeSizes);
}

(List<(int, long)> Files, List<(int, long)> FreeSpaces) ReadFileSystemWithOffsets(string diskMap) {
    var files = new List<(int, long)>((diskMap.Length + 1) / 2);
    var freeSpaces = new List<(int, long)>(diskMap.Length / 2);

    var offset = 0;
    for (var i = 0; 2*i < diskMap.Length; i++) {
        var size = diskMap[2*i] - '0';
        files.Add((offset, size));
        offset += size;
        if (2*i + 1 < diskMap.Length)
        {
            size = diskMap[2*i + 1] - '0';
            freeSpaces.Add((offset, size));
            offset += size;
        }
    }

    return (files, freeSpaces);
}

long CompactedChecksum(List<long> fileSizes, List<long> freeSizes) {
    var checksum = 0L;
    var currentFile = 0;
    var currentFileToMove = fileSizes.Count - 1;
    var blockIndex = 0;

    while (true) {
        // Sum file at current block position
        checksum += SumBlocks(blockIndex, currentFile, fileSizes[currentFile]);
        blockIndex += (int)fileSizes[currentFile];

        currentFile++;
        if (currentFile > currentFileToMove) return checksum;

        // Handle free space after that file
        var freeSpace = freeSizes[currentFile - 1];
        while (freeSpace >= fileSizes[currentFileToMove]) {
            var moveSize = fileSizes[currentFileToMove];
            checksum += SumBlocks(blockIndex, currentFileToMove, moveSize);
            blockIndex += (int)moveSize;
            freeSpace -= moveSize;
            currentFileToMove--;
            if (currentFile > currentFileToMove) return checksum;
        }

        if (freeSpace > 0) {
            checksum += SumBlocks(blockIndex, currentFileToMove, freeSpace);
            fileSizes[currentFileToMove] -= freeSpace;
            blockIndex += (int)freeSpace;
        }
    }
}

long CompactedChecksumPart2(List<(int, long)> files, List<(int, long)> freeSpaces) {
    for (var i = files.Count - 1; i > 0; i--) {
        var size = files[i].Item2;
        for (var j = 0; j < i; j++) {
            if (freeSpaces[j].Item2 >= size) {
                files[i] = (freeSpaces[j].Item1, size);
                freeSpaces[j] = (freeSpaces[j].Item1 + (int)size, freeSpaces[j].Item2 - size);
                break;
            }
        }
    }
    LogUtils.Log("Calculate checksum");

    var checksum = 0L;
    for (var i = 0; i < files.Count; i++) {
        checksum += SumBlocks(files[i].Item1, i, files[i].Item2);
    }
    return checksum;
}


long SumBlocks(int blockIndex, int fileIndex, long length) {
    return blockIndex * fileIndex * length + (length - 1) * length * fileIndex / 2;
}

if (args.Length == 0) {
    EnableLogging = true;
    var part1Cases = new (string, long)[] {
        ("2333133121414131402", 1928),
    };
    TestUtils.Test("SolvePart1Line", SolvePart1Line, part1Cases);
    var part2Cases = new (string, long)[] {
        ("2333133121414131402", 2858),
    };
    TestUtils.Test("SolvePart2Line", SolvePart2Line, part2Cases);
} else {
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart1Line));
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart2Line));
}
