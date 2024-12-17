using LibAoc;
using static LibAoc.LogUtils;


(State, List<long>) ParseInput(IEnumerable<string> lines) {
    var input = lines.Select(ParseUtils.GetNumbers).ToList();
    var program = input[4];
    return (new State(input[0][0], input[1][0], input[2][0], 0), program);
}

State? RunStep(State s, List<long> program, List<long> output) {
    if (s.IP >= program.Count) return null;
    var instr = program[s.IP];
    var operand = program[s.IP + 1];

    switch (instr) {
        case 0: return ADV(s, operand);
        case 1: return BXL(s, operand);
        case 2: return BST(s, operand);
        case 3: return JNZ(s, operand);
        case 4: return BXC(s, operand);
        case 5: return OUT(s, operand, output);
        case 6: return BDV(s, operand);
        case 7: return CDV(s, operand);
    }

    return null;
}

State ADV(State s, long operand)
{
    var denom = (int)Combo(s, operand);
    return s with {
        A = s.A >> denom,
        IP = s.IP + 2,
    };
}

State BXL(State s, long operand)
{
    return s with {
        B = s.B ^ operand,
        IP = s.IP + 2,
    };
}

State BST(State s, long operand)
{
    var op = Combo(s, operand);
    return s with {
        B = op & 0x7,
        IP = s.IP + 2,
    };
}

State JNZ(State s, long operand)
{
    return s with {
        IP = s.A == 0 ? s.IP + 2 : (int)operand,
    };
}

State BXC(State s, long operand)
{
    return s with {
        B = s.B ^ s.C,
        IP = s.IP + 2,
    };
}

State OUT(State s, long operand, List<long> output)
{
    output.Add(Combo(s, operand) & 0x7);
    return s with {
        IP = s.IP + 2,
    };
}

State BDV(State s, long operand)
{
    var denom = (int)Combo(s, operand);
    return s with {
        B = s.A >> denom,
        IP = s.IP + 2,
    };
}
State CDV(State s, long operand)
{
    var denom = (int)Combo(s, operand);
    return s with {
        C = s.A >> denom,
        IP = s.IP + 2,
    };
}

long Combo(State s, long operand)
{
    if (operand <= 3) return operand;
    if (operand == 4) return s.A;
    if (operand == 5) return s.B;
    if (operand == 6) return s.C;
    throw new InvalidOperationException($"Combo Operand {operand}, {s}");
}

string SolvePart1(IEnumerable<string> lines) {
    var (s, program) = ParseInput(lines);
    var output = new List<long>();
    while (s != null) {
        s = RunStep(s, program, output);
    }
    return string.Join(",", output);
}

ulong SolvePart2(IEnumerable<string> lines) {
    var (_, program) = ParseInput(lines);
    // Program is equivalent to
    // B := [A] & 0x7
    // B := B ^ 0b10
    // C := A >> [B]
    // B := B ^ 0x111
    // B := B ^ C
    // A := A >> 3
    // OUT [B]
    // JNZ 0
    // ==> Solve 3 bits at a time
    var min = ulong.MaxValue;
    var bits48 = 0xFFFF_FFFFF_FFFFul;
    Stack<(ulong Bits, ulong BitsKnown, int Index)> attempts = new();
    attempts.Push((0, ~bits48, 0));

    while (attempts.Any()) {
        var (bits, known, i) = attempts.Pop();
        if (i >= program.Count) {
            Log($">>> Result {bits} ({bits:B48})");
            if (bits < min) min = bits;
            continue;
        }

        var knownA = (known >> (i * 3)) & 0b111;
        if (knownA != 0b111)
        {
            var bitsA = (bits >> (i * 3)) & 0b111;
            var newKnown = known | (0b111ul << (i * 3));
            Log(">>>>>>");
            Log("i", i);
            Log("value", bits.ToString("B48"), bits);
            Log("known", (known & bits48).ToString("B48"));
            Log("bitsA", bitsA);
            for (ulong possibleA = 0; possibleA < 8; possibleA++) {

                // Value does not align with bits already known => continue;
                if (((possibleA ^ bitsA) & knownA) != 0) continue;
                var newBits = bits | (possibleA << (i * 3));
                Log("  >>", newBits.ToString("B48"));
                attempts.Push((newBits, newKnown, i));
            }
        } else {
            var target = program[i];
            var a = bits >> i*3;
            var k = known >> i*3;
            var bShift = (a & 0b111) ^ 0b010;
            var bitsC = (a >> (int)bShift) & 0b111;
            var b = bShift ^ 0b111;
            var cTarget = b ^ (ulong)target;
            var cShift = i * 3 + (int)bShift;
            var cKnown = (known >> cShift) & 0b111;
            Log("======");
            Log("i", i);
            Log("target", target, target.ToString("B3"));
            Log("     ", "_5_|4|_3_|2|_1_|0|_9_|8|_7_|6|_5_|4|_3_|2|_1_|0|");
            Log("value", bits.ToString("B48"), bits);
            Log("known", (known & bits48).ToString("B48"));
            Log("a", a.ToString("B3"));
            Log("bShift", bShift.ToString("B3"));
            Log("cShift", cShift);
            Log("b", b.ToString("B3"));
            Log("cTarget", cTarget.ToString("B3"), "bitsC", bitsC.ToString("B3"), "cKnown", cKnown.ToString("B3"));

            if (((cTarget ^ bitsC) & cKnown) != 0) {
                Log("Impossible!");
                continue;
            }
            var newBits = bits | (cTarget << cShift);
            var newKnown = known | (0b111ul << cShift);
            Log($"Continue: {newBits:B48}");
            Log($"          {newKnown & bits48:B48}");
            Log($"          {0b111ul << cShift:B48}");
            attempts.Push((newBits, newKnown, i + 1));
        }
    }

    return min;
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}

record State(long A, long B, long C, int IP);
