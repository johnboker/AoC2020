using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            var spokenNumbers = new SortedDictionary<int, List<int>>(Numbers
                        .Select((n, i) => new
                        {
                            Number = n,
                            Index = i
                        })
                        .OrderBy(a => a.Index)
                        .ToDictionary(k => k.Number, v => new List<int> { v.Index }));

            var lastNumber = Numbers.Last();

            for (int turn = Numbers.Count(); turn < maxTurn; turn++)
            {
                var numberOfTimesSpoken = spokenNumbers[lastNumber].Count;

                if (numberOfTimesSpoken == 1)
                {
                    if (spokenNumbers.ContainsKey(0))
                    {
                        spokenNumbers[0].Add(turn);
                    }
                    else
                    {
                        spokenNumbers.Add(0, new List<int> { turn });
                    }
                    lastNumber = 0;
                }
                else if (numberOfTimesSpoken > 1)
                {
                    var lastTwo = spokenNumbers[lastNumber].TakeLast(2).ToList();
                    var n = Math.Abs(lastTwo[0] - lastTwo[1]);
                    if (spokenNumbers.ContainsKey(n))
                    {
                        spokenNumbers[n].Add(turn);
                    }
                    else
                    {
                        spokenNumbers.Add(n, new List<int> { turn });
                    }
                    lastNumber = n;
                }
            }

            return spokenNumbers.Where(a => a.Value.Any(b => b == maxTurn - 1)).FirstOrDefault().Key;
        }
    }
}