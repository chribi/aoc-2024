using System.Text.RegularExpressions;
using LibAoc;
using static LibAoc.LogUtils;

Regex MulRegex = new Regex(@"mul\((?<a>\d{1,3}),(?<b>\d{1,3})\)", RegexOptions.Compiled);

int SolvePart1Line(string line) {
    var matches = MulRegex.Matches(line);
    var result = 0;
    foreach (Match m in matches) {
        var a = int.Parse(m.Groups["a"].Value);
        var b = int.Parse(m.Groups["b"].Value);
        result += a*b;
    }
    return result;
}

Regex MulRegex2 = new Regex(@"(mul\((?<a>\d{1,3}),(?<b>\d{1,3})\)|do\(\)|don't\(\))", RegexOptions.Compiled);
int SolvePart2(IEnumerable<string> lines) {
    var enabled = true;
    var result = 0;
    foreach (var line in lines) {
        var matches = MulRegex2.Matches(line);
        foreach (Match m in matches) {
            if (m.Value == "do()") {
                enabled = true;
            } else if (m.Value == "don't()") {
                enabled = false;
            } else {
                if (!enabled) continue;
                var a = int.Parse(m.Groups["a"].Value);
                var b = int.Parse(m.Groups["b"].Value);
                result += a*b;
            }
        }
    }
    return result;
}

if (args.Length == 0) {
    EnableLogging = true;
    var part1Cases = new (string, int)[] {
        ("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", 161),
    };
    TestUtils.Test("SolvePart1Line", SolvePart1Line, part1Cases);


} else {
    Utils.AocMain(args, Utils.SolveLineByLine(SolvePart1Line));
    Utils.AocMain(args, SolvePart2);
}
