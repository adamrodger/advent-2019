namespace AdventOfCode.IntCode
{
    /// <summary>
    /// Operation codes
    /// </summary>
    public enum OpCode
    {
        /// <summary>
        /// Stop
        /// </summary>
        Halt = 99,

        /// <summary>
        /// c = a + b
        /// </summary>
        Add = 1,

        /// <summary>
        /// c = a * b
        /// </summary>
        Multiply = 2,

        /// <summary>
        /// a = (read stdin)
        /// </summary>
        Input = 3,

        /// <summary>
        /// echo a
        /// </summary>
        Output = 4,

        /// <summary>
        /// pointer = a != 0 ? b : pointer
        /// </summary>
        JumpNZ = 5,

        /// <summary>
        /// pointer = a == 0 ? b : pointer
        /// </summary>
        JumpZ = 6,

        /// <summary>
        /// c = a &lt, b ? 1 : 0
        /// </summary>
        LessThan = 7,

        /// <summary>
        /// c = a == b ? 1 : 0
        /// </summary>
        Equal = 8,

        /// <summary>
        /// relative base += a
        /// </summary>
        Relative = 9,
    }
}
