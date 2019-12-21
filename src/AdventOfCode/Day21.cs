using System;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.IntCode;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 21
    /// </summary>
    public class Day21
    {
        public int Part1(string[] input)
        {
            var program = new[]
            {
                "OR A J",     // J = A
                "AND B J",    // J = J || B
                "AND C J",    // J = J || C
                "NOT J J",    // J = !(A || B || C)
                "AND D J",    // J = D && !(A || B || C)
                "WALK"
            };

            return RunSpringScript(input, program);
        }


        public int Part2(string[] input)
        {
            var program = new[]
            {
                "OR A J",     // J = A
                "AND B J",    // J = J || B
                "AND C J",    // J = J || C
                "NOT J J",    // J = !(A || B || C)
                "AND D J",    // J = D && !(A || B || C)

                "OR E T",     // T = E
                "OR H T",     // T = E || H

                "AND T J",    // J = (D && !(A || B || C)) && (E || H)
                "RUN"
            };

            return RunSpringScript(input, program);
        }

        private static int RunSpringScript(string[] input, string[] program)
        {
            var vm = new IntCodeEmulator(input);
            program.ForEach(p => InputInstruction(vm, p));
            vm.Execute();

            while (vm.StdOut.Any())
            {
                long output = vm.StdOut.Dequeue();

                if (output > 255)
                {
                    return (int) output;
                }

                Debug.Write((char) output);
            }

            return int.MinValue;
        }

        private static void InputInstruction(IntCodeEmulator vm, string instruction)
        {
            foreach (char c in instruction)
            {
                vm.StdIn.Enqueue(c);
            }

            vm.StdIn.Enqueue('\n');
        }
    }
}
