using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private const int Unused = -1234567;

        /// <summary>
        /// Instruction map
        /// </summary>
        private readonly Dictionary<OpCode, Instruction> Instructions;

        /// <summary>
        /// Program instructions
        /// </summary>
        public long[] Program { get; }

        /// <summary>
        /// Instruction pointer
        /// </summary>
        public long Pointer { get; private set; }

        /// <summary>
        /// Program result (in register 0)
        /// </summary>
        public int Result => (int)this.Program[0];

        /// <summary>
        /// The VM has halted
        /// </summary>
        public bool Halted { get; private set; }

        /// <summary>
        /// The VM is waiting if the next opcode requires input but stdin is currently empty
        /// </summary>
        public bool WaitingForInput => this.PeekNextOpCode() == OpCode.Input && !this.StdIn.Any();

        /// <summary>
        /// Program noun (in register 1)
        /// </summary>
        public int Noun
        {
            get => (int)this.Program[1];
            set => this.Program[1] = value;
        }

        /// <summary>
        /// Program verb (in register 2)
        /// </summary>
        public int Verb
        {
            get => (int)this.Program[2];
            set => this.Program[2] = value;
        }

        /// <summary>
        /// Standard input
        /// </summary>
        public Queue<long> StdIn { get; }

        /// <summary>
        /// Standard output
        /// </summary>
        public Queue<long> StdOut { get; }

        /// <summary>
        /// Offset for relative-mode parameters
        /// </summary>
        public long RelativeBase { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="IntCodeEmulator"/> class.
        /// </summary>
        /// <param name="program">Program instructions</param>
        public IntCodeEmulator(IReadOnlyList<string> program)
        {
            this.Program = program[0].Numbers().Select(n => (long)n).Pad(program[0].Length * 2).ToArray();
            this.StdIn = new Queue<long>();
            this.StdOut = new Queue<long>();
            this.Pointer = 0;
            this.RelativeBase = 0;

            this.Instructions = new[]
            {
                new Instruction(OpCode.Halt,       0,    (a, b, c) => { }),
                new Instruction(OpCode.Add,        3,    (a, b, c) => this.Program[c] = a + b),
                new Instruction(OpCode.Multiply,   3,    (a, b, c) => this.Program[c] = a * b),
                new Instruction(OpCode.Input,      1,    (a, b, c) => this.Program[a] = this.StdIn.Dequeue()),
                new Instruction(OpCode.Output,     1,    (a, b, c) => this.StdOut.Enqueue(a)),
                new Instruction(OpCode.JumpZ,      2,    (a, b, c) => this.Pointer = a == 0 ? b - 2 : this.Pointer), // take off 2 to allow pointer to jump after
                new Instruction(OpCode.JumpNZ,     2,    (a, b, c) => this.Pointer = a != 0 ? b - 2 : this.Pointer), // take off 2 to allow pointer to jump after
                new Instruction(OpCode.LessThan,   3,    (a, b, c) => this.Program[c] = a < b ? 1 : 0),
                new Instruction(OpCode.Equal,      3,    (a, b, c) => this.Program[c] = a == b ? 1 : 0),
                new Instruction(OpCode.Relative,   1,    (a, b, c) => this.RelativeBase += a),
            }.ToDictionary(o => o.OpCode);
        }

        /// <summary>
        /// Execute the program until a Halt instruction (code 99) is received
        /// </summary>
        public void Execute()
        {
            while (!this.Halted)
            {
                this.Step();
            }
        }

        /// <summary>
        /// Execute one instruction
        /// </summary>
        public void Step()
        {
            long rawOpCode = this.Program[this.Pointer];

            if (rawOpCode == (int) OpCode.Halt)
            {
                this.Halted = true;
            }

            if (Debugger.IsAttached)
            {
                Debug.Write($"{this.Pointer.ToString().PadLeft(5)}:\t\t");
            }

            // skip over the opcode to the args
            this.Pointer++;

            // parse opcode
            (OpCode opCode, ParameterMode modeA, ParameterMode modeB, ParameterMode modeC) = ParseOpcode(rawOpCode);

            // get the instruction and args
            Instruction instruction = this.Instructions[opCode];
            long[] args = this.Program.Skip((int)this.Pointer).Take(instruction.Args).Pad(3, Unused).ToArray();

            if (Debugger.IsAttached)
            {
                Debug.Write($"{instruction.OpCode.ToString().PadRight(12)}\t\t");
                Debug.Write($"{args[0].ToString().PadRight(10)}{modeA.ToString()[0]}\t\t\t");
                Debug.Write($"{args[1].ToString().PadRight(10)}{modeB.ToString()[0]}\t\t\t");
                Debug.Write($"{args[2].ToString().PadRight(10)}{modeC.ToString()[0]}\t\t\t");
                Debug.Write("|||\t\t\t");
            }

            // dereference the args
            this.DereferenceArguments(opCode, args, modeA, modeB, modeC);

            if (Debugger.IsAttached)
            {
                Debug.Write($"{string.Join("\t\t", args.Select(a => a.ToString().PadRight(15)))}");
            }

            // invoke the action, which may change the program or the pointer
            instruction.Action.Invoke(args[0], args[1], args[2]);

            if (Debugger.IsAttached)
            {
                Debug.WriteLine($"\t\t\t| {this.RelativeBase}");
            }

            // skip the args to the next instruction
            this.Pointer += instruction.Args;
        }

        /// <summary>
        /// Dereference the arguments depending on the parameter mode
        /// </summary>
        /// <param name="opCode">Current opcode</param>
        /// <param name="args">Args to modify</param>
        /// <param name="modeA">Mode for arg A</param>
        /// <param name="modeB">Mode for arg B</param>
        /// <param name="modeC">Mode for arg C</param>
        private void DereferenceArguments(OpCode opCode, long[] args, ParameterMode modeA, ParameterMode modeB, ParameterMode modeC)
        {
            long offsetA = modeA == ParameterMode.Relative ? this.RelativeBase : 0;
            long offsetB = modeB == ParameterMode.Relative ? this.RelativeBase : 0;
            long offsetC = modeC == ParameterMode.Relative ? this.RelativeBase : 0;

            if (modeA != ParameterMode.Immediate && args[0] != Unused)
            {
                if (opCode == OpCode.Input)
                {
                    // input arg always refers to a memory location, don't dereference
                    args[0] = args[0] + offsetA;
                }
                else
                {
                    args[0] = this.Program[args[0] + offsetA];
                }
            }

            if (modeB != ParameterMode.Immediate && args[1] != Unused)
            {
                args[1] = this.Program[args[1] + offsetB];
            }

            if (modeC != ParameterMode.Immediate && args[2] != Unused)
            {
                // don't dereference C because it's always a memory location destination (currently...)
                args[2] = args[2] + offsetC;
            }
        }

        /// <summary>
        /// Peek at what the next OpCode is without advancing the pointer
        /// </summary>
        /// <returns>Next opcode</returns>
        public OpCode PeekNextOpCode()
        {
            long rawOpCode = this.Program[this.Pointer];
            (OpCode opCode, ParameterMode _, ParameterMode _, ParameterMode _) = ParseOpcode(rawOpCode);
            return opCode;
        }

        /// <summary>
        /// Decode "immediate mode" opcodes to the opcode plus its parameter modes
        /// </summary>
        /// <param name="rawOpCode">Raw opcode value</param>
        /// <returns></returns>
        private static (OpCode opCode, ParameterMode modeA, ParameterMode modeB, ParameterMode modeC) ParseOpcode(long rawOpCode)
        {
            long opCode = rawOpCode % 100;
            string s = rawOpCode.ToString().PadLeft(5, '0');

            return ((OpCode)opCode,
                    (ParameterMode)Enum.Parse(typeof(ParameterMode), s[2].ToString()),
                    (ParameterMode)Enum.Parse(typeof(ParameterMode), s[1].ToString()),
                    (ParameterMode)Enum.Parse(typeof(ParameterMode), s[0].ToString()));
        }
    }
}
