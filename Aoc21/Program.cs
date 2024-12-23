using LibAoc;
using static LibAoc.LogUtils;

using DirpadMemo = long[,,];

long SolveLine(string sequence, DirpadMemo memo, int depth) {
    var cost = GetCostNumpad(sequence, memo, depth);
    Log(sequence, cost);
    var value = int.Parse(sequence.Substring(0, 3));
    return cost * value;
}

long SolvePart1(IEnumerable<string> lines) {
    var total = 0L;
    var memo = BuildDirpadMemo(3);
    foreach (var seq in lines) {
        total += SolveLine(seq, memo, 3);
    }
    return total;
}
long SolvePart2(IEnumerable<string> lines) {
    var total = 0L;
    var memo = BuildDirpadMemo(26);
    foreach (var seq in lines) {
        total += SolveLine(seq, memo, 26);
    }
    return total;
}

DirpadMemo BuildDirpadMemo(int maxDepth) {
    var memo = new long[5, 5, maxDepth];
    // memo[currentPos, targetBtn, depth] = minimal end user cost to press 'targetBtn'
    // at given depth when robot is currently at currentPos

    // depth 0 == user input ==> everything can be pressed directly
    var dirpadButtons = "^<v>A";
    foreach (var from in dirpadButtons)
        foreach (var to in dirpadButtons)
            memo[DirpadIdx(from), DirpadIdx(to), 0] = 1;

    for (var depth = 1; depth < maxDepth; depth++) {
        foreach (var from in dirpadButtons) {
            foreach (var to in dirpadButtons) {
                var (s, sAlternate) = ShortestSequenceDirPad(from, to);
                var cost = GetCost(s, memo, depth - 1);
                if (sAlternate != null) {
                    var costAlternate = GetCost(sAlternate, memo, depth - 1);
                    cost = cost < costAlternate ? cost : costAlternate;
                }
                memo[DirpadIdx(from), DirpadIdx(to), depth] = cost;
            }
        }
    }

    return memo;
}

long GetCostNumpad(string seq, DirpadMemo memo, int depth) {
    var total = 0L;
    var current = 'A';
    foreach (var btn in seq) {
        var (s, sAlternate) = ShortestSequenceNumpad(current, btn);
        var cost = GetCost(s, memo, depth - 1);

        if (sAlternate != null) {
            var costAlternate = GetCost(sAlternate, memo, depth - 1);
            cost = cost < costAlternate ? cost : costAlternate;
            s = sAlternate;
        }
        Log(current, btn, s, cost);

        total += cost;
        current = btn;
    }
    return total;
}

long GetCost(string seq, DirpadMemo memo, int depth) {
    var cost = 0L;
    var current = 'A';
    foreach (var btn in seq) {
        cost += memo[DirpadIdx(current), DirpadIdx(btn), depth];
        current = btn;
    }

    return cost;
}

(int, int) NumpadPos(char btn) {
    if (btn == 'A') return (0, 2);
    if (btn == '0') return (0, 1);
    var num = btn - '0';
    var row = 1 + (num - 1) / 3;
    var col = (num - 1) % 3;
    return (row, col);
}

(int, int) DirpadPos(char btn) {
    return btn switch {
        '^' => (1, 1),
        '<' => (0, 0),
        'v' => (0, 1),
        '>' => (0, 2),
        _   => (1, 2),
    };
}

int DirpadIdx(char btn) {
    return btn switch {
        '^' => 0,
        '<' => 1,
        'v' => 2,
        '>' => 3,
        _   => 4,
    };
}

(string, string?) ShortestSequence((int Row, int Col) from, (int Row, int Col) to, (int Row, int Col) avoid) {
    if (from == to) return ("A", null);
    var updown = new string(from.Row < to.Row ? '^' : 'v', Math.Abs(from.Row - to.Row));
    var leftright = new string(from.Col < to.Col ? '>' : '<', Math.Abs(from.Col - to.Col));

    if (from.Row == to.Row) return (leftright + "A", null);
    if (from.Col == to.Col) return (updown + "A", null);
    if (from.Col == avoid.Col && to.Row == avoid.Row) {
        return (leftright + updown + "A", null);
    }
    if (from.Row == avoid.Row && to.Col == avoid.Col) {
        return (updown + leftright + "A", null);
    }

    return (updown + leftright + "A", leftright + updown + "A");
}

(string, string?) ShortestSequenceNumpad(char from, char to) {
    var fromPos = NumpadPos(from);
    var toPos = NumpadPos(to);
    return ShortestSequence(fromPos, toPos, (0, 0));
}

(string, string?) ShortestSequenceDirPad(char from, char to) {
    var fromPos = DirpadPos(from);
    var toPos = DirpadPos(to);
    return ShortestSequence(fromPos, toPos, (1, 0));
}

if (args.Length == 0) {
    EnableLogging = true;
    var memo = BuildDirpadMemo(26);
    var part1Cases = new (string, long)[] {
        ("029A", 1972),
        ("980A", 58800),
        ("179A", 12172),
        ("456A", 29184),
        ("379A", 24256),
    };
    TestUtils.Test("SolvePart1Line", s => SolveLine(s, memo, 3), part1Cases);
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
