using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day15Solution : ISolution
    {
        public List<int> Numbers { get; set; }

        public async Task ReadInput(string file)
        {
            Numbers = (await File.ReadAllTextAsync(file)).Split(",").Select(a => Convert.ToInt32(a)).ToList();
        }

        public void Solve1()
        {
            Console.WriteLine(FindNumberSpokenOnTurn(2020));
        }

        public void Solve2()
        {

            Console.WriteLine(FindNumberSpokenOnTurn(30000000));
        }

        public int FindNumberSpokenOnTurn(int maxTurn)
        {
            var spokenNumbers = Numbers
                        .Select((n, i) => new
                        {
                            Number = n,
                            Index = i
                        })
                        .OrderBy(a => a.Index)
                        .ToDictionary(k => k.Number, v => v.Index + 1);

            var lastNumber = Numbers.Last();

            for (int turn = Numbers.Count(); turn < maxTurn; turn++)
            {
                var next = spokenNumbers.ContainsKey(lastNumber) ? turn - spokenNumbers[lastNumber] : 0;
                spokenNumbers[lastNumber] = turn;
                lastNumber = next;
            }

            return lastNumber;
        }
    }
}