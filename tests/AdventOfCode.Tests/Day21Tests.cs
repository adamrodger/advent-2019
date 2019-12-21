using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day21Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day21 solver;

        public Day21Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day21();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day21.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 19354928;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 21 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 1141997803;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 21 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
