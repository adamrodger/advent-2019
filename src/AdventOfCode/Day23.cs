using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using AdventOfCode.IntCode;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 23
    /// </summary>
    public class Day23
    {
        public long Part1(string[] input)
        {
            var vms = new Dictionary<long, IntCodeEmulator>(50);
            var mailboxes = new Dictionary<long, Queue<long>>(50);

            for (int id = 0; id < 50; id++)
            {
                var vm = new IntCodeEmulator(input);
                vm.StdIn.Enqueue(id);
                vm.ExecuteUntilYield();

                mailboxes[id] = new Queue<long>();

                vms[id] = vm;
            }

            while (true)
            {
                for (int id = 0; id < 50; id++)
                {
                    var vm = vms[id];
                    var mailbox = mailboxes[id];

                    if (!mailbox.Any())
                    {
                        mailbox.Enqueue(-1);
                    }

                    while (mailbox.Any())
                    {
                        vm.StdIn.Enqueue(mailbox.Dequeue());
                    }

                    vm.ExecuteUntilYield();

                    while (vm.StdOut.Any())
                    {
                        var (dest, x, y) = (vm.StdOut.Dequeue(), vm.StdOut.Dequeue(), vm.StdOut.Dequeue());

                        if (dest == 255)
                        {
                            return y;
                        }

                        mailboxes[dest].Enqueue(x);
                        mailboxes[dest].Enqueue(y);
                        
                    }
                }
            }
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
