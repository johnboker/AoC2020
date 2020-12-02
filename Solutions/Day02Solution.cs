using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day02Solution : ISolution
    {
        private List<InputPassword> Input { get; set; }

        public async Task ReadInput(string file)
        {
            Input = (from i in (await File.ReadAllLinesAsync(file))
                     let p = i.Split(" ").Select(a => a.Trim(':')).ToArray()
                     let m = p[0].Split('-')
                     select new InputPassword
                     {
                         Password = p[2],
                         Min = Convert.ToInt32(m[0]),
                         Max = Convert.ToInt32(m[1]),
                         Character = p[1][0]
                     }).ToList();
        }

        public void Solve1()
        {
            var answer = Input.Where(a => a.IsValid1()).Count();
            Console.WriteLine(answer);
        }

        public void Solve2()
        {
            var answer = Input.Where(a => a.IsValid2()).Count();
            Console.WriteLine(answer);
        }

        public class InputPassword
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public char Character { get; set; }
            public string Password { get; set; }

            public bool IsValid1()
            {
                var cnt = Password.Where(a => a == Character).Count();
                return cnt >= Min && cnt <= Max;
            }

            public bool IsValid2()
            {
                var c1 = Password[Min - 1];
                var c2 = Password[Max - 1];

                return (c1 == Character || c2 == Character) && c1 != c2;
            }
        }
    }
}