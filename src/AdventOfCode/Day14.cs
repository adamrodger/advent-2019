using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 14
    /// </summary>
    public class Day14
    {
        public long Part1(string[] input, long requiredFuel = 1)
        {
            var reactions = new Dictionary<string, Reaction>(input.Length);
            var got = new Dictionary<string, long>(reactions.Count);
            var needed = new Dictionary<string, long>(reactions.Count);

            // parse reactions
            foreach (string line in input)
            {
                MatchCollection matches = Regex.Matches(line, @"(\d+) ([A-Z]+)");
                (int quantity, string chemical)[] parsed = matches.Cast<Match>().Select(m => (int.Parse(m.Groups[1].Value), m.Groups[2].Value)).ToArray();

                (int quantity, string chemical) output = parsed.Last();

                reactions[output.chemical] = new Reaction
                {
                    Inputs = parsed.Take(parsed.Length - 1).ToArray(),
                    Output = output
                };

                needed[output.chemical] = 0;
                got[output.chemical] = 0;
            }

            needed["ORE"] = 0;
            needed["FUEL"] = requiredFuel;

            React("FUEL", reactions, got, needed);
            
            return needed["ORE"];
        }

        public long Part2(string[] input)
        {
            const long target = 1000000000000;

            // establish rough upper bound as power of 2
            long max = 1;
            while (this.Part1(input, max) < target)
            {
                max *= 2;
            }

            // converge on correct answer using binary chop
            long min = 0;
            while (min < max - 1)
            {
                long mid = (min + max) / 2;
                long required = this.Part1(input, mid);

                if (required < target)
                {
                    // too low, chop to the higher half
                    min = mid;
                }
                else if (required > target)
                {
                    // too high, chop to the lower half
                    max = mid;
                }
            }

            return min;
        }

        /// <summary>
        /// Work out how much of each chemical is needed to produce the needed amount of fuel
        /// </summary>
        /// <param name="chemical">Current chemical to produce (if necessary)</param>
        /// <param name="reactions">Reaction formulae</param>
        /// <param name="got">Stock if chemicals already produced</param>
        /// <param name="needed">Amount of each chemical yet to be produced</param>
        private static void React(string chemical, Dictionary<string, Reaction> reactions, Dictionary<string, long> got, Dictionary<string, long> needed)
        {
            if (!reactions.ContainsKey(chemical))
            {
                // input is a raw material - no reaction required
                return;
            }

            Reaction reaction = reactions[chemical];

            // can't react fractional amounts so round up to next whole number
            double fractionalMultiplier = (double)(needed[chemical] - got[chemical]) / reaction.Output.quantity;
            long multiplier = (long)Math.Ceiling(fractionalMultiplier);

            // produce some more of it using the reaction formula in a depth-first recursive search
            foreach ((int quantity, string chemical) input in reaction.Inputs)
            {
                needed[input.chemical] += input.quantity * multiplier;

                // follow the reaction backwards to create the needed inputs until we hit raw material
                React(input.chemical, reactions, got, needed);
            }

            got[chemical] += reaction.Output.quantity * multiplier;
        }
    }

    public class Reaction
    {
        public ICollection<(int quantity, string chemical)> Inputs { get; set; }
        public (int quantity, string chemical) Output { get; set; }

        public override string ToString()
        {
            return $"{string.Join(", ", Inputs.Select(i => i.quantity + " " + i.chemical))} => {this.Output.quantity} {this.Output.chemical}";
        }
    }
}
