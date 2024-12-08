using LibAoc;
using static LibAoc.LogUtils;

long SolvePart1Line(string line) {
    var nums = ParseUtils.GetNumbers(line);
    var target = nums[0];
    var last = nums.Count - 1;

    var vals = new Stack<(long, int)>();
    vals.Push((nums[1], 1));
    while (vals.Any()) {
        var (current, i) = vals.Pop();
        if (i == last)  {
            if (current == target) return target;
            continue;
        }

        // No chance to reach target => early termination
        if (current > target) continue;

        var next = nums[i + 1];
        vals.Push((current * next, i + 1));
        vals.Push((current + next, i + 1));
    }
    return 0;
}

long SolvePart2Line(string line) {
    var nums = ParseUtils.GetNumbers(line);
    var target = nums[0];
    var last = nums.Count - 1;

    var vals = new Stack<(long, int)>();
    vals.Push((nums[1], 1));
    while (vals.Any()) {
        var (current, i) = vals.Pop();
        if (i == last)  {
            if (current == target) return target;
            continue;
        }

        // No chance to reach target => early termination
        if (current > target) continue;

        var next = nums[i + 1];
        vals.Push((current * next, i + 1));
        vals.Push((current + next, i + 1));
        vals.Push((Concat(current, next), i + 1));
    }
    return 0;
}

long Concat(long n, long m) {
    return long.Parse(n.ToString() + m.ToString());
}

if (args.Length == 0) {
    EnableLogging = true;
    var part1Cases = new (string, long)[] {
        ("190: 10 19", 190),
        ("83: 17 5", 0),
        ("3267: 81 40 27", 3267),
    };
    TestUtils.Test("SolvePart1Line", SolvePart1Line, part1Cases);
} else {
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart1Line));
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart2Line));
}
