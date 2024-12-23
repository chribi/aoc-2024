using LibAoc;
using static LibAoc.LogUtils;
using Graph = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>;

int SolvePart1(IEnumerable<string> lines) {
    var g = BuildGraph(lines);
    return Find3Cliques(g).Count(clique => clique.Any(v => v.StartsWith("t")));
}

string SolvePart2(IEnumerable<string> lines) {
    var g = BuildGraph(lines);
    var maxClique = FindMaxClique(g);
    return string.Join(",", maxClique);
}

IEnumerable<string[]> Find3Cliques(Graph g) {
    foreach (var v in g.Keys) {
        var adj = g[v];
        if (adj.Count < 2) continue;
        for (var i = 0; i < adj.Count - 1; i++) {
            for (var j = i + 1; j < adj.Count; j++) {
                var v2 = adj[i];
                var v3 = adj[j];
                if (g.TryGetValue(v2, out var l) && l.Contains(v3)) {
                    var clique = new string[3];
                    clique[0] = v;
                    clique[1] = v2;
                    clique[2] = v3;
                    yield return clique;
                }
            }
        }
    }
}

IEnumerable<List<string>> FindCliques(Graph g, List<string> partial, List<string> remaining, int minSize) {
    var missing = minSize - partial.Count;
    var bound = missing <= 0
        ? remaining.Count
        : remaining.Count - missing + 1;
    for (var i = 0; i < bound; i++) {
        var vNext = remaining[i];
        var adjNext = g.TryGetValue(vNext, out var l) ? l : new List<string>();
        var remainingNext = remaining.Where(adjNext.Contains).ToList();

        if (remainingNext.Count + 1 + partial.Count < minSize) continue;

        var copy = partial.ToList();
        copy.Add(vNext);
        if (remainingNext.Count == 0)
            yield return copy;
        else
            foreach (var c in FindCliques(g, copy, remainingNext, minSize))
                yield return c;
    }
}

List<string> FindMaxClique(Graph g) {
    var currentMax = new List<string>();
    foreach (var v in g.Keys) {
        var adj = g[v];
        if (adj.Count < currentMax.Count) continue;

        var current = new List<string> { v };
        foreach (var clique in FindCliques(g, current, adj, currentMax.Count + 1)) {
            if (clique.Count > currentMax.Count) {
                currentMax = clique;
                Log("New best", currentMax.Count, string.Join(", ", currentMax));
            }

        }
    }
    return currentMax;
}

Graph BuildGraph(IEnumerable<string> connections) {
    Graph graph = new();
    // only record connections from smaller to bigger vertices
    foreach (var conn in connections) {
        var parts = conn.Split('-');
        if (parts[0].CompareTo(parts[1]) < 0)
            AddEdge(graph, parts[0], parts[1]);
        else
            AddEdge(graph, parts[1], parts[0]);
    }

    // adjecent vertices are listed in ascending order
    foreach (var adjList in graph.Values) {
        adjList.Sort();
    }
    return graph;
}

void AddEdge(Graph g, string from, string to) {
    if (g.ContainsKey(from)) {
        g[from].Add(to);
    } else {
        var adjFrom = new List<string> { to };
        g[from] = adjFrom;
    }
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
