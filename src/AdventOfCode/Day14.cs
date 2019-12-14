using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            got["ORE"] = 0;
            needed["ORE"] = 0;
            
            needed["FUEL"] = requiredFuel;

            // recursively work backwards from FUEL, adding required amounts of input for each output
            React("FUEL", reactions, got, needed);
            
            return needed["ORE"];

            // guessed 174729 -- too low
        }

        public void React(string chemical, Dictionary<string, Reaction> reactions, Dictionary<string, long> got, Dictionary<string, long> needed)
        {
            if (!reactions.ContainsKey(chemical))
            {
                // input is a raw material - no reaction required
                return;
            }

            Reaction reaction = reactions[chemical];

            // can't react fractions so round up to next whole number
            double fractionalMultiplier = (double)(needed[chemical] - got[chemical]) / reaction.Output.quantity;
            long multiplier = (long)Math.Ceiling(fractionalMultiplier);

            // react up some more of it
            got[chemical] += reaction.Output.quantity * multiplier;

            foreach ((int quantity, string chemical) input in reaction.Inputs)
            {
                needed[input.chemical] += input.quantity * multiplier;

                // follow the reaction backwards
                React(input.chemical, reactions, got, needed);
            }
        }

        public long Part2(string[] input)
        {
            const long MAX_ORE = 1000000000000;

            // establish upper bound
            long max = 1;
            long required;
            while ((required = Part1(input, max)) < MAX_ORE)
            {
                max *= 2;
                Debug.WriteLine($"{max} - {required}");
            }

            // find lower bound
            long min = 0;
            while (min < max - 1)
            {
                long mid = (min + max) / 2;
                required = Part1(input, mid);

                Debug.WriteLine($"{mid} - {required}");

                if (required < MAX_ORE)
                {
                    min = mid;
                }
                else if (required > MAX_ORE)
                {
                    max = mid;
                }
            }

            return min;

            // guessed 2690377 -- too low
            //    - Required ore for 2_690_377 fuel is actually 239_527_482 so can't just do 1_000_000_000_000 / orePerFuel
            // guessed 82892753 -- too high -- I was running the sample input!! big lolz!!!
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
