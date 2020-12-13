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
            Busses = lines[1].Split(',').Select(a => new Bus { Number = a == "x" ? -1 : int.Parse(a), StartTime = Timestamp0 }).ToList();

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
            Busses.RemoveAt(0);
            var time = 0L;
            foreach (var bus in Busses)
            {
                var t = bus.Number;
                while (true)
                {
                    time += incrememt;
                    if ((time + bus.Offset) % t == 0)
                    {
                        incrememt *= t;
                        break;
                    }
                }
            }

            Console.WriteLine(time);
        }

        public class Bus
        {
            public long Offset { get; set; }
            public long StartTime { get; set; }
            public long Number { get; set; }
            public bool DepartsAtTime(int t)
            {
                return t % Number == 0;
            }
        }
    }
}
