using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day14Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day14 solver;

        public Day14Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day14();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day14.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "157 ORE => 5 NZVS",
                "165 ORE => 6 DCFZ",
                "44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL",
                "12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ",
                "179 ORE => 7 PSHF",
                "177 ORE => 5 HKGWZ",
                "7 DCFZ, 7 PSHF => 2 XJWVT",
                "165 ORE => 2 GPVTF",
                "3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT",
            };
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 371695;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 14 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 82892753;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 4052920;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 14 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
