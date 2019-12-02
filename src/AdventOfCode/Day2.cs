using System;
using AdventOfCode.IntCode;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 2
    /// </summary>
    public class Day2
    {
        public int Part1(string[] input)
        {
            var emulator = new IntCodeEmulator(input)
            {
                Noun = 12,
                Verb = 2
            };

            emulator.Execute();

            return emulator.Result;
        }

        public int Part2(string[] input)
        {
            const int target = 19690720;

            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    var emulator = new IntCodeEmulator(input)
                    {
                        Noun = noun,
                        Verb = verb
                    };

                    emulator.Execute();

                    if (emulator.Result == target)
                    {
                        return noun * 100 + verb;
                    }
                }
            }

            throw new Exception("not found");
        }
    }
}
