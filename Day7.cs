using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace AdventOfCode2023
{
    internal class Day7
    {
        public Day7()
        {
            int day = 7;
            var input = @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";


            input = Helper.GetWebInput(day);
            var lines = Helper.GetLines(input).ToArray();
            part1(lines);
        }

        static string cPower = "AKQJT98765432";

        private void part1(string[] lines)
        {
            var cards = new List<Card>();
            foreach (var line in lines)
                cards.Add(new Card(line));



            cards.Sort(delegate (Card x, Card y)
            {
                if (x.Power < y.Power) return -1;
                if (x.Power > y.Power) return 1;

                // same power
                int i = 0;
                while (x.Data[i] == y.Data[i]) i++; // preskocenie rovnakych

                int xC = cPower.IndexOf(x.Data[i]);
                int yC = cPower.IndexOf(y.Data[i]);

                if (xC < yC) return 1;
                if (xC > yC) return -1;

                return 1;
            });
            /*
1: 32T3K 765        1 dvoicka
2: KTJJT 220        2 dvojicky
3: KK677 28         2 dvojicky
4: T55J5 684        1 trojicka
5: QQQJA 483        1 trojicka

 */

            int sum = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                sum += card.Offer * (i + 1);
            }
            Console.WriteLine($"Vysledok je {sum}");
        }
    }


    [DebuggerDisplay("{Data}, Pow: {Power}, Offer: {Offer}")]
    internal class Card
    {
        public string Data { get; }
        public int Offer { get; }
        public int Power { get; }

        private IEnumerable<IGrouping<char, char>> grps;

        public Card(string line)
        {
            var parts = line.Split(' ');
            this.Data = parts[0];
            this.Offer = int.Parse(parts[1]);

            this.grps = Data.GroupBy(c => c);
            var grpsCount = grps.Count();
            if (grpsCount == 5)
                Power = 0; // High card
            else if (grpsCount == 4)
                Power = 1; // One pair
            else if (grpsCount == 3)
            {
                /*
                1 + 1 + 3
                1 + 2 + 2
                 */

                if (grps.Any(g => g.Count() == 3))
                    Power = 3; // Three of kind
                else
                    Power = 2; // Two pair

            }
            else if (grpsCount == 2)
            {
                /*
                 3+2
                 4+1
                 */
                if (grps.Any(g => g.Count() == 3))
                    Power = 4; //FullHouse
                else
                    Power = 5; //Four of kind
            }
            else
                Power = 6;  // Five od kind
        }
    }
}