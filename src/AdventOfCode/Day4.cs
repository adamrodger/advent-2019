using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 4
    /// </summary>
    public class Day4
    {
        private static readonly Regex pairRegex = new Regex("(\\d)\\1+", RegexOptions.Compiled | RegexOptions.Singleline);

        public int Part1(string[] input)
        {
            return ValidPasswords(input).Count();
        }

        public int Part2(string[] input)
        {
            IEnumerable<string> matches = ValidPasswords(input);

            // contains at least one 'exact' double - i.e. 22 instead of 222
            return matches.Count(m => pairRegex.Matches(m).OfType<Match>().Any(g => g.Length == 2));
        }

        private static IEnumerable<string> ValidPasswords(string[] input)
        {
            int[] numbers = input[0].Split('-').Select(int.Parse).ToArray();

            var matches = Enumerable.Range(numbers[0], numbers[1] - numbers[0])
                                    .Select(i => i.ToString())
                                    .Where(s => s[0] <= s[1] && s[1] <= s[2] && s[2] <= s[3] && s[3] <= s[4] && s[4] <= s[5])
                                    .Where(s => pairRegex.IsMatch(s));
            return matches;
        }
    }
}
