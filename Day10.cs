using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        Point[] coords = new Point[2];


        (int x, int y, string alowedChars)[] dirVariants = new (int x, int y, string chars)[]
        {
            (1, 0, "┘┐─"), // doprava
            (-1, 0, "─└┌"), // dolava
            (0, 1, "│┘└"),  // dole
            (0, -1, "│┐┌"),  // hore
        };

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
            coords[0] = new Point();
            coords[1] = new Point();

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
            visited[startX, startY] = true;
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
            int newx = 0, newy = 0;
            //int newx = 0, newy = 3;
            // find S direction
            bool found = false;
            foreach (var dir in dirVariants)
            {
                newx = x + dir.x;
                newy = y + dir.y;
                if (newx >= 0 && newy >= 0 && newx < width && newy < height && dir.alowedChars.Contains(map[newy][newx]))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new ApplicationException("nenajdena cesta");
            }
            visited[newx, newy] = true;
            distance++;
            while (true)
            {
                var signal = stepNext(ref newx, ref newy);
                Console.WriteLine($"{newx}, {newy}");
                distance++;

                if (map[newy][newx] == 'S')
                {
                    // stop
                    break;
                }
            }

            //File.WriteAllLines("map-big.txt", lines2, Encoding.UTF8);
        }

        private int stepNext(ref int x, ref int y)
        {
            int newx, newy;
            var actChar = map[y][x];
            coords[0].X = coords[0].Y = coords[1].X = coords[1].Y = 0;
            switch (actChar)
            {
                case '─': // x:1, x:-1
                    coords[0].X = 1;
                    coords[1].X = -1;
                    break;
                case '│': // y: 1, y: -1
                    coords[0].Y = 1;
                    coords[1].Y = -1;
                    break;
                case '┌': // x: 1, y: 1
                    coords[0].X = 1;
                    coords[1].Y = 1;
                    break;
                case '┐': // x: -1, y: 1
                    coords[0].X = -1;
                    coords[1].Y = 1;
                    break;
                case '┘': // x: -1, y: -1
                    coords[0].X = -1;
                    coords[1].Y = -1;
                    break;
                case '└': // x: 1, y: -1
                    coords[0].X = 1;
                    coords[1].Y = -1;
                    break;

                default:
                    throw new ApplicationException("bad char");
            }
            foreach (var coord in coords)
            {
                newx = x + coord.X;
                newy = y + coord.Y;
                if (newx < 0 || newy < 0 || newx >= width || newy >= height)
                    continue;
                if (map[newy][newx] == 'S')
                {
                    x = newx;
                    y = newy;
                    return 2;
                }
                if (visited[newx, newy])
                    continue;

                // pozicia je validna, ale je tam validny znak?
                // validnu znak je taky, ktory sa vie napojit na predosly
                /*
            (1, 0, "┘┐─"), // doprava
            (-1, 0, "─└┌"), // dolava
            (0, 1, "│┘└"),  // dole
            (0, -1, "│┐┌"),  // hore                 
                 */
                string validChars = "";
                if (coord.X == 1)
                    validChars = "┘┐─";
                else if (coord.X == -1)
                    validChars = "─└┌";
                else if (coord.Y == 1)
                    validChars = "│┘└";
                else if (coord.Y == -1)
                    validChars = "│┐┌";

                var newChar = map[newy][newx];

                if (validChars.Contains(newChar))
                {
                    x = newx;
                    y = newy;
                    visited[x, y] = true;
                    return 1;
                }
            }
            return 0;
        }
    }
}