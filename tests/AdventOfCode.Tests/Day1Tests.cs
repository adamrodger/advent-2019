using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day1Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day1 solver;

        public Day1Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day1();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day1.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 3305301;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 1 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 4955106;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 1 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}