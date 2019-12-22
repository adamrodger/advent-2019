using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using AdventOfCode.Utilities;
using MoreLinq;
using Nito.Collections;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 22
    /// </summary>
    public class Day22
    {
        public int Part1(string[] input, int N = 10007)
        {
            var deck = new Deque<int>(Enumerable.Range(0, N));

            foreach (string instruction in input)
            {
                if (instruction == "deal into new stack")
                {
                    deck = deck.Reverse().ToDeque();
                }
                else if (instruction.StartsWith("cut"))
                {
                    int n = instruction.Numbers<int>().First();

                    if (n < 0)
                    {
                        Enumerable.Range(0, n * -1).ForEach(i => deck.AddToFront(deck.RemoveFromBack()));
                    }
                    else
                    {
                        Enumerable.Range(0, n).ForEach(i => deck.AddToBack(deck.RemoveFromFront()));
                    }
                }
                else if (instruction.StartsWith("deal with increment"))
                {
                    int n = instruction.Numbers<int>().First();

                    int count = deck.Count;
                    var newDeck = new int[count];

                    for (int i = 0; i < count; i++)
                    {
                        newDeck[(n * i) % count] = deck.RemoveFromFront();
                    }

                    deck = newDeck.ToDeque();
                }
            }

            return deck.IndexOf(2019);

            // 9525 -- too high
            // 9403 -- too high
            // 7753 -- too high
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

    public static class DequeExtensions
    {
        public static Deque<T> ToDeque<T>(this IEnumerable<T> @this)
        {
            return new Deque<T>(@this);
        }
    }
}
