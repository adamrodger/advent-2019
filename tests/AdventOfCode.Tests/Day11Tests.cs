using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day11Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day11 solver;

        public Day11Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day11();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day11.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 1907;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 11 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = string.Join(Environment.NewLine,
                                       "  ██  ███  ████ █  █ ████  ██  ████  ██   ",
                                       " █  █ █  █ █    █ █     █ █  █ █    █  █  ",
                                       " █  █ ███  ███  ██     █  █    ███  █     ",
                                       " ████ █  █ █    █ █   █   █ ██ █    █ ██  ",
                                       " █  █ █  █ █    █ █  █    █  █ █    █  █  ",
                                       " █  █ ███  ████ █  █ ████  ███ █     ███  ",
                                       Environment.NewLine);

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 11 - Part 2 - \n{result}");

            Assert.Equal(expected, result);
        }
    }
}
