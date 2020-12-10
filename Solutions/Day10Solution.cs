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
            var len = Input.Count();
            var s = new decimal[len];
            s[0] = 1;
            for (var i = 1; i < len; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (Input[i] - Input[j] <= 3)
                    {
                        s[i] += s[j];
                    }
                }
            }

            Console.WriteLine(s[len - 1]);
        }
    }
}
