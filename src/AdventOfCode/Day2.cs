using System;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 2
    /// </summary>
    public class Day2
    {
        public int Part1(string[] input)
        {
            return RunIntCodeProgram(input, 12, 2);
        }

        public int Part2(string[] input)
        {
            const int target = 19690720;

            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    int result = RunIntCodeProgram(input, noun, verb);

                    if (result == target)
                    {
                        return noun * 100 + verb;
                    }
                }
            }

            throw new Exception("not found");
        }

        private static int RunIntCodeProgram(string[] input, int noun, int verb)
        {
            int[] program = input.First().Split(',').Select(int.Parse).ToArray();

            program[1] = noun;
            program[2] = verb;

            int counter = 0;

            while (true)
            {
                int instruction = program[counter];

                if (instruction == 99)
                {
                    return program[0];
                }

                int a = program[counter + 1];
                int b = program[counter + 2];
                int c = program[counter + 3];

                switch (instruction)
                {
                    case 1:
                        program[c] = program[a] + program[b];
                        break;
                    case 2:
                        program[c] = program[a] * program[b];
                        break;
                }

                counter += 4;
            }
        }
    }
}
