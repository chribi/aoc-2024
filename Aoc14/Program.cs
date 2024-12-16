using LibAoc;
using static LibAoc.LogUtils;

long SolvePart1(IEnumerable<string> lines) {
    var robots = lines.Select(ReadRobot).ToList();
    var afterMoving = Simulate(robots, 100, 101, 103);
    var (q1, q2, q3, q4) = CountByQuadrant(afterMoving, 101, 103);
    return q1 * q2 * q3 * q4;
}
long SolvePart2(IEnumerable<string> lines) {
    var robots = lines.Select(ReadRobot).ToList();
    var width = 101;
    var height = 103;
    for (var step = 1; step < 20000; step++) {

        var map = new Map2D(width, height);
        foreach (var robot in robots) {
            long x = (robot.Pos.X + robot.Vel.X * step) % width;
            long y = (robot.Pos.Y + robot.Vel.Y * step) % height;
            if (x < 0) x += width;
            if (y < 0) y += height;
            map[(int)y, (int)x] = 'X';
        }

        // check for filled center
        if (map[50, 49] == 'X' && map[50, 50] == 'X' && map[50, 51] == 'X'
           && map[51, 49] == 'X' && map[51, 50] == 'X' && map[51, 51] == 'X'
           && map[52, 49] == 'X' && map[52, 50] == 'X' && map[52, 51] == 'X') {
            Console.WriteLine($"Step {step}");
            map.Print();
        }
    }

    return 0;
}

List<Point2D> Simulate(List<(Point2D Pos, Point2D Vel)> robots, long steps, long width, long height) {
    var result = new List<Point2D>(robots.Count);
    foreach (var robot in robots) {
        long x = (robot.Pos.X + robot.Vel.X * steps) % width;
        long y = (robot.Pos.Y + robot.Vel.Y * steps) % height;
        if (x < 0) x += width;
        if (y < 0) y += height;
        result.Add((x, y));
    }

    return result;
}

(long, long, long, long) CountByQuadrant(List<Point2D> positions, long width, long height) {
    var midX = width / 2;
    var midY = height / 2;
    long q1 = 0;
    long q2 = 0;
    long q3 = 0;
    long q4 = 0;

    foreach (var p in positions) {
        if (p.X == midX || p.Y == midY) continue;
        if (p.X < midX) {
            if (p.Y < midY) q1++; else q4++;
        } else {
            if (p.Y < midY) q2++; else q3++;
        }
    }
    return (q1, q2, q3, q4);
}

(Point2D Pos, Point2D Vel) ReadRobot(string line) {
    var nums = ParseUtils.GetNumbers(line);
    return ((nums[0], nums[1]), (nums[2], nums[3]));
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    if (args.Length > 1 && args[1] == "find") {
        Utils.AocMain(args, SolvePart2);
    }
}
