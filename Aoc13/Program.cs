using LibAoc;
using static LibAoc.LogUtils;

long SolvePart1(IEnumerable<string> lines) {
    return Utils.SplitAtEmptyLines(lines)
        .Select(m => SolveMachine(m, 0))
        .Sum();
}

long SolvePart2(IEnumerable<string> lines) {
    long offset = 10000000000000L;
    return Utils.SplitAtEmptyLines(lines)
        .Select(m => SolveMachine(m, offset))
        .Sum();
}

long SolveMachine(List<string> description, long offset = 0) {
    var buttonA = ReadXY(description[0]);
    var buttonB = ReadXY(description[1]);
    var price = ReadXY(description[2]);
    price.X += offset;
    price.Y += offset;

    var d = buttonA.X * buttonB.Y - buttonA.Y * buttonB.X;
    var dr = buttonA.X * price.Y - buttonA.Y * price.X;
    if (d == 0 || dr % d != 0) {
        return 0;
    }
    var b = dr / d;
    var c = price.X - b * buttonB.X;
    if (c % buttonA.X != 0) {
        return 0;
    }
    var a = c / buttonA.X;
    return 3*a + b;
}


(long X, long Y) ReadXY(string line) {
    var nums = ParseUtils.GetNumbers(line);
    return (nums[0], nums[1]);
}


if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
