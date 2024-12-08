using LibAoc;
using static LibAoc.LogUtils;

int SolvePart1(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var antennas = GetAntennas(map);
    var antinodes = new HashSet<Point2D>();
    foreach (var freqAntennas in antennas.Values) {
        foreach (var antinode in GetAntinodes(map, freqAntennas)) {
            antinodes.Add(antinode);
        }
    }

    return antinodes.Count;
}

int SolvePart2(IEnumerable<string> lines) {
    var map = new Map2D(lines.ToList());
    var antennas = GetAntennas(map);
    var antinodes = new HashSet<Point2D>();
    foreach (var freqAntennas in antennas.Values) {
        foreach (var antinode in GetAntinodesPart2(map, freqAntennas)) {
            antinodes.Add(antinode);
        }
    }

    return antinodes.Count;
}

Dictionary<char, List<Point2D>> GetAntennas(Map2D map) {
    var result = new Dictionary<char, List<Point2D>>();
    foreach (Point2D p in map.Positions()) {
        var f = map[p];
        if (f == '.') continue;
        if (!result.ContainsKey(f)) {
            result[f] = new List<Point2D>();
        }
        result[f].Add(p);
    }
    return result;
}

IEnumerable<Point2D> GetAntinodes(Map2D map, List<Point2D> antennas)
{
    foreach (var p1 in antennas) {
        foreach (var p2 in antennas) {
            if (p1 == p2) continue;

            var pResult = p2 + p2 - p1;
            if (map.ValidPoint(pResult))
                yield return pResult;
        }
    }
}

IEnumerable<Point2D> GetAntinodesPart2(Map2D map, List<Point2D> antennas)
{
    for (var i = 0; i < antennas.Count; i++) {
        for (var j = i + 1; j < antennas.Count; j++) {
            var p1 = antennas[i];
            var p2 = antennas[j];
            var d = p2 - p1;
            var current = p2;
            while (map.ValidPoint(current)) {
                yield return current;
                current = current + d;
            }
            current = p1;
            while (map.ValidPoint(current)) {
                yield return current;
                current = current - d;
            }
        }
    }
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
