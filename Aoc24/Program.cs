using LibAoc;
using static LibAoc.LogUtils;


(Dictionary<string, int> Inputs, Dictionary<string, Gate> Gates)
ReadInput(IEnumerable<string> lines) {
    var inputBlocks = Utils.SplitAtEmptyLines(lines).ToArray();
    var inputs = inputBlocks[0]
        .Select(line => line.Split(':'))
        .ToDictionary(a => a[0], a => int.Parse(a[1]));
    var gates = inputBlocks[1]
        .Select(line => line.Split(' '))
        .ToDictionary(a => a[4], a => new Gate(a[1], a[0], a[2]));
    return (inputs, gates);
}

ulong SolvePart1(IEnumerable<string> lines) {
    var (inputs, gates) = ReadInput(lines);

    while (gates.Any()) {
        var canEval = gates.Where(kvp => inputs.ContainsKey(kvp.Value.In1) && inputs.ContainsKey(kvp.Value.In2));
        foreach (var (target, gate) in canEval) {
            gates.Remove(target);
            var value = EvalGate(gate, inputs);
            inputs[target] = value;
        }
    }

    var zValues = inputs.Where(kvp => kvp.Key[0] == 'z').OrderBy(kvp => kvp.Key);
    var result = 0ul;
    var shift = 0;
    foreach (var val in zValues) {
        Log(val);
        result += (ulong)val.Value << shift;
        shift++;
    }

    return result;
}

string SolvePart2(IEnumerable<string> lines) {
    var (_, gates) = ReadInput(lines);
    /*
    var zGates = gates.Keys.Where(name => name[0] == 'z').Order();
    foreach (var zGate in zGates) {
        Log("==========");
        Log($"{zGate}: {BuildExpression(zGate, gates)}");
        var relevantInputs = RelevantInputs(zGate, gates).Order().ToList();
        Log("Inputs:" + string.Join(" ", relevantInputs));
    }
    */

    // Not really an automated solution ... the swaps where determined by repeatedly running
    // the code below until it found an error, then fixing that error by carefully looking at
    // the input and swapping two wires.
    SwapWires("z10", "mkk", gates);
    SwapWires("z14", "qbw", gates);
    SwapWires("wjb", "cvp", gates);
    SwapWires("z34", "wcb", gates);

    var swappedWires = new List<string> { "z10", "mkk", "z14", "qbw", "wjb", "cvp", "z34", "wcb" };

    // z_n = x_n ^ y_n ^ c_n-1
    // where c_n-1 is carry from last position:
    // c_n = dc_n + oc_n
    // with dc_n = x_n & y_n
    //      oc_n = c_n-1 & (x_n ^ y_n)
    var renamings = new Dictionary<string, string>();
    Identify("C_00", new Gate("AND", "x00", "y00"));
    for (var n = 1; n <= 44; n++) {

        Identify($"ADD_{n:d2}", new Gate("XOR", $"x{n:d2}", $"y{n:d2}"));
        Identify($"DC_{n:d2}", new Gate("AND", $"x{n:d2}", $"y{n:d2}"));
        Identify($"OC_{n:d2}", new Gate("AND", $"C_{n-1:d2}", $"ADD_{n:d2}"));
        Identify($"C_{n:d2}", new Gate("OR", $"DC_{n:d2}", $"OC_{n:d2}"));
        var z = FindGate(gates, new Gate("XOR", $"C_{n-1:d2}", $"ADD_{n:d2}"));
        Log($"z{n:d2} == ", z);

    }
    return string.Join(",", swappedWires.Order());

    void Identify(string newName, Gate gate) {
        var oldName = FindGate(gates, gate);
        if (oldName == null) throw new Exception($"No gate {gate} for {newName}");
        // if (oldName[0] == 'z') throw new Exception($"Gate {gate} for {newName} is output {oldName}");
        RenameWire(oldName, newName, gates);
        Log(oldName, " => ", newName);
        renamings.Add(newName, oldName);
    }
}



string? FindGate(Dictionary<string, Gate> gates, Gate gate) {
    foreach (var (n, g) in gates) {
        if (g.Op == gate.Op && (g.In1 == gate.In1 && g.In2 == gate.In2
                || g.In2 == gate.In1 && g.In1 == gate.In2))
            return n;
    }
    return null;
}

void RenameWire(string oldName, string newName, Dictionary<string, Gate> gates) {
    gates[newName] = gates[oldName];
    gates.Remove(oldName);
    foreach (var (name, gate) in gates) {
        gates[name] = gate with {
            In1 = gate.In1 == oldName ? newName : gate.In1,
            In2 = gate.In2 == oldName ? newName : gate.In2,
        };
    }
}

void SwapWires(string wireA, string wireB, Dictionary<string, Gate> gates) {
    var temp = gates[wireA];
    gates[wireA] = gates[wireB];
    gates[wireB] = temp;
}

string BuildExpression(string name, Dictionary<string, Gate> gates) {
    if (!gates.ContainsKey(name)) return name;
    var expr = gates[name];
    var left = BuildExpression(expr.In1, gates);
    var right = BuildExpression(expr.In2, gates);
    return $"({left} {expr.Op} {right})";
}

HashSet<string> RelevantInputs(string name, Dictionary<string, Gate> gates) {
    if (!gates.ContainsKey(name)) return new HashSet<string> { name };
    var expr = gates[name];
    var left = RelevantInputs(expr.In1, gates);
    var right = RelevantInputs(expr.In2, gates);
    left.UnionWith(right);
    return left;
}

int EvalGate(Gate gate, Dictionary<string, int> inputs) {
    var val1 = inputs[gate.In1];
    var val2 = inputs[gate.In2];
    switch (gate.Op) {
        case "OR": return val1 | val2;
        case "XOR": return val1 ^ val2;
        case "AND": return val1 & val2;
        default: throw new Exception("Bad gate: " + gate.ToString());
    }
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}

record class Gate(string Op, string In1, string In2);
