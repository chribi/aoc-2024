using LibAoc;
using static LibAoc.LogUtils;

long SolvePart1(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var visited = new bool[map.Height, map.Width];

    var result = 0L;
    for (var row = 0; row < map.Height; row++) {
        for (var col = 0; col < map.Width; col++) {
            if (visited[row, col]) continue;

            var plant = map[row, col];
            var next = new Stack<Point2D>();
            next.Push((row, col));

            var area = 0L;
            var perimeter = 0L;
            while (next.Any()) {
                var current = next.Pop();
                if (visited[current.Row, current.Col]) continue;

                area++;
                perimeter += 4;
                visited[current.Row, current.Col] = true;
                foreach (var nb in map.Neighbors(current)) {
                    if (map[nb] == plant) {
                        perimeter--;
                        if (!visited[nb.Row, nb.Col]) {
                            next.Push(nb);
                        }
                    }
                }
            }

            result += area * perimeter;
        }
    }
    return result;
}

long SolvePart2(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var visited = new bool[map.Height, map.Width];

    var result = 0L;
    for (var row = 0; row < map.Height; row++) {
        for (var col = 0; col < map.Width; col++) {
            if (visited[row, col]) continue;

            var plant = map[row, col];
            var next = new Stack<Point2D>();
            next.Push((row, col));

            var area = 0L;
            var perimeter = new List<(Point2D, Direction)>();
            while (next.Any()) {
                var current = next.Pop();
                if (visited[current.Row, current.Col]) continue;

                area++;
                visited[current.Row, current.Col] = true;
                foreach (var dir in new[] { Direction.U, Direction.D, Direction.L, Direction.R }) {
                    var nb = current + dir.AsVector();
                    if (map.ValidPoint(nb) && map[nb] == plant) {
                        if (!visited[nb.Row, nb.Col]) {
                            next.Push(nb);
                        }
                    }
                    else {
                       perimeter.Add((current, dir));
                    }
                }
            }

            var sides = CountSides(perimeter);

            result += area * sides;
        }
    }
    return result;
}

long CountSides(List<(Point2D, Direction)> perimeter) {
    var sides = 0;
    Log("CountSides");
    while (perimeter.Any()) {
        Log(string.Join("\n", perimeter));
        sides++;
        var current = perimeter[0];
        Log("Current", current);
        perimeter.RemoveAt(0);

        var pos = current.Item1;
        var dir = current.Item2;
        var growDir = dir.NextClockwise().AsVector();
        int i;
        while ((i = perimeter.IndexOf((pos + growDir, dir))) >= 0) {
            perimeter.RemoveAt(i);
            pos = pos + growDir;
        }

        pos = current.Item1;
        while ((i = perimeter.IndexOf((pos - growDir, dir))) >= 0) {
            perimeter.RemoveAt(i);
            pos = pos - growDir;
        }
    }

    Log("Sides", sides);
    return sides;
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
