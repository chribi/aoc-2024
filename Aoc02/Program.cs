using LibAoc;
using static LibAoc.LogUtils;

int SolvePart1Line(string line) {
    var nums = ParseUtils.GetNumbers(line);

    return IsSafe(nums) ? 1 : 0;
}

int SolvePart2Line(string line) {
    var nums = ParseUtils.GetNumbers(line);

    return IsSafeDampened(nums) ? 1 : 0;
}

bool IsSafe(List<long> numbers) {
    var increasing = numbers[0] < numbers[1];
    return Enumerable
        .Zip(numbers, numbers.Skip(1))
        .All(p => SafePair(increasing, p.First, p.Second));
}

bool SafePair(bool increasing, long num1, long num2) {
    var diff = Math.Abs(num1 - num2);
    return num1 < num2 == increasing && 1 <= diff && diff <= 3;
}

bool IsSafeDampened(List<long> numbers) {
    if (IsSafe(numbers)) return true;

    var inc1 = numbers[0] < numbers[1];
    var inc2 = numbers[1] < numbers[2];
    var inc3 = numbers[2] < numbers[3];

    var increasing = inc1 == inc2 ? inc1 : inc3;
    var badPairIndex = Enumerable.Range(0, numbers.Count - 1)
        .First(i => !SafePair(increasing, numbers[i], numbers[i + 1]));
    var numbersCopy = numbers.ToList();
    numbersCopy.RemoveAt(badPairIndex);
    numbers.RemoveAt(badPairIndex + 1);
    return IsSafe(numbersCopy) || IsSafe(numbers);
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart1Line));
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart2Line));
}
