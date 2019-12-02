using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day2Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day2 solver;

        public Day2Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day2();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day2.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 6627023;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 2 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 4019;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 2 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
