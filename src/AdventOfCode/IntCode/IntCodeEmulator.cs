using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode.IntCode
{
    public class IntCodeEmulator
    {
        private readonly Dictionary<int, Instruction> Instructions;

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

        public Queue<int> Input { get; }

        public StringBuilder Output { get; }

        public IntCodeEmulator(IReadOnlyList<string> program, Queue<int> input = null, StringBuilder output = null)
        {
            this.Program = program[0].Numbers();
            this.Input = input;
            this.Output = output;
            this.Pointer = 0;

            this.Instructions = new Dictionary<int, Instruction>
            {
                [OpCodes.Halt]     = new Instruction(OpCodes.Halt,     0, (p, a, b, c) => { }),
                [OpCodes.Add]      = new Instruction(OpCodes.Add,      3, (p, a, b, c) => p[c] = a + b),
                [OpCodes.Multiply] = new Instruction(OpCodes.Multiply, 3, (p, a, b, c) => p[c] = a * b),
                [OpCodes.Input]    = new Instruction(OpCodes.Input,    1, (p, a, b, c) => p[a] = this.Input.Dequeue()),
                [OpCodes.Output]   = new Instruction(OpCodes.Output,   1, (p, a, b, c) => this.Output.Append(a)),
                [OpCodes.JumpZ]    = new Instruction(OpCodes.JumpZ,    2, (p, a, b, c) => this.Pointer = a == 0 ? b - 2 : this.Pointer), // take off 2 to allow pointer to jump after
                [OpCodes.JumpNZ]   = new Instruction(OpCodes.JumpNZ,   2, (p, a, b, c) => this.Pointer = a != 0 ? b - 2 : this.Pointer), // take off 2 to allow pointer to jump after
                [OpCodes.Lt]       = new Instruction(OpCodes.Lt,       3, (p, a, b, c) => p[c] = a < b ? 1 : 0),
                [OpCodes.Eq]       = new Instruction(OpCodes.Eq,       3, (p, a, b, c) => p[c] = a == b ? 1 : 0),
            };
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

                // parse opcode
                (int opCode, int modeA, int modeB, int modeC) decoded = ParseOpcode(opCode);

                // get the instruction and args
                Instruction instruction = this.Instructions[decoded.opCode];
                int[] args = this.Program.Skip(this.Pointer).Take(instruction.Args).Pad(3, -1).ToArray();

                // dereference the args
                if (decoded.modeA == 0 && args[0] > -1 && decoded.opCode != OpCodes.Input)
                {
                    args[0] = this.Program[args[0]];
                }
                if (decoded.modeB == 0 && args[1] > -1)
                {
                    args[1] = this.Program[args[1]];
                }
                if (decoded.modeC == 0 && args[2] > -1)
                {
                    // this can't actually happen I don't think
                    //args[2] = this.Program[args[2]];
                }

                // invoke the action
                instruction.Action.Invoke(this.Program, args[0], args[1], args[2]);

                // skip the args to the next op code
                this.Pointer += instruction.Args;
            }
        }

        private static (int opCode, int modeA, int modeB, int modeC) ParseOpcode(int opCode)
        {
            if (opCode < 100)
            {
                return (opCode, 0, 0, 0);
            }

            int parsedOpCode = opCode % 100;

            string s = opCode.ToString().PadLeft(5, '0');

            // this is awful, fix later
            return (parsedOpCode, int.Parse(s[2].ToString()), int.Parse(s[1].ToString()), int.Parse(s[0].ToString()));
        }
    }
}
