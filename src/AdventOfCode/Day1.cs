using System;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 1
    /// </summary>
    public class Day1
    {
        public int Part1(string[] input)
        {
            return input.Select(int.Parse).Select(i => (i / 3) - 2).Sum();
        }

        public int Part2(string[] input)
        {
            int total = 0;

            foreach (int module in input.Select(int.Parse))
            {
                int fuel = (module / 3) - 2;

                while (fuel > 0)
                {
                    total += fuel;
                    fuel = (fuel / 3) - 2;
                }
            }

            return total;
        }
    }
}
