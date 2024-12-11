using LibAoc;
using static LibAoc.LogUtils;

(long, long?) BlinkStone(long stone) {
    if (stone == 0) return (1, null);

    var s = stone.ToString();
    if (s.Length % 2 == 0) {
        var half = s.Length / 2;
        return (long.Parse(s.Substring(0, half)),
                long.Parse(s.Substring(half)));
    }
    return (stone * 2024, null);
}

Dictionary<long, long> Blink(Dictionary<long, long> stones) {
    var next = new Dictionary<long, long>();
    foreach (var (stone, count) in stones) {
        var (a, b) = BlinkStone(stone);
        next.AddOrSet(a, count);
        if (b is long bb)
            next.AddOrSet(bb, count);
    }
    return next;
}

long Solve(string line, long numBlinks) {
    var stonesList = ParseUtils.GetNumbers(line);
    var stones = new Dictionary<long, long>();
    foreach (var s in stonesList) {
        stones.AddOrSet(s, 1);
    }
    for (var i = 0; i < numBlinks; i++) {
        stones = Blink(stones);
    }

    return stones.Values.Sum();
}

long SolvePart1Line(string line) {
    return Solve(line, 25);
}

long SolvePart2Line(string line) {
    return Solve(line, 75);
}

if (args.Length == 0) {
    EnableLogging = true;
    var part1Cases = new (string, long)[] {
        ("125 17", 55312),
    };
    TestUtils.Test("SolvePart1Line", SolvePart1Line, part1Cases);
} else {
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart1Line));
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart2Line));
}
