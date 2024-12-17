using LibAoc;
using static LibAoc.LogUtils;

(long, long) SolvePart1And2(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var start = map.Find('S');
    var end = map.Find('E');

    return MinimalPath(map, start, end);
}

(long, long) MinimalPath(Map2D map, Point2D start, Point2D end) {
    // Run Djikstra on Position x Direction
    var distances = new long[map.Height, map.Width, 4];
    var prev = new List<(Point2D, Direction)>[map.Height, map.Width, 4];
    foreach (var p in map.Positions())
        for (var dir = 0; dir < 4; dir++)
        {
            distances[p.Row, p.Col, dir] = long.MaxValue;
            prev[p.Row, p.Col, dir] = new List<(Point2D, Direction)>();
        }

    distances[start.Row, start.Col, (int)Direction.R] = 0;

    var q = new List<((Point2D, Direction), long)>();
    q.Add(((start, Direction.R), 0));

    while (q.Count > 0) {
        var (current, curDist) = Dequeue(q);// Dequeue
        if (current.Item1 == end) {
            // Check if there are further paths in the queue that need to be counted
            if (!q.Any(item => item.Item1.Item1 == end && item.Item2 == curDist))
            {
                var ends = Enumerable.Range(0, 4)
                    .Where(d => distances[end.Row, end.Col, d] == curDist)
                    .Select(d => (end, (Direction)d));
                return (curDist, CountPrevNodes(prev, ends));
            }
        }
        distances[current.Item1.Row, current.Item1.Col, (int)current.Item2] = curDist;
        foreach (var (nb, nbDir, dist) in Neighbors(map, current.Item1, current.Item2)) {
            var d = dist + curDist;
            var curBest = distances[nb.Row, nb.Col, (int)nbDir];
            if (curBest < d) continue;
            if (curBest == d) {
                prev[nb.Row, nb.Col, (int)nbDir].Add(current);
                continue;
            }

            prev[nb.Row, nb.Col, (int)nbDir].Clear();
            prev[nb.Row, nb.Col, (int)nbDir].Add(current);
            distances[nb.Row, nb.Col, (int)nbDir] = d;
            UpdateQueue(q, (nb, nbDir), d);
        }
    }

    return (-1, -1);
}

long CountPrevNodes(List<(Point2D, Direction)>[,,] prev, IEnumerable<(Point2D, Direction)> ends)
{
    var pathPoints = new HashSet<(Point2D, Direction)>();
    var s = new Stack<(Point2D, Direction)>(ends);

    while (s.Count > 0) {
        var current = s.Pop();
        if (pathPoints.Contains(current)) continue;
        pathPoints.Add(current);

        var (pos, dir) = current;
        foreach (var p in prev[pos.Row, pos.Col, (int)dir]) {
            s.Push(p);
        }
    }

    var points = new HashSet<Point2D>(pathPoints.Select(p => p.Item1));
    return points.Count;
}

IEnumerable<(Point2D, Direction, long)> Neighbors(Map2D map, Point2D p, Direction dir) {
    for (var d = 0; d < 4; d++) {
        var otherDir = (Direction)d;
        if (otherDir != dir) yield return (p, otherDir, 1000);
    }
    var pNext = p + dir.AsVector();
    if (map[pNext] != '#') {
        yield return (pNext, dir, 1);
    }
}

T Dequeue<T>(List<T> l) {
    var first = l[0];
    l.RemoveAt(0);
    return first;
}

void UpdateQueue<T>(List<(T, long)> q, T elem, long dist) {
    var i = q.FindIndex(e => e.Item1.Equals(elem));
    if (i >= 0) q.RemoveAt(i);
    var j = q.FindIndex(e => e.Item2 > dist);
    if (j >= 0)
        q.Insert(j, (elem, dist));
    else
        q.Add((elem, dist));
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1And2);
}
