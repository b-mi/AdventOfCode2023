using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace AdventOfCode2023
{
    internal class Helper
    {
        internal static IEnumerable<string> GetLines(string input)
        {
            return input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        internal static string GetWebInput(int day)
        {
            var file = Directory.GetCurrentDirectory();
            file = Path.Combine($"..\\..\\day{day}.txt");
            file = Path.GetFullPath(file);

            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                Process.Start(file);
                throw new Exception($"Vytvorenie inputu {file}");
            }

            return File.ReadAllText(file);
        }
    }
}