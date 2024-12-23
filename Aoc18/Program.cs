using LibAoc;
using static LibAoc.LogUtils;

int SolvePart1(IEnumerable<string> lines) {
    var map = new Map2D(71, 71);
    foreach(var line in lines.Take(1024)) {
        var c = ParseUtils.GetNumbers(line);
        map[(c[0], c[1])] = '#';
    }

    return AStarPathLength(map, (0, 0), (70, 70));
}

string SolvePart2(IEnumerable<string> lines) {
    var map = new Map2D(71, 71);
    var linesL = lines.ToList();
    foreach(var line in linesL.Take(1024)) {
        var c = ParseUtils.GetNumbers(line);
        map[(c[0], c[1])] = '#';
    }

    for (var i = 1024; i < linesL.Count; i++) {
        var c = ParseUtils.GetNumbers(linesL[i]);
        map[(c[0], c[1])] = '#';
        var length = AStarPathLength(map, (0, 0), (70, 70));
        Log(i, linesL[i], length);
        if (length < 0) return linesL[i];
    }

    return "ERROR";
}

int AStarPathLength(Map2D map, Point2D start, Point2D end) {
    var dists = new int[map.Height, map.Width];
    foreach (var p in map.Positions()) {
        dists[p.Row, p.Col] = int.MaxValue;
    }
    dists[start.Row, start.Col] = 0;
    var open = new Dictionary<Point2D, int>();
    var closed = new HashSet<Point2D>();
    open.Add(start, 0);
    while (open.Any()) {
        var current = open.MinBy(kvp => kvp.Value);
        if (current.Key == end) return dists[end.Row, end.Col];
        open.Remove(current.Key);
        closed.Add(current.Key);

        foreach(var nb in map.Neighbors(current.Key)) {
            if (map[nb] == '#') continue;
            if (closed.Contains(nb)) continue;
            var newDist = dists[current.Key.Row, current.Key.Col] + 1;
            if (newDist >= dists[nb.Row, nb.Col]) continue;
            dists[nb.Row, nb.Col] = newDist;
            var f = newDist + (int)Point2D.Dist(nb, end);
            if (open.ContainsKey(nb))
                open[nb] = f;
            else
                open.Add(nb, f);
        }
    }

    return -1;
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}

