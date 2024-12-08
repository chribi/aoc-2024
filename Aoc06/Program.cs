using LibAoc;
using static LibAoc.LogUtils;

int SolvePart1(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var visited = GetPath(map);
    return visited.Count();
}

List<Point2D> GetPath(Map2D map) {
    var pos = (Point2D)map.Positions().First(p => map[p] == '^');
    var visited = new List<Point2D>();
    var direction = Direction.U;

    while (map.ValidPoint(pos)) {
        if (!visited.Contains(pos))
            visited.Add(pos);

        var nextPos = pos.Move(direction, 1);
        if (map.ValidPoint(nextPos) && map[nextPos] == '#') {
            direction = direction.NextClockwise();
        } else {
            pos = nextPos;
        }
    }
    return visited;
}

bool HasLoop(Map2D map) {
    var pos = (Point2D)map.Positions().First(p => map[p] == '^');
    var visited = new List<(Point2D, Direction)>();
    var direction = Direction.U;

    while (true) {
        if (!visited.Contains((pos, direction))) {
            visited.Add((pos, direction));
        } else {
            return true;
        }

        var nextPos = pos.Move(direction, 1);
        while (map.ValidPoint(nextPos) && map[nextPos] == '#') {
            direction = direction.NextClockwise();
            nextPos = pos.Move(direction, 1);
        }

        if (!map.ValidPoint(pos)) return false;

        pos = nextPos;
    }
}

int SolvePart2(IEnumerable<string> lines) {
    var initialMap = new Map2D(lines.ToList());
    var candidatePositions = GetPath(initialMap);

    var loopPositions = 0;
    // skip start pos of guard
    foreach (var position in candidatePositions.Skip(1)) {
        var map = initialMap.Clone();
        Console.WriteLine("Testing " + position);
        map[position] = '#';
        if (HasLoop(map)) loopPositions++;
    }
    return loopPositions;
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
