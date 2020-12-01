using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day01Solution : ISolution
    {
        private int[] Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = (await File.ReadAllLinesAsync(file))
                    .Select(a => Convert.ToInt32(a))
                    .ToArray();
        }

        public void Solve1()
        {
            var answer = (from a in Input
                          from b in Input
                          where a != b && a + b == 2020
                          select a * b).FirstOrDefault();

            Console.WriteLine(answer);
        }

        public void Solve2()
        {
            var answer = (from a in Input
                          from b in Input
                          from c in Input
                          where a != b && a != c && b != c && a + b + c == 2020
                          select a * b * c).FirstOrDefault();

            Console.WriteLine(answer);
        }
    }
}