using System;
using System.IO;
using AdventOfCode.Utilities;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day8Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day8 solver;

        public Day8Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day8();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day8.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 1690;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 8 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            string expected = string.Join(Environment.NewLine,
                "BBBB BBB  BBBB B  B BBB  ",
                "   B B  B    B B  B B  B ",
                "  B  B  B   B  B  B BBB  ",
                " B   BBB   B   B  B B  B ",
                "B    B    B    B  B B  B ",
                "BBBB B    BBBB  BB  BBB  ",
                Environment.NewLine);

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 8 - Part 2 -\n{result}");

            Assert.Equal(expected, result);
        }
    }
}
