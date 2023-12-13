using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023
{
    internal class Day2
    {
        public Day2()
        {
            int day = 2;
            var input = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";


            input = Helper.GetWebInput(day);

            int sum = 0;
            var lines = Helper.GetLines(input);
            var lineId = 1;

            //part1(ref sum, lines, ref lineId);
            part2(ref sum, lines, ref lineId);
            Console.WriteLine($"Vysledok: {sum}");

        }


        private static void part2(ref int sum, IEnumerable<string> lines, ref int lineId)
        {
            var maxes = new Dictionary<string, int>();
            maxes.Add("red", 0);
            maxes.Add("green", 0);
            maxes.Add("blue", 0);

            foreach (var item in lines)
            {
                maxes["red"] = 0;
                maxes["green"] = 0;
                maxes["blue"] = 0;

                var parts = item.Split(':', ';');
                var gameId = int.Parse(parts[0].Split(' ')[1]);

                var sets = parts.Skip(1).ToArray();

                foreach (var set in sets)
                {
                    var sparts = set.Split(',');
                    foreach (var spart in sparts)
                    {
                        var oneparts = spart.Trim().Split(' ');
                        var cnt = int.Parse(oneparts[0]);
                        var color = oneparts[1];
                        if (cnt > maxes[color])
                        {
                            maxes[color] = cnt;
                        }
                    }

                }
                sum += maxes["red"] * maxes["green"] * maxes["blue"];
                //Console.WriteLine(gameId);

                lineId++;
            }
        }

        private static void part1(ref int sum, IEnumerable<string> lines, ref int lineId)
        {
            var maxes = new Dictionary<string, int>();
            maxes.Add("red", 12);
            maxes.Add("green", 13);
            maxes.Add("blue", 14);

            foreach (var item in lines)
            {
                if (lineId == 966)
                {
                }

                var parts = item.Split(':', ';');
                var gameId = int.Parse(parts[0].Split(' ')[1]);

                var sets = parts.Skip(1).ToArray();

                bool ok = true;
                foreach (var set in sets)
                {
                    var sparts = set.Split(',');
                    foreach (var spart in sparts)
                    {
                        var oneparts = spart.Trim().Split(' ');
                        var cnt = int.Parse(oneparts[0]);
                        var color = oneparts[1];
                        if (cnt > maxes[color])
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (!ok)
                        break;

                }
                if (ok)
                {
                    sum += gameId;
                    //Console.WriteLine(gameId);
                }

                lineId++;
            }
        }
    }
}