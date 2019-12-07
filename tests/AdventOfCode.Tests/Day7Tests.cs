using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day7Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day7 solver;

        public Day7Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day7();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day7.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 272368;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 7 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 19741286;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 7 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
