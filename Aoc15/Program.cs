using LibAoc;
using static LibAoc.LogUtils;

(Map2D Map, Point2D StartPos, string Commands) ParseInput(IEnumerable<string> lines) {
    var blocks = Utils.SplitAtEmptyLines(lines).ToList();
    var map = new Map2D(blocks[0]);
    var commands = string.Concat(blocks[1]);
    var startPos = map.Positions().First(p => map[p] == '@');
    map[startPos] = '.';
    return (map, startPos, commands);
}

long SolvePart1(IEnumerable<string> lines) {
    var (map, pos, commands) = ParseInput(lines);

    foreach (var c in commands) {
        var dir = c.AsDirection();
        if (!CanMove(map, pos, dir)) continue;

        Move(map, pos, dir);
        pos += dir.AsVector();
    }

    var result = 0L;
    foreach (var p in map.Positions()) {
        if (map[p] != 'O') continue;

        result += 100 * p.Row + p.Col;
    }

    return result;
}

bool CanMove(Map2D map, Point2D pos, Direction dir) {
    while (true) {
        pos = pos.Move(dir, 1);
        if (map[pos] == '#') return false;
        if (map[pos] == '.') return true;
    }
}

void Move(Map2D map, Point2D pos, Direction dir) {
    var last = map[pos];
    while (true) {
        pos = pos.Move(dir, 1);
        if (map[pos] == '.') {
            map[pos] = last;
            break;
        }

        var tmp = map[pos];
        map[pos] = last;
        last = tmp;
    }
}

long SolvePart2(IEnumerable<string> lines) {
    var (map, pos, commands) = ParseInput(
            lines.Select(l => l
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.")));

    foreach (var c in commands) {
        var dir = c.AsDirection();
        // if (!CanMove2(map, pos, dir)) continue;

        if (TryMove2(map, pos, dir)) {
            pos += dir.AsVector();
        }
    }

    var result = 0L;
    foreach (var p in map.Positions()) {
        if (map[p] != '[') continue;

        result += 100 * p.Row + p.Col;
    }

    return result;
}

bool TryMove2(Map2D map, Point2D pos, Direction dir) {
    if (dir == Direction.R || dir == Direction.L)
    {
        var canMove = CanMove(map, pos, dir);
        if (canMove) Move(map, pos, dir);
        return canMove;
    }

    var d = dir.AsVector();
    var nextPos = pos + d;
    if (map[nextPos] == '.') return true;
    if (map[nextPos] == '#') return false;

    var i = 0;
    var boxes = new List<Point2D>();

    if (map[nextPos] == '[') boxes.Add(nextPos);
    if (map[nextPos] == ']') boxes.Add(nextPos - (0, 1));

    // Check and collect boxes to move
    while (i < boxes.Count) {
        var current = boxes[i];
        // Check left position
        nextPos = current + d;
        if (map[nextPos] == '#') return false;
        if (map[nextPos] == '[') boxes.Add(nextPos);
        if (map[nextPos] == ']') boxes.Add(nextPos - (0, 1));
        // Check right position
        nextPos += (0, 1);
        if (map[nextPos] == '#') return false;
        if (map[nextPos] == '[') boxes.Add(nextPos);
        // map[nextPos] == ']' already handled via left position
        i++;
    }

    // Move boxes
    for (i = boxes.Count - 1; i >= 0; i--) {
        var box = boxes[i];
        map[box] = '.';
        map[box + (0, 1)] = '.';
        map[box + d] = '[';
        map[box + d + (0, 1)] = ']';
    }

    return true;
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
