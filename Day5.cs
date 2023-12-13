using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023
{
    internal class Day5
    {
        char[] sepSpace = new char[] { ' ' };
        private ulong[] seeds;
        List<Tuple<ulong, ulong>> seeds2;

        //Dictionary<string, Ranges> dicts = new Dictionary<string, Ranges>();

        Dictionary<string, Ranges> dicts = new Dictionary<string, Ranges>();

        string[] transformPlan =
        {
            "seed-to-soil map:",
            "soil-to-fertilizer map:",
            "fertilizer-to-water map:",
            "water-to-light map:",
            "light-to-temperature map:",
            "temperature-to-humidity map:",
            "humidity-to-location map:"
        };

        public Day5()
        {

            int day = 5;
            var input = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";


            input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();


            //var min = part1(lines);
            var min = part2(lines);
            Console.WriteLine($"Vysledok: {min}");
        }

        private ulong part2(string[] lines)
        {

            dicts.Add("seed-to-soil map:", new Ranges());
            dicts.Add("soil-to-fertilizer map:", new Ranges());
            dicts.Add("fertilizer-to-water map:", new Ranges());
            dicts.Add("water-to-light map:", new Ranges());
            dicts.Add("light-to-temperature map:", new Ranges());
            dicts.Add("temperature-to-humidity map:", new Ranges());
            dicts.Add("humidity-to-location map:", new Ranges());

            Ranges aRange = null;
            seeds2 = new List<Tuple<ulong, ulong>>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("seeds:"))
                {
                    var s2 = line.Replace("seeds: ", "").Split(sepSpace, StringSplitOptions.RemoveEmptyEntries).Select(i => ulong.Parse(i)).ToArray();
                    var cnt = s2.Length / 2;
                    for (int i = 0; i < cnt; i++)
                    {
                        var from = i * 2;
                        var n1 = s2[from];
                        var n2 = n1 + s2[from + 1] - 1;
                        seeds2.Add(Tuple.Create(n1, n2));
                    }
                }
                else if (line.Contains("map:"))
                    aRange = dicts[line.Trim()];
                else
                {
                    // je to trojica cisel a ladujeme to do pola

                    var nums = line.Split(sepSpace, StringSplitOptions.RemoveEmptyEntries).Select(i => ulong.Parse(i)).ToArray();
                    aRange.Add(nums);

                }
            }

            var min = ulong.MaxValue;
            seeds2.AsParallel().ForAll(range =>
            {
                Console.WriteLine($"{range.Item1}-{range.Item2} ({range.Item2 - range.Item1})");
                for (ulong seed = range.Item1; seed <= range.Item2; seed++)
                {
                    var loc = getLocation(seed);
                    if (loc < min) min = loc;
                }
                Console.WriteLine($"DONE: {range.Item1}-{range.Item2} ({range.Item2 - range.Item1})");

            });


            return min;
        }

        private ulong part1(string[] lines)
        {

            dicts.Add("seed-to-soil map:", new Ranges());
            dicts.Add("soil-to-fertilizer map:", new Ranges());
            dicts.Add("fertilizer-to-water map:", new Ranges());
            dicts.Add("water-to-light map:", new Ranges());
            dicts.Add("light-to-temperature map:", new Ranges());
            dicts.Add("temperature-to-humidity map:", new Ranges());
            dicts.Add("humidity-to-location map:", new Ranges());

            Ranges aRange = null;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("seeds:"))
                    seeds = line.Replace("seeds: ", "").Split(sepSpace, StringSplitOptions.RemoveEmptyEntries).Select(i => ulong.Parse(i)).ToArray();
                else if (line.Contains("map:"))
                    aRange = dicts[line.Trim()];
                else
                {
                    // je to trojica cisel a ladujeme to do pola

                    var nums = line.Split(sepSpace, StringSplitOptions.RemoveEmptyEntries).Select(i => ulong.Parse(i)).ToArray();
                    aRange.Add(nums);

                }
            }

            var min = ulong.MaxValue;
            foreach (var seed in seeds)
            {
                var loc = getLocation(seed);
                //Console.WriteLine($"Seed: {seed}, Loc: {loc}");
                if (loc < min) min = loc;
            }


            return min;
        }

        private ulong getLocation(ulong seed)
        {
            ulong tranValue = seed;
            foreach (string arrName in transformPlan)
            {
                var arr = dicts[arrName];
                tranValue = arr.transform(tranValue, arrName);
            }
            return tranValue;
        }
    }

    internal class Ranges : List<Range>
    {
        internal void Add(ulong[] nums)
        {
            // 52 50 48
            /*
                The second line means that the 
                    source range starts at 50 and contains 48 values: 50, 51, ..., 96, 97. 
                This corresponds to a destination range starting at 52 and also 
                    containing 48 values: 52, 53, ..., 98, 99. So, seed number 53 corresponds to soil number 55.             
             */
            var rangeLength = nums[2];
            var range = new Range();
            range.SrcStart = nums[1];
            range.SrcEnd = range.SrcStart + rangeLength - 1;
            range.DstStart = nums[0];
            range.DstEnd = range.DstStart + rangeLength - 1;
            Add(range);
        }

        internal ulong transform(ulong tranValue, string arrName)
        {
            foreach (var item in this)
            {
                if (tranValue >= item.SrcStart && tranValue <= item.SrcEnd)
                {
                    var dx = tranValue - item.SrcStart;
                    tranValue = item.DstStart + dx;
                    return tranValue;
                }
            }
            return tranValue;
        }
    }

    [DebuggerDisplay("Src: {SrcStart}-{SrcEnd}, Dst: {DstStart}-{DstEnd}")]
    internal class Range
    {
        public ulong SrcStart { get; internal set; }
        public ulong SrcEnd { get; internal set; }
        public ulong DstStart { get; internal set; }
        public ulong DstEnd { get; internal set; }
    }
}