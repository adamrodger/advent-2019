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
        public OpCode OpCode { get; }

        /// <summary>
        /// Number of arguments for the instruction
        /// </summary>
        public int Args { get; }

        /// <summary>
        /// Instruction action of (a, b, c), where some or all of the 3 args may be ignored
        /// </summary>
        public Action<int, int, int> Action { get; }

        public Instruction(OpCode opCode, int args, Action<int, int, int> action)
        {
            this.OpCode = opCode;
            this.Args = args;
            this.Action = action;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(this.OpCode)}: {this.OpCode}, {nameof(this.Args)}: {this.Args}";
        }
    }
}
