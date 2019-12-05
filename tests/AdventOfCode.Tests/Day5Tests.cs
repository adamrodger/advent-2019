using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day5Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day5 solver;

        public Day5Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day5();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day5.txt");
            return input;
        }

        [Theory]
        [InlineData("3,0,4,0,99", 1)]
        [InlineData("1002,6,3,6,4,6,33", 99)]
        public void Part1_SampleInput_ProducesCorrectResponse(string input, int expected)
        {
            var result = solver.Part1(new [] { input });

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 7988899;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 5 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 0)] // position mode, input == 8?
        [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 1)] // position mode, input < 8?
        [InlineData("3,3,1108,-1,8,3,4,3,99", 0)] // immediate mode, input == 8?
        [InlineData("3,3,1107,-1,8,3,4,3,99", 1)] // immediate mode, input < 8?
        public void Part2_SampleInput_ProducesCorrectResponse(string input, int expected)
        {
            var result = solver.Part2(new[] { input });

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 13758663;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 5 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
