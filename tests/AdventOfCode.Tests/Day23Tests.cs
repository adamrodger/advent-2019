using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day23Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day23 solver;

        public Day23Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day23();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day23.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 15416;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 23 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 23 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
