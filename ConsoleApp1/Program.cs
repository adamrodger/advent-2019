using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AdventOfCode.IntCode;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(args[0]);
            var vm = new IntCodeEmulator(input);

            while (true)
            {
                vm.ExecuteUntilYield();

                while (vm.StdOut.Any())
                {
                    Console.Write((char)vm.StdOut.Dequeue());
                }

                string line = Console.ReadLine();
                foreach (char c in line)
                {
                    vm.StdIn.Enqueue(c);
                }

                vm.StdIn.Enqueue(10);
            }
        }
    }
}
