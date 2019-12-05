using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode.IntCode
{
    /// <summary>
    /// Emulator for IntCode programs
    /// </summary>
    public class IntCodeEmulator
    {
        /// <summary>
        /// Indicates that a parameter is unused in an operation
        /// </summary>
        private const int Unused = -1;

        /// <summary>
        /// Instruction map
        /// </summary>
        private readonly Dictionary<int, Instruction> Instructions;

        /// <summary>
        /// Program instructions
        /// </summary>
        public int[] Program { get; }

        /// <summary>
        /// Instruction pointer
        /// </summary>
        public int Pointer { get; private set; }

        /// <summary>
        /// Program result (in register 0)
        /// </summary>
        public int Result => this.Program[0];

        /// <summary>
        /// Program noun (in register 1)
        /// </summary>
        public int Noun
        {
            get => this.Program[1];
            set => this.Program[1] = value;
        }

        /// <summary>
        /// Program verb (in register 2)
        /// </summary>
        public int Verb
        {
            get => this.Program[2];
            set => this.Program[2] = value;
        }

        /// <summary>
        /// Standard input
        /// </summary>
        public Queue<int> StdIn { get; }

        /// <summary>
        /// Standard output
        /// </summary>
        public StringBuilder StdOut { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="IntCodeEmulator"/> class.
        /// </summary>
        /// <param name="program">Program instructions</param>
        /// <param name="stdIn">Standard input</param>
        /// <param name="stdOut">Standard output</param>
        public IntCodeEmulator(IReadOnlyList<string> program, Queue<int> stdIn = null, StringBuilder stdOut = null)
        {
            this.Program = program[0].Numbers();
            this.StdIn = stdIn;
            this.StdOut = stdOut;
            this.Pointer = 0;

            this.Instructions = new Dictionary<int, Instruction>
            {
                [OpCodes.Halt]     = new Instruction(OpCodes.Halt,     0, (a, b, c) => { }),
                [OpCodes.Add]      = new Instruction(OpCodes.Add,      3, (a, b, c) => this.Program[c] = a + b),
                [OpCodes.Multiply] = new Instruction(OpCodes.Multiply, 3, (a, b, c) => this.Program[c] = a * b),
                [OpCodes.Input]    = new Instruction(OpCodes.Input,    1, (a, b, c) => this.Program[a] = this.StdIn.Dequeue()),
                [OpCodes.Output]   = new Instruction(OpCodes.Output,   1, (a, b, c) => this.StdOut.Append(a)),
                [OpCodes.JumpZ]    = new Instruction(OpCodes.JumpZ,    2, (a, b, c) => this.Pointer = a == 0 ? b - 2 : this.Pointer), // take off 2 to allow pointer to jump after
                [OpCodes.JumpNZ]   = new Instruction(OpCodes.JumpNZ,   2, (a, b, c) => this.Pointer = a != 0 ? b - 2 : this.Pointer), // take off 2 to allow pointer to jump after
                [OpCodes.Lt]       = new Instruction(OpCodes.Lt,       3, (a, b, c) => this.Program[c] = a < b ? 1 : 0),
                [OpCodes.Eq]       = new Instruction(OpCodes.Eq,       3, (a, b, c) => this.Program[c] = a == b ? 1 : 0),
            };
        }

        /// <summary>
        /// Execute the program until a Halt instruction (code 99) is received
        /// </summary>
        public void Execute()
        {
            while (true)
            {
                int rawOpCode = this.Program[this.Pointer];

                if (rawOpCode == OpCodes.Halt)
                {
                    return;
                }

                // skip over the opcode to the args
                this.Pointer++;

                // parse opcode
                (int opCode, ParameterMode modeA, ParameterMode modeB, ParameterMode modeC) = ParseOpcode(rawOpCode);

                // get the instruction and args
                Instruction instruction = this.Instructions[opCode];
                int[] args = this.Program.Skip(this.Pointer).Take(instruction.Args).Pad(3, Unused).ToArray();

                // dereference the args
                if (modeA == ParameterMode.Position && args[0] != Unused && opCode != OpCodes.Input) // input can never be immediate
                {
                    args[0] = this.Program[args[0]];
                }
                if (modeB == ParameterMode.Position && args[1] != Unused)
                {
                    args[1] = this.Program[args[1]];
                }
                if (modeC == ParameterMode.Position && args[2] != Unused)
                {
                    // this can't actually happen I don't think - outputs are always to a particular address
                    //args[2] = this.Program[args[2]];
                }

                // invoke the action, which may change the program or the pointer
                instruction.Action.Invoke(args[0], args[1], args[2]);

                // skip the args to the next instruction
                this.Pointer += instruction.Args;
            }
        }

        /// <summary>
        /// Decode "immediate mode" opcodes to the opcode plus its parameter modes
        /// </summary>
        /// <param name="rawOpCode">Raw opcode value</param>
        /// <returns></returns>
        private static (int opCode, ParameterMode modeA, ParameterMode modeB, ParameterMode modeC) ParseOpcode(int rawOpCode)
        {
            if (rawOpCode < 100)
            {
                return (rawOpCode, ParameterMode.Position, ParameterMode.Position, ParameterMode.Position);
            }

            int opCode = rawOpCode % 100;

            string s = rawOpCode.ToString().PadLeft(5, '0');

            // this is awful, fix later
            return (opCode,
                    s[2] == '1' ? ParameterMode.Immediate : ParameterMode.Position,
                    s[1] == '1' ? ParameterMode.Immediate : ParameterMode.Position,
                    s[0] == '1' ? ParameterMode.Immediate : ParameterMode.Position);
        }
    }
}
