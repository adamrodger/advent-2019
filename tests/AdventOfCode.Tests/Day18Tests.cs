using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day18Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day18Attempt2 solver;

        public Day18Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day18Attempt2();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day18.txt");
            return input;
        }

        public static IEnumerable<object[]> GetSampleInput()
        {
            yield return new object[] {
            new[] {
                "#################",
                "#i.G..c...e..H.p#",
                "########.########",
                "#j.A..b...f..D.o#",
                "########@########",
                "#k.E..a...g..B.n#",
                "########.########",
                "#l.F..d...h..C.m#",
                "#################",
            }, 136 };

            yield return new object[]
            {
            new[] {
"########################",
"#@..............ac.GI.b#",
"###d#e#f################",
"###A#B#C################",
"###g#h#i################",
"########################",
            }, 81};

            yield return new object[]
            {
                new[] {
"########################",
"#f.D.E.e.C.b.A.@.a.B.c.#",
"######################.#",
"#d.....................#",
"########################",
            }, 86};

            yield return new object[]
            {
                new[] {
"########################",
"#...............b.C.D.f#",
"#.######################",
"#.....@.a.B.c.d.A.e.F.g#",
"########################",
            }, 132};
        }

        [Theory]
        [MemberData(nameof(GetSampleInput))]
        public void Part1_SampleInput_ProducesCorrectResponse(string[] input, int expected)
        {
            var result = solver.Part1(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 5068;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 18 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        /*[Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }*/

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 18 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}
