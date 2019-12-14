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
        public int Part1(string[] input, int requiredFuel = 1)
        {
            var reactions = new Dictionary<string, Reaction>(input.Length);
            var got = new Dictionary<string, int>(reactions.Count);
            var needed = new Dictionary<string, int>(reactions.Count);

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

        public void React(string chemical, Dictionary<string, Reaction> reactions, Dictionary<string, int> got, Dictionary<string, int> needed)
        {
            if (!reactions.ContainsKey(chemical))
            {
                // input is a raw material - no reaction required
                return;
            }

            Reaction reaction = reactions[chemical];

            // can't react fractions so round up to next whole number
            double fractionalMultiplier = (double)(needed[chemical] - got[chemical]) / reaction.Output.quantity;
            int multiplier = (int)Math.Ceiling(fractionalMultiplier);

            // react up some more of it
            got[chemical] += reaction.Output.quantity * multiplier;

            foreach ((int quantity, string chemical) input in reaction.Inputs)
            {
                needed[input.chemical] += input.quantity * multiplier;

                // follow the reaction backwards
                React(input.chemical, reactions, got, needed);
            }
        }

        public int Part2(string[] input)
        {
            long maxOre = 1_000_000_000_000;
            int orePerFuel = Part1(input);
            int tryFuel = 1;

            // keep producing more and more fuel until we've consumed all the ore
            while(true)
            {
                int requiredOre = Part1(input, tryFuel);

                if (requiredOre > maxOre)
                {
                    return tryFuel;
                }

                tryFuel++;
            }

            return -1;

            // guessed 2690377 -- too low
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
