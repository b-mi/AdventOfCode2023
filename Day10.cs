using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace AdventOfCode2023
{
    internal class Day10
    {
        private int width;
        private int height;
        private int startY;
        private int startX;
        string[] map;
        bool[,] visited;

        public Day10()
        {
            int day = 10;
            var input = @"7-F7-
.FJ|7
SJLL7
|F--J
LJ.LJ";

            // input = @"10 13 16 21 30 45";


            // input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();
            part1(lines);
            //part2(lines);
        }

        private void part1(string[] lines)
        {
            this.width = lines[0].Length;
            this.height = lines.Length;
            this.visited = new bool[this.width, this.height];

            var lines2 = new List<string>();
            foreach (var line in lines)
            {
                var linet = line.Replace('F', '┌')
                    .Replace('-', '─')
                    .Replace('7', '┐')
                    .Replace('|', '│')
                    .Replace('J', '┘')
                    .Replace('L', '└')
                    ;
                lines2.Add(linet);
                if (line.Contains('S'))
                {
                    this.startY = lines2.Count - 1;
                    this.startX = line.IndexOf('S');
                }
            }
            this.map = lines2.ToArray();

            /*
            0, 2
            0, 3
            0, 4
            1, 4
            1, 3 
            2, 3
            3, 3
            4, 3
            4, 2
            3, 2
            3, 1
            3, 0
            2, 0
            2, 1
            1, 1
            1, 2
             */

            int distance = 0;
            int x = startX, y = startY;
            while (true)
            {
                if (getValidDir(x, y, out var nextx, out var nexty))
                {
                    distance++;
                    Console.WriteLine($"{x},{y} -> {nextx},{nexty}");
                    x = nextx; y = nexty;
                }
                else
                    break;
            }

            //File.WriteAllLines("map-big.txt", lines2, Encoding.UTF8);
        }

        private bool getValidDir(int x, int y, out int nextx, out int nexty)
        {
            if (x == 3 && y == 3)
            {
            }
            if (checkDir('N', x, y, out nextx, out nexty)) return true;
            if (checkDir('S', x, y, out nextx, out nexty)) return true;
            if (checkDir('W', x, y, out nextx, out nexty)) return true;
            if (checkDir('E', x, y, out nextx, out nexty)) return true;

            return false;
        }

        private bool checkDir(char dir, int x, int y, out int newx, out int newy)
        {
            string validPaths = "";
            bool checkX = false, checkY = false;
            newx = x;
            newy = y;
            /*
┌ ─ ┐ │ ┘ └             
             */


            switch (dir)
            {
                case 'N': // north
                    newy--;
                    validPaths = "│┐┌";
                    checkY = true;
                    break;
                case 'S': // south
                    newy++;
                    validPaths = "│┘└";
                    checkY = true;
                    break;
                case 'W': // west
                    newx--;
                    validPaths = "─┌└";
                    checkX = true;

                    break;
                case 'E': // east
                    newx++;
                    validPaths = "┘┐─";
                    checkX = true;

                    break;
                default:
                    break;
            }

            if (checkX && (newx < 0 || newx >= width)) return false;
            if (checkY && (newy < 0 || newy >= height)) return false;
            if( visited[newx, newy] ) return false;

            if (validPaths.Contains(map[newy][newx]))
            {
                visited[newx, newy] = true;
                return true;
            }
            return false;

        }
    }
}