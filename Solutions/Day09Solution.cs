using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day09Solution : ISolution
    {
        public decimal[] Input { get; set; }
        public async Task ReadInput(string file)
        {
            Input = (await File.ReadAllLinesAsync(file)).Select(a => Convert.ToDecimal(a)).ToArray();
        }

        private decimal RunSolve1()
        {
            for (var i = 0; i < Input.Count() - 26; i++)
            {
                var preamble = Input[i..(i + 25)];
                var n = Input[i + 25];
                var good = (from a in preamble
                            from b in preamble
                            where a != b
                            select a + b).Any(s => s == n);

                if (!good)
                {
                    return n;
                }
            }

            return -1;
        }

        public void Solve1()
        {
            Console.WriteLine(RunSolve1());
        }

        public void Solve2()
        {
            var n = RunSolve1();
            for (var i = 0; i < Input.Count() - 2; i++)
            {
                for (var j = i + 2; j < Input.Count(); j++)
                {
                    var range = Input[i..j];
                    if (range.Sum() == n)
                    {
                        Console.WriteLine(range.Min() + range.Max());
                        break;
                    }
                }
            }
        }
    }
}
