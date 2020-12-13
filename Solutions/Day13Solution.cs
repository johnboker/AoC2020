using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day13Solution : ISolution
    {
        public int Timestamp0 { get; set; }
        public List<Bus> Busses { get; set; }
        public async Task ReadInput(string file)
        {
            var lines = await File.ReadAllLinesAsync(file);
            Timestamp0 = int.Parse(lines[0]);
            Busses = lines[1].Split(',').Select(a => new Bus { Number = a == "x" ? -1 : int.Parse(a) }).ToList();

            var offset = 0;
            foreach (var b in Busses)
            {
                b.Offset = offset;
                offset++;
            }
            Busses = Busses.Where(a => a.Number != -1).ToList();
        }

        public void Solve1()
        {
            var departed = false;
            Bus departureBus = null;
            int departureTime = 0;
            int t = Timestamp0;
            do
            {
                var departingBusses = Busses.Where(a => a.DepartsAtTime(t));
                if (departingBusses.Any())
                {
                    departureBus = departingBusses.First();
                    departureTime = t;
                    departed = true;
                }
                t++;
            } while (!departed);

            Console.WriteLine($"{departureTime - Timestamp0} x {departureBus.Number} = {(departureTime - Timestamp0) * departureBus.Number}");
        }

        public void Solve2()
        {
            var incrememt = Busses[0].Number;
            var time = 0L;
            for (var i = 1; i < Busses.Count(); i++)
            {
                var bus = Busses[i];
                while (true)
                {
                    time += incrememt;
                    if (bus.DepartsAtTime(time + bus.Offset))
                    {
                        incrememt *= bus.Number;
                        break;
                    }
                }
            }

            Console.WriteLine(time);
        }

        public class Bus
        {
            public long Offset { get; set; }
            public long Number { get; set; }
            public bool DepartsAtTime(long t)
            {
                return t % Number == 0;
            }
        }
    }
}
