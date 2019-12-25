using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day25Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day25 solver;

        public Day25Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day25();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day25.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 67635328;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 25 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
