using LibAoc;
using static LibAoc.LogUtils;

(long, long) SolvePart1And2(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var (dists, path) = GetDistsAndPath(map, map.Find('E'));
    var part1 = CountCheats(map, dists, path, 2);
    var part2 = CountCheats(map, dists, path, 20);
    return (part1, part2);
}

long CountCheats(Map2D map, long[,] dists, List<Point2D> path, int cheatLength) {
    var result = 0L;
    foreach (var cheatStart in path) {
        var distStart = dists[cheatStart.Row, cheatStart.Col];
        if (distStart < 102) continue;
        result += CheatEnds(map, cheatStart, cheatLength)
            .Where(end => dists[end.Item1.Row, end.Item1.Col] <= distStart - 100 - end.Item2)
            .Count();
    }

    return result;
}

IEnumerable<(Point2D, long)> CheatEnds(Map2D map, Point2D start, int maxLength) {
    for (var row = Math.Max(0, start.Row - maxLength);
            row <= Math.Min(map.Height - 1, start.Row + maxLength);
            row++) {
        for (var col = Math.Max(0, start.Col - maxLength);
                col <= Math.Min(map.Width - 1, start.Col + maxLength);
                col++) {
            var d = Point2D.Dist(start, (row, col));
            if (map[(row, col)] != '#' && d <= maxLength) {
                yield return ((row, col), d);
            }
        }
    }
}

(long[,], List<Point2D>) GetDistsAndPath(Map2D map, Point2D end) {
    var dists = new long[map.Height, map.Width];
    var path = new List<Point2D>();
    foreach (var p in map.Positions()) dists[p.Row, p.Col] = -1;
    dists[end.Row, end.Col] = 0;
    path.Add(end);
    var prev = end;
    var prevDist = 0;
    var current = map.Neighbors(prev).Where(p => map[p] != '#').Single();
    while (true) {
        path.Add(current);
        dists[current.Row, current.Col] = ++prevDist;
        var next = map.Neighbors(current).Where(p => p != prev && map[p] != '#').ToList();
        if (next.Count == 0) break;
        if (next.Count != 1) throw new Exception($"Invalid input @ {current}");
        prev = current;
        current = next[0];
    }

    return (dists, path);
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1And2);
}
