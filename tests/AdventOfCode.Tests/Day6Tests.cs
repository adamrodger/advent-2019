using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day6Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day6 solver;

        public Day6Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day6();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day6.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "COM)B",
                "B)C",
                "C)D",
                "D)E",
                "E)F",
                "B)G",
                "G)H",
                "D)I",
                "E)J",
                "J)K",
                "K)L",
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 42;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 254447;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 6 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 445;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 6 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
