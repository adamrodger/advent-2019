namespace AdventOfCode.IntCode
{
    /// <summary>
    /// Operation codes
    /// </summary>
    public class OpCodes
    {
        /// <summary>
        /// Stop
        /// </summary>
        public const int Halt = 99;

        /// <summary>
        /// c = a + b
        /// </summary>
        public const int Add = 1;

        /// <summary>
        /// c = a * b
        /// </summary>
        public const int Multiply = 2;

        /// <summary>
        /// a = (read stdin)
        /// </summary>
        public const int Input = 3;

        /// <summary>
        /// echo a
        /// </summary>
        public const int Output = 4;

        /// <summary>
        /// pointer = a != 0 ? b : pointer
        /// </summary>
        public const int JumpNZ = 5;

        /// <summary>
        /// pointer = a == 0 ? b : pointer
        /// </summary>
        public const int JumpZ = 6;

        /// <summary>
        /// c = a &lt; b ? 1 : 0
        /// </summary>
        public const int Lt = 7;

        /// <summary>
        /// c = a == b ? 1 : 0
        /// </summary>
        public const int Eq = 8;
    }
}
