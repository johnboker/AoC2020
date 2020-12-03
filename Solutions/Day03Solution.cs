using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day03Solution : ISolution
    {
        private List<string> Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = (await File.ReadAllLinesAsync(file)).ToList();
        }

        public void Solve1()
        {
            var lineLength = Input[0].Length;

            var treeCount = 0;
            for (var i = 0; i < Input.Count; i++)
            {
                treeCount += Input[i][i * 3 % lineLength] == '#' ? 1 : 0;
            }

            Console.WriteLine(treeCount);
        }

        public void Solve2()
        {
            var lineLength = Input[0].Length;

            var slopes = new[] { (x: 1, y: 1), (x: 3, y: 1), (x: 5, y: 1), (x: 7, y: 1), (x: 1, y: 2) };
            var treeCounts = new ulong[slopes.Count()];

            for (var i = 0; i < Input.Count; i++)
            {
                int s = 0;
                foreach (var (x, y) in slopes)
                {
                    if (i * x % y == 0)
                    {
                        treeCounts[s] += Input[i][i * x / y % lineLength] == '#' ? 1UL : 0UL;
                    }
                    s++;
                }
            }

            Console.WriteLine(treeCounts.Aggregate((a, b) => a * b));
        }
    }
}
