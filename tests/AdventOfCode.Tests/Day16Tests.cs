using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day16Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day16 solver;

        public Day16Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day16();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day16.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 68764632;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 16 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 52825021;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 16 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
