using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day05Solution : ISolution
    {
        private List<int> Passes { get; set; }

        public async Task ReadInput(string file)
        {
            Passes = (await File.ReadAllLinesAsync(file))
                .Select(a => Convert.ToInt32(a.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1"), 2))
                .OrderBy(a => a)
                .ToList();
        }

        public void Solve1()
        {
            Console.WriteLine($"{Passes.Max()}");
        }

        public void Solve2()
        {
            for (var i = 0; i < Passes.Count() - 1; i++)
            {
                var diff = Passes[i + 1] - Passes[i];
                if (diff > 1) Console.WriteLine($"{Passes[i] + 1}");
            }
        }
    }
}
