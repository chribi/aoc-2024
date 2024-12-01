using LibAoc;
using static LibAoc.LogUtils;

long SolvePart1(IEnumerable<string> lines) {
    var leftList = new List<long>();
    var rightList = new List<long>();

    foreach (var line in lines) {
        var nums = ParseUtils.GetNumbers(line);
        leftList.Add(nums[0]);
        rightList.Add(nums[1]);
    }

    leftList.Sort();
    rightList.Sort();

    return Enumerable
        .Zip(leftList, rightList)
        .Select((pair) => Math.Abs(pair.First - pair.Second))
        .Sum();
}

long SolvePart2(IEnumerable<string> lines) {
    var leftList = new List<long>();
    var rightList = new List<long>();

    foreach (var line in lines) {
        var nums = ParseUtils.GetNumbers(line);
        leftList.Add(nums[0]);
        rightList.Add(nums[1]);
    }

    return leftList.Select(n => n * rightList.Count(a => a == n)).Sum();
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
