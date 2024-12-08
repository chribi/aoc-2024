using LibAoc;
using static LibAoc.LogUtils;

int SolvePart1(IEnumerable<string> lines) {
    var data = lines.ToArray();
    return
        CountXmas(1, 0, data)
        + CountXmas(0, 1, data)
        + CountXmas(1, 1, data)
        + CountXmas(-1, 0, data)
        + CountXmas(0, -1, data)
        + CountXmas(-1, -1, data)
        + CountXmas(1, -1, data)
        + CountXmas(-1, 1, data);
}

int SolvePart2(IEnumerable<string> lines) {
    var data = lines.ToArray();
    var cols = data[0].Length;
    var rows = data.Length;

    var count = 0;
    for (var c = 1; c < cols - 1; c++) {
        for (var r = 1; r < rows - 1; r++) {
            if (data[r][c] != 'A') continue;
            if (!(data[r-1][c-1] == 'M' && data[r+1][c+1] == 'S'
                        || data[r-1][c-1] == 'S' && data[r+1][c+1] == 'M'))
                continue;
            if (!(data[r+1][c-1] == 'M' && data[r-1][c+1] == 'S'
                        || data[r+1][c-1] == 'S' && data[r-1][c+1] == 'M'))
                continue;

            count++;
        }
    }
    return count;
}

int CountXmas(int colDir, int rowDir, string[] data) {
    var cols = data[0].Length;
    var rows = data.Length;

    var count = 0;
    for (var c = 0; c < cols; c++) {
        for (var r = 0; r < rows; r++) {
            var cEnd = c + 3*colDir;
            var rEnd = r + 3*rowDir;

            if (cEnd < 0 || cEnd >= cols || rEnd < 0 || rEnd >= rows) {
                continue;
            }

            if (data[r][c] == 'X'
                    && data[r + rowDir][c + colDir] == 'M'
                    && data[r + 2*rowDir][c + 2*colDir] == 'A'
                    && data[r + 3*rowDir][c + 3*colDir] == 'S') {
                count++;
            }
        }
    }
    return count;
}

if (args.Length == 0) {
    EnableLogging = true;
    var part1Cases = new (string[], int)[] {
        (new [] {
         "MMMSXXMASM",
         "MSAMXMSMSA",
         "AMXSXMAAMM",
         "MSAMASMSMX",
         "XMASAMXAMM",
         "XXAMMXXAMA",
         "SMSMSASXSS",
         "SAXAMASAAA",
         "MAMMMXMMMM",
         "MXMXAXMASX"
         }, 18),
    };
    var part2Cases = new (string[], int)[] {
        (new [] {
         "MMMSXXMASM",
         "MSAMXMSMSA",
         "AMXSXMAAMM",
         "MSAMASMSMX",
         "XMASAMXAMM",
         "XXAMMXXAMA",
         "SMSMSASXSS",
         "SAXAMASAAA",
         "MAMMMXMMMM",
         "MXMXAXMASX"
         }, 9),
    };
    TestUtils.Test("SolvePart1", SolvePart1, part1Cases);
    TestUtils.Test("SolvePart2", SolvePart2, part2Cases);
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
