using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day05Solution : ISolution
    {
        private List<string> Passes { get; set; }

        public async Task ReadInput(string file)
        {
            Passes = (await File.ReadAllLinesAsync(file))
                .Select(a => a.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1"))
                .ToList();
        }

        public void Solve1()
        {
            var seats = Passes
                        .Select(a => Convert.ToInt32(a[0..7], 2) * 8 + Convert.ToInt32(a[7..10], 2));

            Console.WriteLine($"{seats.Max()}");
        }

        public void Solve2()
        {
            var seats = Passes
                        .Select(a => Convert.ToInt32(a[0..7], 2) * 8 + Convert.ToInt32(a[7..10], 2))
                        .OrderBy(a => a)
                        .ToList();

            for (var i = 0; i < seats.Count() - 1; i++)
            {
                var diff = seats[i] - seats[i + 1];
                if (Math.Abs(diff) > 1) Console.WriteLine($"{seats[i] + 1}");
            }
        }
    }
}
