using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Xml.XPath;

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

            var nums = new int[places.Length];
            int numsCnt = 0;

            int instrIdx = 0;
            int cnt = 0;
            while (numsCnt != places.Length)
            {
                var instr = instructions[instrIdx];

                cnt++;
                for (int i = 0; i < places.Length; i++)
                {
                    var next = dct[places[i]];
                    if (instr == 'L')
                        places[i] = next.Item1;
                    else
                        places[i] = next.Item2;

                    if (places[i][2] == 'Z')
                    {
                        if (nums[i] == 0)
                        {
                            nums[i] = cnt;
                            numsCnt++;
                        }
                    }
                }

                instrIdx++;
                if (instrIdx == instructions.Length)
                    instrIdx = 0;

            }

            // 23977527174353 = LCM(18727, 24253, 18113, 22411, 21797, 16271)
            long lcm = lcm_of_array_elements(nums);

            Console.WriteLine($"Result: {lcm}");

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

        public static long lcm_of_array_elements(int[] element_array)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {

                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {

                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array[i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array[i] < 0)
                    {
                        element_array[i] = element_array[i] * (-1);
                    }
                    if (element_array[i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by devisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] = element_array[i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate 
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }

    }
}