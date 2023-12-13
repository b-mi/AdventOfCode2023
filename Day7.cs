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
            //part1(lines);
            part2(lines);
        }

        private void part2(string[] lines)
        {
            string cPower = "AKQT98765432J";
            var cards = new List<Card>();
            foreach (var line in lines)
                cards.Add(new Card(line, true));



            cards.Sort(delegate (Card x, Card y)
            {
                if (x.Power < y.Power) return -1;
                if (x.Power > y.Power) return 1;

                // same power
                if (x.Data == y.Data)
                    return 0; // to iste
                int i = 0;
                while (x.Data[i] == y.Data[i]) i++; // preskocenie rovnakych

                int xC = cPower.IndexOf(x.Data[i]);
                int yC = cPower.IndexOf(y.Data[i]);

                if (xC < yC) return 1;
                if (xC > yC) return -1;

                return 0;
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

        private void part1(string[] lines)
        {
            string cPower = "AKQJT98765432";
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

                return 1; // preco 1?
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


        public Card(string line, bool ver2)
        {
            var parts = line.Split(' ');
            this.Data = parts[0];
            this.Offer = int.Parse(parts[1]);
            int JCount = 0;
            var isJ = Data.Contains('J');
            if (isJ)
                JCount = this.Data.Count((c) => c == 'J');

            this.grps = Data.GroupBy(c => c);
            var grpsCount = grps.Count();
            if (grpsCount == 5)  // vsetky pismena odlisne
            {
                Power = 0; // High card
                if (JCount > 0) // jedno z nich je J
                    Power = 1; // One pair
            }
            else if (grpsCount == 4) // jedna dvojicka, vsetko ostatne odlisne
            {
                Power = 1; // One pair
                if (JCount > 0)
                {
                    // JJABC - je to trojicka
                    // AABCJ - je to trojicka
                    Power = 3; // jedna trojicka
                }
            }
            else if (grpsCount == 3)
            {
                /*
                1 + 1 + 3

                J4555 - four of kind
                4J555 - four of kind
                45JJJ - four of kind

                1 + 2 + 2
                J4455 - full house
                4JJ55 - four of kind
                455JJ - four of kind
                 */

                if (grps.Any(g => g.Count() == 3))
                {
                    Power = 3; // Three of kind
                    if (JCount > 0)
                    {
                        Power = 5; // Four of kind
                    }
                }
                else
                {
                    Power = 2; // Two pair
                    if (JCount > 0)
                    {
                        if (JCount == 1)
                            Power = 4; // full house 3+2
                        else
                            Power = 5; // four of kind
                    }
                }

            }
            else if (grpsCount == 2)
            {
                /*
                 3+2

                JJJ77
                777JJ

                 4+1

                JJJJ6
                6666J

                 */

                if (JCount > 0)
                    Power = 6;
                else
                {
                    if (grps.Any(g => g.Count() == 3))
                        Power = 4; //FullHouse
                    else
                        Power = 5; //Four of kind
                }
            }
            else
                Power = 6;  // Five od kind
        }


    }
}