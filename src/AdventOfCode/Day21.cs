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
             if (!(A || B || C) && D) { jump }
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
            var vm = new IntCodeEmulator(input);

            /*
             Part 1 program fails on this:

             ABCDEFGHI
              ABCDEFGHI
               ABCDEFGHI
                ABCDEFGHI <<< important one - D is safe but E isn't so don't jump
                 ABCDEFGHI
                  ABCDEFGHI <<< do jump here to land on the second ledge, then jump again straight away (i.e. D and H must both be safe)
             .................
             .................
             @................
             #####.#.#...#####
    jumps here ^   ^ lands here then walks off the edge because D isn't safe

             need to jump onto I, then program from part 1 will work
             need to jump at E to land on I
             look for !A && B && !C == !(A || C) && B

            fails on most basic test :D
            .................
            .................
            @................
            #####.###########

            first (incorrect) jump will skip if E is false
            D and H must both be safe

            (D && H) fails - walks straight off the first ledge because H isn't safe. part 1 should pass this:

            .................
            .................
            @................
            #####...####..###

            D && H is just running part 1 twice, so stop the first incorrect jump happening with (part 1) && E - i.e. can walk afterwards
            && E || H == can walk after jumping or can jump twice

            logic:
                - hole in front of me - !(A || B || C)
                - safe to jump        - D
                - can walk after      - E
                - can double jump     - H

            So: !(A || B || C) && D && (E || H)
             */

            var program = new[]
            {
                "OR A J",     // J = A
                "AND B J",    // J = J || B
                "AND C J",    // J = J || C
                "NOT J J",    // J = !(A || B || C)
                "AND D J",    // J = D && !(A || B || C)

                /*
                "OR A T",  // T = A
                "OR C T",  // T = A || C
                "NOT T T", // T = !(A || C)
                "AND B T", // T = !(A || C) && B
                */

                /*
                "OR D T",     // T = D
                "AND H T",     // T = D && H -- safe to jump once or twice
                */

                "OR E T",     // T = E
                "OR H T",     // T = E || H

                "AND T J"     // J = (D && !(A || B || C)) && (E || H)
            };

            program.ForEach(p => InputInstruction(vm, p));
            InputInstruction(vm, "RUN");
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
