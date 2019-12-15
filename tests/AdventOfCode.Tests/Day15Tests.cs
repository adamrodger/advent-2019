using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day15Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day15 solver;

        public Day15Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day15();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day15.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 318;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 15 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 390;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 15 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
