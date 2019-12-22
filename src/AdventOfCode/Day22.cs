using System;
using System.Linq;
using System.Numerics;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 22
    /// </summary>
    public class Day22
    {
        public int Part1(string[] input, int count = 10007)
        {
            int[] deck = Shuffle(input, count);

            return Array.IndexOf(deck, 2019);
        }

        public BigInteger Part2(string[] input)
        {
            BigInteger d = 119315717514047; // deck size
            BigInteger s = 101741582076661; // number of shuffles

            BigInteger x = 2020;                        // the deck index we're tracking
            BigInteger y = ReverseShuffle(input, d, x); // that card's location one shuffle back in time
            BigInteger z = ReverseShuffle(input, d, y); // that card's location another shuffle back in time (i.e. 2 back from x)

            // work out a and b in a*x+b, which is the simplified form of the reversing function equations
            BigInteger a = (y - z) * ModInverse(x - y + d, d) % d;
            BigInteger b = (y - a * x) % d;

            // apply a*x+b for the known a, b and x
            var result = (BigInteger.ModPow(a, s, d) * x + (BigInteger.ModPow(a, s, d) - 1) * ModInverse(a - 1, d) * b) % d;
            return result;
        }

        /// <summary>
        /// Shuffles a deck of the given size according to the given instructions
        /// </summary>
        /// <param name="input">Shuffling instructions</param>
        /// <param name="count">Deck size</param>
        /// <returns>Shuffled deck</returns>
        public static int[] Shuffle(string[] input, int count)
        {
            var deck = Enumerable.Range(0, count).ToArray();

            foreach (string instruction in input)
            {
                if (instruction == "deal into new stack")
                {
                    deck = deck.Reverse().ToArray();
                    continue;
                }

                var newDeck = new int[count];
                int n = instruction.Numbers<int>().First();

                if (instruction.StartsWith("cut"))
                {
                    for (int i = 0; i < count; i++)
                    {
                        newDeck[i] = deck[(count + i + n) % count];
                    }
                }
                else if (instruction.StartsWith("deal with increment"))
                {
                    for (int i = 0; i < count; i++)
                    {
                        newDeck[i] = deck[(i * n) % count];
                    }
                }

                deck = newDeck;
            }

            return deck;
        }

        /// <summary>
        /// Reverses the shuffle at index <paramref name="i"/>
        /// </summary>
        /// <param name="instructions">Instructions for shuffle (to run in reverse)</param>
        /// <param name="cards">Number of cards in the deck</param>
        /// <param name="i">Deck position</param>
        /// <returns>Starting position of the card currently at the given index (i.e. the shuffle has been reversed)</returns>
        private static BigInteger ReverseShuffle(string[] instructions, BigInteger cards, BigInteger i)
        {
            foreach (string instruction in instructions.Reverse())
            {
                if (instruction == "deal into new stack")
                {
                    i = cards - 1 - i;
                    continue;
                }

                int n = instruction.Numbers<int>().First();

                if (instruction.StartsWith("cut"))
                {
                    i += n;
                }
                else if (instruction.StartsWith("deal with increment"))
                {
                    i = ModInverse(n, cards) * i % cards;
                }
            }

            return i;
        }

        /// <summary>
        /// https://stackoverflow.com/a/15768873
        /// </summary>
        public static BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            return BigInteger.ModPow(a, n - 2, n);
        }
    }
}
