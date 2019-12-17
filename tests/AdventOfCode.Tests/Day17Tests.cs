using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day17Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day17 solver;

        public Day17Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day17();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day17.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 1544;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 17 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 17 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
