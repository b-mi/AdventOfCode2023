using System.Linq;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace AdventOfCode2023
{
    internal class Day4
    {
        int total = 0;


        public Day4()
        {
            int day = 4;
            var input = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";


            input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();


            var sum = part1(lines);
            //var sum = part2(lines);
            Console.WriteLine($"Vysledok: {sum}");
        }

        private int part1(string[] lines)
        {
            var sep = new char[] { ' ' };
            var dctCards = new Dictionary<int, int>();

            // ziskanie kariat a poctu vitaztiev
            foreach (var line in lines)
            {
                var parts = line.Split(':', '|');
                var cardId = int.Parse(parts[0].Split(sep, StringSplitOptions.RemoveEmptyEntries)[1]);
                var winNums = parts[1].Split(sep, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
                var myNums = parts[2].Split(sep, StringSplitOptions.RemoveEmptyEntries);
                var winCnt = myNums.Where(n => winNums.Contains(n)).Count();
                dctCards.Add(cardId, winCnt);
            }

            // vytvorenie zoznamu

            foreach (var item in dctCards)
            {
                addCards(item.Key, dctCards);
            }

            return total;
        }

        private void addCards(int cardId, Dictionary<int, int> dctCards)
        {
            total++;
            var winCnt = dctCards[cardId];
            if (winCnt > 0)
            {
                for (int cardCopyId = cardId + 1; cardCopyId <= cardId + winCnt; cardCopyId++)
                {
                    addCards(cardCopyId, dctCards);
                }
            }
        }
    }
}