using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day10Solution : ISolution
    {
        public List<int> Input { get; set; }
        public async Task ReadInput(string file)
        {
            Input = (await File.ReadAllLinesAsync(file)).Select(a => Convert.ToInt32(a)).OrderBy(a => a).ToList();
            Input.Insert(0, 0);
            Input.Add(Input.Max() + 3);
        }

        public void Solve1()
        {
            var differences = new List<int>();
            for (int i = 0; i < Input.Count() - 1; i++)
            {
                differences.Add(Input[i + 1] - Input[i]);
            }
            var d1 = differences.Where(a => a == 1).Count();
            var d3 = differences.Where(a => a == 3).Count();
            Console.WriteLine($"{d1}x{d3}={d1 * d3}");
        }

        public void Solve2()
        {
            var s = new decimal[Input.Count()];
            s[0] = 1;
            foreach (var i in Enumerable.Range(1, Input.Count() - 1))
            {
                foreach (var j in Enumerable.Range(0, i))
                {
                    if (Input[i] - Input[j] <= 3)
                    {
                        s[i] += s[j];
                    }
                }
            }

            Console.WriteLine(s.Last());
        }
    }
}
