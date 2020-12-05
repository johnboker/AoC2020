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
            Passes = (await File.ReadAllLinesAsync(file)).ToList();
        }

        public void Solve1()
        {
            var maxSeat = 0;
            foreach (var p in Passes)
            {
                var rows = (min: 0, max: 127);
                var n = 64;
                for (var i = 0; i < 7; i++)
                {
                    if (p[i] == 'F') rows.max -= n;
                    else if (p[i] == 'B') rows.min += n;
                    n /= 2;
                }

                var cols = (min: 0, max: 7);
                n = 4;
                for (var i = 7; i < 10; i++)
                {
                    if (p[i] == 'L') cols.max -= n;
                    else if (p[i] == 'R') cols.min += n;
                    n /= 2;
                }

                var seat = rows.min * 8 + cols.min;
                if (seat > maxSeat) maxSeat = seat;
            }

            Console.WriteLine($"{maxSeat}");
        }

        public void Solve2()
        {
            var seats = new List<int>();
            foreach (var p in Passes)
            {
                var rows = (min: 0, max: 127);
                var n = 64;
                for (var i = 0; i < 7; i++)
                {
                    if (p[i] == 'F') rows.max -= n;
                    else if (p[i] == 'B') rows.min += n;
                    n /= 2;
                }

                var cols = (min: 0, max: 7);
                n = 4;
                for (var i = 7; i < 10; i++)
                {
                    if (p[i] == 'L') cols.max -= n;
                    else if (p[i] == 'R') cols.min += n;
                    n /= 2;
                }

                var seat = rows.min * 8 + cols.min;
                seats.Add(seat);
            }

            seats = seats.OrderBy(a => a).ToList();

            for (var i = 0; i < seats.Count() - 1; i++)
            {
                var diff = seats[i] - seats[i + 1];
                if (Math.Abs(diff) > 1) Console.WriteLine($"{seats[i] + 1}");
            }
        }
    }
}
