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
        public int Part1(string[] input, int N = 10007)
        {
            /*var deck = new Deque<int>(Enumerable.Range(0, N));

            foreach (string instruction in input)
            {
                var newDeck = new int[N];

                if (instruction == "deal into new stack")
                {
                    for (int i = 0; i < N; i++)
                    {
                        newDeck[i] = deck[N - i - 1];
                    }
                }
                else if (instruction.StartsWith("cut"))
                {
                    int n = instruction.Numbers<int>().First();

                    for (int i = 0; i < N; i++)
                    {
                        newDeck[i] = deck[(N + i - n) % N];
                    }
                }
                else if (instruction.StartsWith("deal with increment"))
                {
                    int n = instruction.Numbers<int>().First();

                    for (int i = 0; i < N; i++)
                    {
                        newDeck[(n * i) % N] = deck.RemoveFromFront();
                    }
                }

                deck = newDeck.ToDeque();
            }

            return deck.IndexOf(2019);*/

            // 9525 -- too high
            // 9403 -- too high
            // 7753 -- too high

            throw new NotImplementedException();
        }

        public BigInteger Part2(string[] input)
        {
            BigInteger d = 119315717514047; // deck size
            BigInteger s = 101741582076661; // number of shuffles

            BigInteger x = 2020;                        // the deck index we're tracking
            BigInteger y = ReverseShuffle(input, d, x); // that card's location one shuffle back in time
            BigInteger z = ReverseShuffle(input, d, y); // that card's location another shuffle back in time (i.e. 2 back from x)

            BigInteger a = (y - z) * ModInverse(x - y + d, d) % d;
            BigInteger b = (y - a * x) % d;

            var result = (BigInteger.ModPow(a, s, d) * x + (BigInteger.ModPow(a, s, d) - 1) * ModInverse(a - 1, d) * b) % d;
            return result;
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
