using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day12Solution : ISolution
    {
        public List<(char Action, int Value)> Input { get; set; }
        public async Task ReadInput(string file)
        {
            Input = (from i in await File.ReadAllLinesAsync(file)
                     select (i[0], Convert.ToInt32(i[1..])))
                     .ToList();
        }

        public void Solve1()
        {
            var location = (x: 0, y: 0);
            var direction = 0;

            foreach (var i in Input)
            {
                switch (i.Action)
                {
                    case 'N':
                        location.y -= i.Value;
                        break;
                    case 'S':
                        location.y += i.Value;
                        break;
                    case 'E':
                        location.x += i.Value;
                        break;
                    case 'W':
                        location.x -= i.Value;
                        break;
                    case 'L':
                        direction += 4 - i.Value / 90;
                        direction %= 4;
                        break;
                    case 'R':
                        direction += i.Value / 90;
                        direction %= 4;
                        break;
                    case 'F':
                        switch (direction)
                        {
                            case 0:
                                location.x += i.Value;
                                break;
                            case 1:
                                location.y += i.Value;
                                break;
                            case 2:
                                location.x -= i.Value;
                                break;
                            case 3:
                                location.y -= i.Value;
                                break;
                        }
                        break;
                }

            }

            Console.WriteLine($"{Math.Abs(location.x)} + {Math.Abs(location.y)} = {Math.Abs(location.x) + Math.Abs(location.y)}");
        }

        public void Solve2()
        {
            var location = (x: 0, y: 0);
            var waypoint = (x: 10, y: -1);

            foreach (var i in Input)
            {
                switch (i.Action)
                {
                    case 'N':
                        waypoint.y -= i.Value;
                        break;
                    case 'S':
                        waypoint.y += i.Value;
                        break;
                    case 'E':
                        waypoint.x += i.Value;
                        break;
                    case 'W':
                        waypoint.x -= i.Value;
                        break;
                    case 'R':
                    case 'L':
                        var r = i.Action == 'R' ? i.Value / 90 : 4 - (i.Value / 90);
                        for (var j = 0; j < r; j++)
                        {
                            waypoint.x ^= waypoint.y;
                            waypoint.y ^= waypoint.x;
                            waypoint.x ^= waypoint.y;
                            waypoint.x = -waypoint.x;
                        }
                        break;
                    case 'F':
                        location.x += i.Value * waypoint.x;
                        location.y += i.Value * waypoint.y;
                        break;
                }
            }

            Console.WriteLine($"{Math.Abs(location.x)} + {Math.Abs(location.y)} = {Math.Abs(location.x) + Math.Abs(location.y)}");
        }
    }
}
