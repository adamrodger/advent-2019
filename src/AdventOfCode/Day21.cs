using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
            const int maxInstructions = 15;

            var vm = new IntCodeEmulator(input);

            // AND X Y      Y = X && Y
            // OR X Y       Y = X || Y
            // NOT X Y      Y = !X

            // T == jump
            // J == jump

            // A,B,C,D - true = ground, false = hole

            /*
             if (A || B)
             {
                 jump - (jump goes 4 squares)
             }
             -- gets past stage one which is ####  # ######
             -- fails on stage 2             ####   #######

             -- Look if there's a hole somewhere in front of us !(A || B || C) but land 4 ahead (D is true)
             if (!(A || B || C) && D { jump }
            */


            var program = new[]
            {
                "OR A J",     // J = A
                "AND B J",    // J = J || B
                "AND C J",    // J = J || C
                "NOT J J",    // J = !(A || B || C)
                "AND D J"     // J = D && !(A || B || C)
            };

            program.ForEach(p => InputInstruction(vm, p));
            InputInstruction(vm, "WALK");
            vm.Execute();

            while (vm.StdOut.Any())
            {
                if (vm.StdOut.Peek() > 255)
                {
                    return (int)vm.StdOut.Dequeue();
                }

                Debug.Write((char)vm.StdOut.Dequeue());
            }

            return int.MinValue;
        }


        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
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
