using System;

namespace AdventOfCode.IntCode
{
    /// <summary>
    /// IntCode instruction
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// OpCode of the instruction
        /// </summary>
        public int OpCode { get; }

        /// <summary>
        /// Number of arguments for the instruction
        /// </summary>
        public int Args { get; }

        /// <summary>
        /// Instruction action of (program, a, b, c), where some or all of the 3 args may be ignored
        /// </summary>
        public Action<int[], int, int, int> Action { get; }

        public Instruction(int opCode, int args, Action<int[], int, int, int> action)
        {
            this.OpCode = opCode;
            this.Args = args;
            this.Action = action;
        }
    }
}
