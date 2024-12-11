using LibAoc;

int SolvePart1(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var trailEnds = new HashSet<Point2D>[map.Height, map.Width];

    foreach (var p in map.Positions()) {
        if (map[p] == '9') {
            trailEnds[p.Row, p.Col] = new HashSet<Point2D> { p };
        }
    }

    for (var height = '8'; height >= '0'; height--) {
        foreach (var p in map.Positions()) {
            if (map[p] != height) continue;
            var ends = new HashSet<Point2D>();
            foreach (var nb in map.Neighbors(p))
            {
                if (map[nb] == height + 1) {
                    ends.UnionWith(trailEnds[nb.Row, nb.Col]);
                }
            }
            trailEnds[p.Row, p.Col] = ends;
        }
    }

    var totalScore = 0;
    foreach (var p in map.Positions()) {
        if (map[p] == '0') {
            LogUtils.Log(p, trailEnds[p.Row, p.Col].Count);
            totalScore += trailEnds[p.Row, p.Col].Count;
        }
    }
    return totalScore;
}

int SolvePart2(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var trailEnds = new int[map.Height, map.Width];

    foreach (var p in map.Positions()) {
        if (map[p] == '9') {
            trailEnds[p.Row, p.Col] = 1;
        }
    }

    for (var height = '8'; height >= '0'; height--) {
        foreach (var p in map.Positions()) {
            if (map[p] != height) continue;
            var ends = 0;
            foreach (var nb in map.Neighbors(p))
            {
                if (map[nb] == height + 1) {
                    ends += trailEnds[nb.Row, nb.Col];
                }
            }
            trailEnds[p.Row, p.Col] = ends;
        }
    }

    var totalScore = 0;
    foreach (var p in map.Positions()) {
        if (map[p] == '0') {
            LogUtils.Log(p, trailEnds[p.Row, p.Col]);
            totalScore += trailEnds[p.Row, p.Col];
        }
    }
    return totalScore;
}

if (args.Length == 0) {
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
