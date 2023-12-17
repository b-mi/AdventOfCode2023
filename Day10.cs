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
        int zoomArg = 3;
        private char startChar;


        //(int x, int y, string alowedChars)[] dirVariants = new (int x, int y, string chars)[]
        //{
        //    (1, 0, "┘┐─"), // doprava
        //    (-1, 0, "─└┌"), // dolava
        //    (0, 1, "│┘└"),  // dole
        //    (0, -1, "│┐┌"),  // hore
        //};

        public Day10()
        {
            int day = 10;
            var input = @"7-F7-
.FJ|7
SJLL7
|F--J
LJ.LJ";

            int inpId = 2; // 1-4

            switch (inpId)
            {
                case 1:
                    input = @".F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...";

                    break;
                case 2:
                    input = @"..........
.S------7.
.|F----7|.
.||....||.
.||....||.
.|L-7F-J|.
.|..||..|.
.L--JL--J.
..........";

                    break;
                case 3:
                    input =
        @"FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJIF7FJ-
L---JF-JLJIIIIFJLJJ7
|F|F-JF---7IIIL7L|7|
|FFJF7L7F-JF7IIL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L";

                    break;
                case 4:
                    input =
        @"FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJ7F7FJ-
L---JF-JLJ.||-FJLJJ7
|F|F-JF---7F7-L7L|7|
|FFJF7L7F-JF7|JL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L";

                    break;
                default:
                    break;
            }

            // input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();
            //part1(lines);
            part2(lines);
        }

        private void part2(string[] lines)
        {
            this.width = lines[0].Length;
            this.height = lines.Length;
            this.visited = new bool[this.width, this.height];
            var zoom = new char[this.width * zoomArg, this.height * zoomArg];

            coords[0] = new Point();
            coords[1] = new Point();

            #region translation & get start pos
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
            #endregion


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
            int newx = startX, newy = startY;

            // ziskanie validneho smeru zo startu
            var sgHor = stepNextChar(ref newx, ref newy, '─', false);
            if (sgHor != 1)
            {
                newx = startX; newy = startY;
                var sgVert = stepNextChar(ref newx, ref newy, '│', false);
                if (sgVert != 1)
                    throw new ApplicationException("no connection of S");
            }

            distance++;
            while (true)
            {
                var signal = stepNext(ref newx, ref newy, distance > 1);
                //Console.WriteLine($"{newx}, {newy}");
                distance++;

                if (map[newy][newx] == 'S')
                {
                    // stop
                    break;
                }
            }

            this.startChar = '┌';
            // fixfields
            for (int y0 = 0; y0 < height; y0++)
                for (int x0 = 0; x0 < width; x0++)
                {
                    setZoom(x0, y0, zoom);
                }


            saveFields(zoom, lines2);


            Console.WriteLine($"Vysledok: {distance >> 1}");
        }

        private void setZoom(int x, int y, char[,] zoom)
        {
            var lns = new string[3];
            var c = map[y][x];
            var zx = x * zoomArg;
            var zy = y * zoomArg;
            switch (c)
            {
                case '.':
                    lns[0] = "   ";
                    lns[1] = " . ";
                    lns[2] = "   ";
                    break;
                case '─':
                    lns[0] = "   ";
                    lns[1] = "───";
                    lns[2] = "   ";

                    break;
                case '│':
                    lns[0] = " │ ";
                    lns[1] = " │ ";
                    lns[2] = " │ ";
                    break;

                case '┌':
                    lns[0] = "   ";
                    lns[1] = " ┌─";
                    lns[2] = " │ ";
                    break;

                case '┐':
                    lns[0] = "   ";
                    lns[1] = "─┐ ";
                    lns[2] = " │ ";
                    break;

                case '┘':
                    lns[0] = " │ ";
                    lns[1] = "─┘ ";
                    lns[2] = "   ";
                    break;

                case '└':
                    lns[0] = " │ ";
                    lns[1] = " └─";
                    lns[2] = "   ";
                    break;

                case 'S':
                    lns[0] = "   ";
                    lns[1] = " S ";
                    lns[2] = "   ";
                    break;

                default:
                    break;
            }
            for (int row = 0; row < zoomArg; row++)
            {
                for (int col = 0; col < zoomArg; col++)
                {
                    var zc = lns[row][col];
                    zoom[zx + col, zy + row] = zc;
                }
            }
        }

        private void saveFields(char[,] fields, List<string> lines2)
        {
            var fileName = "map-part2-W.txt";
            File.WriteAllLines(fileName, lines2, Encoding.UTF8);

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("------------------------------");
            sb.AppendLine();
            for (int y = 0; y < height * zoomArg; y++)
            {
                for (int x = 0; x < width * zoomArg; x++)
                {
                    sb.Append(fields[x, y]);
                }
                sb.AppendLine();
            }
            File.AppendAllText(fileName, sb.ToString(), Encoding.UTF8);
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
            int newx = startX, newy = startY;
            visited[startX, startY] = true;

            var sgHor = stepNextChar(ref newx, ref newy, '─', false);
            if (sgHor != 1)
            {
                newx = startX; newy = startY;
                var sgVert = stepNextChar(ref newx, ref newy, '│', false);
                if (sgVert != 1)
                    throw new ApplicationException("no connection of S");
            }

            distance++;
            while (true)
            {
                var signal = stepNext(ref newx, ref newy, distance > 1);
                //Console.WriteLine($"{newx}, {newy}");
                distance++;

                if (map[newy][newx] == 'S')
                {
                    // stop
                    break;
                }
            }

            //File.WriteAllLines("map-part2-D.txt", lines2, Encoding.UTF8);

            Console.WriteLine($"Vysledok: {distance >> 1}");
        }

        private int stepNext(ref int x, ref int y, bool canTestS)
        {
            var actChar = map[y][x];
            return stepNextChar(ref x, ref y, actChar, canTestS);
        }

        private int stepNextChar(ref int x, ref int y, char actChar, bool canTestS)
        {
            int newx, newy;
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
                if (canTestS && map[newy][newx] == 'S')
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