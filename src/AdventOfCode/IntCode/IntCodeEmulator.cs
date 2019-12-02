using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode.IntCode
{
    public class IntCodeEmulator
    {
        public static readonly Dictionary<int, Instruction> Instructions = new Dictionary<int, Instruction>
        {
            [OpCodes.Halt]     = new Instruction(OpCodes.Halt,     0, (p, a, b, c) => { }),
            [OpCodes.Add]      = new Instruction(OpCodes.Add,      3, (p, a, b, c) => p[c] = p[a] + p[b]),
            [OpCodes.Multiply] = new Instruction(OpCodes.Multiply, 3, (p, a, b, c) => p[c] = p[a] * p[b]),
        };

        public int[] Program { get; }

        public int Pointer { get; private set; }

        public int Result => this.Program[0];

        public int Noun
        {
            get => this.Program[1];
            set => this.Program[1] = value;
        }

        public int Verb
        {
            get => this.Program[2];
            set => this.Program[2] = value;
        }

        public IntCodeEmulator(IReadOnlyList<string> input)
        {
            this.Program = input[0].Numbers();
            this.Pointer = 0;
        }

        public void Execute()
        {
            while (true)
            {
                int opCode = this.Program[this.Pointer];

                if (opCode == OpCodes.Halt)
                {
                    return;
                }

                // skip to the args
                this.Pointer++;

                // invoke the action
                Instruction instruction = Instructions[opCode];
                int[] args = this.Program.Skip(this.Pointer).Take(instruction.Args).Pad(3, -1).ToArray();
                instruction.Action.Invoke(this.Program, args[0], args[1], args[2]);

                // skip the args to the next op code
                this.Pointer += instruction.Args;
            }
        }
    }
}
