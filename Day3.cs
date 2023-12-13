using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace AdventOfCode2023
{
    internal class Day3
    {
        public Day3()
        {
            int day = 3;
            var input = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";


            input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();

            //var sum = part1(lines);
            var sum = part2(lines);
            Console.WriteLine($"Vysledok: {sum}");
        }

        private int part2(string[] lines)
        {
            int sum = 0;

            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var ch = line[x];
                    if (ch != '*') continue;

                    var hs = new HashSet<int>();
                    for (int y0 = y - 1; y0 <= y + 1; y0++)
                    {
                        for (int x0 = x - 1; x0 <= x + 1; x0++)
                        {
                            if (x0 == x && y0 == y) continue;
                            var num = getNumAround(x0, y0, lines);
                            if (num > 0)
                            {
                                hs.Add(num);
                                if (hs.Count > 2)
                                    break;
                            }
                        }
                        if (hs.Count > 2)
                            break;
                    }
                    if (hs.Count == 2)
                    {
                        sum += (hs.First() * hs.Last());
                    }
                    else
                    {
                    }
                }
            }


            return sum;
        }

        private int getNumAround(int x, int y, string[] lines)
        {
            if (x < 0 || y < 0 || x >= lines[0].Length || y >= lines.Length) return 0;
            var line = lines[y];
            if (char.IsDigit(line[x]))
            {
                int start = x, end = x;
                int len = 1;
                // zistit zaciatok
                while (start > 0 && char.IsDigit(line[start - 1]))
                {
                    start--;
                    len++;
                }

                while (end < line.Length - 1 && char.IsDigit(line[end + 1]))
                {
                    end++;
                    len++;
                }
                var str = line.Substring(start, end - start + 1);
                return int.Parse(str);
            }
            return 0;
        }

        private int part1(string[] lines)
        {
            int sum = 0;

            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var ch = line[x];
                    if (ch == '.') continue;
                    if (char.IsDigit(ch))
                    {
                        int start = x, len = 1;
                        while (x < line.Length - 1 && char.IsDigit(line[x + 1]))
                        {
                            x++;
                            len++;
                        }
                        if (hasAdjSymbol(lines, start, y, len))
                        {
                            var partNum = int.Parse(line.Substring(start, len));
                            sum += partNum;
                            Console.WriteLine(partNum);
                        }
                    }
                }
            }

            return sum;
        }

        private bool hasAdjSymbol(string[] lines, int x, int y, int len)
        {
            var line = lines[y];
            // left
            if (isSym(line, x - 1)) return true; // 

            // right
            if (isSym(line, x + len)) return true; // 


            string lineUp = y > 0 ? lines[y - 1] : null;
            string lineDn = y < lines.Length - 1 ? lines[y + 1] : null;
            for (int x0 = x - 1; x0 <= x + len; x0++)
            {
                if (isSym(lineUp, x0)) return true;
                if (isSym(lineDn, x0)) return true;
            }

            return false;
        }

        private bool isSym(string line, int pos)
        {
            if (line == null) return false;
            if (pos < 0) return false;
            if (pos > line.Length - 1) return false;
            return line[pos] != '.';
        }
    }
}