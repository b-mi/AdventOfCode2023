﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace AdventOfCode2023
{
    internal class Day8
    {
        public Day8()
        {
            int day = 8;
            var input = @"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)";

            input = @"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)";


            input = @"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)";


            input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();
            //part1(lines);
            part2(lines);
        }

        private void part2(string[] lines)
        {
            var instructions = lines[0];
            var sep = new char[] { '=', ',' };
            var dct = new Dictionary<string, Tuple<string, string>>();
            var hsBad = new HashSet<string>();
            foreach (var line in lines.Skip(2))
            {
                //Console.WriteLine(line);
                var parts = line.Split(sep, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim().Replace("(", "").Replace(")", "")).ToArray();
                dct.Add(parts[0], Tuple.Create(parts[1], parts[2]));
            }

            Console.WriteLine("A");

            var places = dct.Where(kv => kv.Key[2] == 'A').Select(kv => kv.Key).ToArray();
            foreach (var item in places)
            {
                Console.WriteLine(item);
            }

            var placesZ = dct.Where(kv => kv.Key[2] == 'Z').Select(kv => kv.Key).ToArray();
            Console.WriteLine("Z");

            foreach (var item in placesZ)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("-");


            int instrIdx = 0;
            ulong cnt = 0;
            int okCount = 0;
            int maxOk = 0;
            while (okCount != places.Length - 1)
            {
                var instr = instructions[instrIdx];

                okCount = 0;
                cnt++;
                for (int i = 0; i < places.Length; i++)
                {
                    var next = dct[places[i]];
                    if (instr == 'L')
                        places[i] = next.Item1;
                    else
                        places[i] = next.Item2;


                    //Console.WriteLine($"{cnt}: {instr}, {places[i]}");

                    if (places[i][2] == 'Z')
                        okCount++;
                }

                if (okCount > maxOk)
                {
                    maxOk = okCount;
                    Console.WriteLine($"Max: {maxOk}");
                }

                instrIdx++;
                if (instrIdx == instructions.Length)
                    instrIdx = 0;

                if (cnt % 100_000_000 == 0)
                    Console.WriteLine(cnt);
            }

            Console.WriteLine($"Result: {cnt}");

        }


        private void part1(string[] lines)
        {
            var instructions = lines[0];
            var sep = new char[] { '=', ',' };
            var dct = new Dictionary<string, Tuple<string, string>>();
            foreach (var line in lines.Skip(2))
            {
                Console.WriteLine(line);
                var parts = line.Split(sep, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim().Replace("(", "").Replace(")", "")).ToArray();
                dct.Add(parts[0], Tuple.Create(parts[1], parts[2]));
            }

            var place = "AAA";
            int instrIdx = 0;
            ulong cnt = 0;
            while (place != "ZZZ")
            {
                var instr = instructions[instrIdx];

                var next = dct[place];
                cnt++;
                if (instr == 'L')
                    place = next.Item1;
                else
                    place = next.Item2;

                instrIdx++;
                if (instrIdx == instructions.Length)
                    instrIdx = 0;

                if (cnt % 1_000_000 == 0)
                    Console.WriteLine(cnt);
            }

            Console.WriteLine($"Result: {cnt}");

        }
    }
}