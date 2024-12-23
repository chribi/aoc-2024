using LibAoc;
using static LibAoc.LogUtils;

(long, long) SolvePart1And2(IEnumerable<string> lines) {
    var towels = lines.First().Split(",")
        .Select(s => s.Trim())
        .ToHashSet();
    var count = 0L;
    var sum = 0L;
    foreach (var pattern in lines.Skip(2)) {
        var arrangements = PatternArrangements(pattern, towels);
        if (arrangements > 0) count++;
        sum += arrangements;
    }
    return (count, sum);
}

long PatternArrangements(string pattern, HashSet<string> towels)
{
    Log(pattern, pattern.Length);
    var maxLength = towels.Select(s => s.Length).Max();
    var possiblePrefixes = new long[pattern.Length + 1];
    possiblePrefixes[0] = 1;

    for (var prefix = 0; prefix < pattern.Length; prefix++)
    {
        if (possiblePrefixes[prefix] == 0) continue;

        for (var length = 1; length <= Math.Min(maxLength, pattern.Length - prefix); length++) {
            var s = pattern.Substring(prefix, length);
            if (towels.Contains(s)) {
                possiblePrefixes[prefix + length] += possiblePrefixes[prefix];
            }
        }
    }
    return possiblePrefixes[pattern.Length];
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1And2);
}
