using LibAoc;
using static LibAoc.LogUtils;


(List<(long,long)> Rules, List<List<long>> Updates) ReadInput(IEnumerable<string> lines) {
    var split = Utils.SplitAtEmptyLines(lines).ToList();
    var rules = split[0].Select(s => ParseUtils.GetNumbers(s)).Select(ns => (ns[0], ns[1])).ToList();
    var updates = split[1].Select(ParseUtils.GetNumbers).ToList();
    return (rules, updates);
}

long SolvePart1(IEnumerable<string> lines) {
    var (rules, updates) = ReadInput(lines);

    return updates
        .Where(u => IsCorrectOrder(u, rules))
        .Select(u => u[(u.Count - 1) / 2])
        .Sum();
}

bool IsCorrectOrder(List<long> update, List<(long, long)> rules) {
    foreach (var (a, b) in rules) {
        var posA = update.IndexOf(a);
        var posB = update.IndexOf(b);
        if (posA >= 0 && posB >= 0 && posA > posB) {
            return false;
        }
    }

    return true;
}

long SolvePart2(IEnumerable<string> lines) {
    var (rules, updates) = ReadInput(lines);

    return updates
        .Where(u => !IsCorrectOrder(u, rules))
        .Select(u => FixOrder(u, rules))
        .Select(u => u[(u.Count - 1) / 2])
        .Sum();
}

List<long> FixOrder(List<long> update, List<(long,long)> rules) {
    bool sorted = true;
    do {
        sorted = true;
        foreach (var (a, b) in rules) {
            var posA = update.IndexOf(a);
            var posB = update.IndexOf(b);
            if (posA >= 0 && posB >= 0 && posA > posB) {
                sorted = false;
                update[posA] = b;
                update[posB] = a;
            }
        }
    } while (!sorted);
    return update;
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
