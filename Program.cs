using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Day7();
                //new Day6();
                //new Day5();
                //new Day4();
                //new Day3();
                //new Day2();
                //new Day1();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done. Press key.");
            Console.ReadKey();
        }
    }
}
