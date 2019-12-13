using System.IO;
using AdventOfCode;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main()
        {
            string[] input = File.ReadAllLines("inputs/day13.txt");
            Day13 solver = new Day13();
            solver.Part2(input);
        }
    }
}
