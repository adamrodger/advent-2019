namespace AdventOfCode.IntCode
{
    /// <summary>
    /// Parameter mode
    /// </summary>
    public enum ParameterMode
    {
        /// <summary>
        /// The parameter represents a pointer to a memory address
        /// </summary>
        Position = 0,

        /// <summary>
        /// The parameter represents a literal value
        /// </summary>
        Immediate = 1
    }
}
