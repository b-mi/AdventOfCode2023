using System.Linq;
using System;

namespace AdventOfCode2023
{
    internal class Day6
    {
        public Day6()
        {

            int day = 6;
            var input = @"Time:      7  15   30
Distance:  9  40  200";


            input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();
            //part1(lines);
            part2(lines);
        }


        private int part2(string[] lines)
        {
            var times = new ulong[] { ulong.Parse(lines[0].Replace(" ", "").Split(':')[1]) };
            var dists = new ulong[] { ulong.Parse(lines[1].Replace(" ", "").Split(':')[1]) };
            ulong total = 1;
            for (int i = 0; i < times.Length; i++)
            {
                var duration = times[i];
                var record = dists[i];
                ulong recCnt = 0;
                for (ulong pushed_ms = 1; pushed_ms < duration; pushed_ms++)
                {
                    var race_time = duration - pushed_ms;
                    var result_dist = pushed_ms * race_time;
                    if (result_dist > record)
                        recCnt++;

                }
                total *= recCnt;
                Console.WriteLine($"RACE: time {duration}, record: {record}, result: {recCnt}");

            }
            Console.WriteLine($"Vysledok: {total}");
            return 0;
        }


        private int part1(string[] lines)
        {
            var times = lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(i => int.Parse(i)).ToArray();
            var dists = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(i => int.Parse(i)).ToArray();
            int total = 1;
            for (int i = 0; i < times.Length; i++)
            {
                var duration = times[i];
                var record = dists[i];
                int recCnt = 0;
                for (int pushed_ms = 1; pushed_ms < duration; pushed_ms++)
                {
                    var race_time = duration - pushed_ms;
                    var result_dist = pushed_ms * race_time;
                    if (result_dist > record)
                        recCnt++;

                }
                total *= recCnt;
                Console.WriteLine($"RACE: time {duration}, record: {record}, result: {recCnt}");

            }
            Console.WriteLine($"Vysledok: {total}");
            return 0;
        }
    }
}