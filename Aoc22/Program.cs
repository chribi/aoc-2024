using LibAoc;
using static LibAoc.LogUtils;

long SolvePart1Line(string line) {
    var num = ulong.Parse(line);
    return (long)IterateTimes(num, 2000);
}

int StrategyId(int[] changes) {
    var result = 0;
    foreach (var c in changes) {
        result = result * 19 + c + 9;
    }
    return result;
}

int SolvePart2(IEnumerable<string> lines) {
    var secretNumbers = lines.Select(ulong.Parse).ToList();
    var totalBananas = new Dictionary<int, int>();

    for (var i = 0; i < secretNumbers.Count; i++) {
        var currentBananas = new Dictionary<int, int>();
        var changes = new int[4];

        var secret = secretNumbers[i];
        var lastPrice = (int)(secret % 10);

        for (var n = 1; n <= 2000; n++) {
            secret = Iterate(secret);
            var price = (int)(secret % 10);
            changes[0] = changes[1];
            changes[1] = changes[2];
            changes[2] = changes[3];
            changes[3] = price - lastPrice;
            lastPrice = price;

            if (n < 4) continue;
            var id = StrategyId(changes);
            if (!currentBananas.ContainsKey(id)) {
                currentBananas[id] = price;
            }
        }

        foreach (var kvp in currentBananas) {
            if (totalBananas.ContainsKey(kvp.Key)) {
                totalBananas[kvp.Key] += kvp.Value;
            } else {
                totalBananas[kvp.Key] = kvp.Value;
            }
        }
    }

    return totalBananas.Values.Max();
}

ulong Iterate(ulong number) {
    var n = Prune(Mix(number * 64ul, number));
    n = Prune(Mix(n / 32ul, n));
    n = Prune(Mix(n * 2048, n));
    return n;
}

ulong Prune(ulong number) => number & 0xFFFFFFul;

ulong Mix(ulong number, ulong other) => number ^ other;

ulong IterateTimes(ulong number, int times) {
    for (var i = 1; i <= times; i++) {
        number = Iterate(number);
    }
    return number;
}

if (args.Length == 0) {
    EnableLogging = true;
    var part1Cases = new (ulong, ulong)[] {
        (1, 8685429),
        (10, 4700978),
        (100, 15273692),
        (2024, 8667524),
    };
    TestUtils.Test("Iterate 2000x", n => IterateTimes(n, 2000), part1Cases);

    var nums = new List<ulong> { 123, 15887950, 16495136, 527345, 704524,
        1553684, 12683156, 11100544, 12249484, 7753432, 5908254, };
    for (var i = 0; i < nums.Count - 1; i++) {
        var cur = nums[i];
        var next = nums[i+1];
        TestUtils.TestCase($"Iterate({cur})", Iterate, cur, next);
    }

    var sample = new List<string> { "1", "2", "3", "2024" };
    TestUtils.TestCase("Sample Part2", SolvePart2, sample, 23);
} else {
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart1Line));
    Utils.AocMain(args, SolvePart2);
}
