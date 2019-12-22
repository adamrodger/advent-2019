using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day22Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day22 solver;

        public Day22Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day22();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day22.txt");
            return input;
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 3036;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 22 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 70618172909245;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 22 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Shuffle_Reverse_ReversesDeck()
        {
            int[] expected = {9, 8, 7, 6, 5, 4, 3, 2, 1, 0};

            int[] actual = Day22.Shuffle(new[] { "deal into new stack" }, expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shuffle_DoubleReverse_LeavesDeckTheSame()
        {
            int[] expected = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

            int[] actual = Day22.Shuffle(new[] { "deal into new stack", "deal into new stack" }, expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shuffle_CutPositive_MovesCardsToTheBack()
        {
            int[] expected = {3, 4, 5, 6, 7, 8, 9, 0, 1, 2};

            int[] actual = Day22.Shuffle(new[] { "cut 3" }, expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shuffle_CutNegative_MovesCardsToTheFront()
        {
            int[] expected = {6, 7, 8, 9, 0, 1, 2, 3, 4, 5};

            int[] actual = Day22.Shuffle(new[] { "cut -4" }, expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shuffle_DoubleCut_LeavesDeckTheSame()
        {
            int[] expected = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

            int[] actual = Day22.Shuffle(new[] { "cut 3", "cut -3" }, expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shuffle_Increment_ShufflesDeckCorrectly()
        {
            int[] expected = {0, 7, 4, 1, 8, 5, 2, 9, 6, 3};

            int[] actual = Day22.Shuffle(new[] { "deal with increment 3" }, expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shuffle_AllOperations_ShufflesDeck()
        {
            int[] expected = {3, 0, 7, 4, 1, 8, 5, 2, 9, 6};

            int[] actual = Day22.Shuffle(new[]
                                         {
                                             "cut 6",
                                             "deal with increment 7",
                                             "deal into new stack"
                                         },
                                         expected.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shuffle_MultipleOperations_ShufflesDeck()
        {
            int[] expected = { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7 };

            int[] actual = Day22.Shuffle(new[]
                                         {
                                             "deal with increment 7",
                                             "deal into new stack",
                                             "deal into new stack"
                                         },
                                         expected.Length);

            Assert.Equal(expected, actual);
        }
    }
}
