using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode2023
{
    internal class Day1
    {
        public Day1()
        {
            var input = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";


            input = Helper.GetWebInput(1);

            //int sum = part1(input);
            int sum = part2(input);
            Console.WriteLine($"Vysledok: {sum}");

        }

        private int part2(string input)
        {
            var validNumbers = new string[] { "one", "two", "three", "four", "five",
                "six", "seven", "eight", "nine",
                "1", "2", "3", "4", "5", "6", "7", "8", "9"  };

            int sum = 0;
            var lines = Helper.GetLines(input);
            var lineId = 1;
            foreach (var item in lines)
            {
                if (lineId == 966)
                {
                }

                int minPos = int.MaxValue, minValue = 0;
                int maxPos = int.MinValue, maxValue = 0;

                foreach (var num in validNumbers)
                {
                    var pos = item.IndexOf(num);
                    if (pos == -1) continue;
                    if (pos < minPos)
                    {
                        minPos = pos;
                        minValue = num.Length == 1 ? int.Parse(num) : Array.IndexOf(validNumbers, num) + 1;
                    }

                    pos = item.LastIndexOf(num);
                    if (pos > maxPos)
                    {
                        maxPos = pos;
                        maxValue = num.Length == 1 ? int.Parse(num) : Array.IndexOf(validNumbers, num) + 1;
                    }
                }

                if (minValue <= 0 || maxValue <= 0)
                {
                }

                sum += int.Parse($"{minValue}{maxValue}");
                Console.WriteLine($"{lineId,4}: {minValue}, {maxValue}, {item}");
                lineId++;
            }
            return sum;
        }

        private static int part1(string input)
        {
            var sum = 0;
            foreach (var item in Helper.GetLines(input))
            {
                int idx1 = 0, idx2 = item.Length - 1;
                while (!char.IsDigit(item[idx1])) idx1++;
                while (!char.IsDigit(item[idx2])) idx2--;

                sum += int.Parse($"{item[idx1]}{item[idx2]}");
            }

            return sum;
        }
    }
}