using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day17Solution : ISolution
    {
        public Dictionary<(int X, int Y, int Z), char> Input { get; set; }
        public Dictionary<(int X, int Y, int Z, int W), char> Input4 { get; set; }

        public async Task ReadInput(string file)
        {
            var lines = await File.ReadAllLinesAsync(file);
            Input = new Dictionary<(int X, int Y, int Z), char>();
            Input4 = new Dictionary<(int X, int Y, int Z, int W), char>();
            var space = new Space();
            var space4 = new Space4();
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    space.SetValueForPosition((x, y, 0), line[x], Input);
                    space4.SetValueForPosition((x, y, 0, 0), line[x], Input4);
                }
            }
        }
        public void Solve1()
        {
            var space = new Space(Input);
            for (var i = 0; i < 6; i++)
            {
                space.Cycle();
            }
            Console.WriteLine(space.ActiveCount);
        }
        public void Solve2()
        {
            var space = new Space4(Input4);
            for (var i = 0; i < 6; i++)
            {
                space.Cycle();
            }
            Console.WriteLine(space.ActiveCount);
        }

        public class Space4
        {
            public Space4()
            {
                Data = new Dictionary<(int X, int Y, int Z, int W), char>();
            }
            public Space4(Dictionary<(int X, int Y, int Z, int W), char> data)
            {
                Data = new Dictionary<(int X, int Y, int Z, int W), char>(data);
            }
            public int CycleCount { get; set; }
            public Dictionary<(int X, int Y, int Z, int W), char> Data { get; set; }
            public void SetValueForPosition((int X, int Y, int Z, int W) position, char v, Dictionary<(int X, int Y, int Z, int W), char> data = null)
            {
                data ??= Data;

                if (v == '#')
                {
                    data[position] = '#';
                }
                else
                {
                    if (data.ContainsKey(position)) data.Remove(position);
                }
            }

            public int NeighborCount((int X, int Y, int Z, int W) position, Dictionary<(int X, int Y, int Z, int W), char> data)
            {
                var neigbors = Neighbors(position);
                return neigbors.Where(a => IsActive(a, data)).Count();
            }

            public List<(int X, int Y, int Z, int W)> Neighbors((int X, int Y, int Z, int W) position)
            {
                var (x, y, z, w) = position;

                var neighbors = new List<(int X, int Y, int Z, int W)>();
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        for (var k = -1; k <= 1; k++)
                        {
                            for (var l = -1; l <= 1; l++)
                            {
                                neighbors.Add((x + i, y + j, z + k, w + l));
                            }
                        }
                    }
                }
                neighbors.Remove(position);
                return neighbors;
            }

            public void Cycle()
            {
                var current = new Dictionary<(int X, int Y, int Z, int W), char>(Data);
                var checks = current.Keys.Select(a => Neighbors(a)).SelectMany(a => a).ToList();
                checks.AddRange(current.Keys);
                checks = checks.Distinct().ToList();

                foreach (var check in checks)
                {
                    var active = IsActive(check, current);
                    var neighborCount = NeighborCount(check, current);
                    if (active && !(neighborCount == 2 || neighborCount == 3))
                    {
                        SetValueForPosition(check, '.', Data);
                    }
                    if (!active && neighborCount == 3)
                    {
                        SetValueForPosition(check, '#', Data);
                    }
                }

                CycleCount++;

            }


            private bool IsActive((int X, int Y, int Z, int W) location, Dictionary<(int X, int Y, int Z, int W), char> data) => data.ContainsKey(location);

            public int ActiveCount => Data.Count();
        }




        public class Space
        {
            public Space()
            {
                Data = new Dictionary<(int X, int Y, int Z), char>();
            }
            public Space(Dictionary<(int X, int Y, int Z), char> data)
            {
                Data = new Dictionary<(int X, int Y, int Z), char>(data);
            }
            public int CycleCount { get; set; }
            public Dictionary<(int X, int Y, int Z), char> Data { get; set; }
            public void SetValueForPosition((int X, int Y, int Z) position, char v, Dictionary<(int X, int Y, int Z), char> data = null)
            {
                data ??= Data;

                if (v == '#')
                {
                    data[position] = '#';
                }
                else
                {
                    if (data.ContainsKey(position)) data.Remove(position);
                }
            }

            public int NeighborCount((int X, int Y, int Z) position, Dictionary<(int X, int Y, int Z), char> data)
            {
                var neighbors = Neighbors(position);
                return neighbors.Where(a => IsActive(a, data)).Count();
            }

            public List<(int X, int Y, int Z)> Neighbors((int X, int Y, int Z) position)
            {
                var (x, y, z) = position;

                var neighbors = new List<(int X, int Y, int Z)>();
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        for (var k = -1; k <= 1; k++)
                        {
                            neighbors.Add((x + i, y + j, z + k));
                        }
                    }
                }
                neighbors.Remove(position);
                return neighbors;
            }

            public void Cycle()
            {
                var current = new Dictionary<(int X, int Y, int Z), char>(Data);
                var checks = current.Keys.Select(a => Neighbors(a)).SelectMany(a => a).ToList();
                checks.AddRange(current.Keys);
                checks = checks.Distinct().ToList();

                foreach (var check in checks)
                {
                    var active = IsActive(check, current);
                    var neighborCount = NeighborCount(check, current);
                    if (active && !(neighborCount == 2 || neighborCount == 3))
                    {
                        SetValueForPosition(check, '.', Data);
                    }
                    if (!active && neighborCount == 3)
                    {
                        SetValueForPosition(check, '#', Data);
                    }
                }

                CycleCount++;
            }

            private void Print()
            {
                Console.WriteLine($"Cycle {CycleCount}");
                var minX = Data.Keys.Min(a => a.X);
                var maxX = Data.Keys.Max(a => a.X);

                var minY = Data.Keys.Min(a => a.Y);
                var maxY = Data.Keys.Max(a => a.Y);

                var minZ = Data.Keys.Min(a => a.Z);
                var maxZ = Data.Keys.Max(a => a.Z);

                for (var z = minZ; z <= maxZ; z++)
                {
                    Console.WriteLine($"Z={z}");
                    for (var y = minY; y <= maxY; y++)
                    {
                        for (var x = minX; x <= maxX; x++)
                        {
                            Console.Write(IsActive((x, y, z), Data) ? "#" : ".");
                        }
                        Console.WriteLine();
                    }
                }
            }

            private bool IsActive((int X, int Y, int Z) location, Dictionary<(int X, int Y, int Z), char> data) => data.ContainsKey(location);

            public int ActiveCount => Data.Count();
        }
    }
}