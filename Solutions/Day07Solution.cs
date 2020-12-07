using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day07Solution : ISolution
    {
        private List<Bag> Bags { get; set; }

        public async Task ReadInput(string file)
        {
            Bags = new List<Bag>();
            var lines = await File.ReadAllLinesAsync(file);

            foreach (var line in lines)
            {
                var parts = line.Split(" bags contain ");
                var bagColor = parts[0];
                var bag = Bags.FirstOrDefault(a => a.Color == bagColor);
                if (bag == null)
                {
                    bag = new Bag { Color = bagColor };
                    Bags.Add(bag);
                }

                var bagInfos = from b in parts[1].Split(",").Where(a => a.Trim() != "no other bags.")
                               let p = b.Replace("bags", "").Replace("bag", "").Replace(".", "").Trim()
                               let n = Convert.ToInt32(p[0..p.IndexOf(' ')])
                               let c = p[p.IndexOf(' ')..].Trim()
                               select new { Number = n, Color = c };

                foreach (var bi in bagInfos)
                {
                    var bagInfoBag = Bags.FirstOrDefault(a => a.Color == bi.Color);
                    if (bagInfoBag == null)
                    {
                        bagInfoBag = new Bag { Color = bi.Color };
                        Bags.Add(bagInfoBag);
                    }
                    bag.CanHold.Add(bagInfoBag, bi.Number);
                }
            }
        }

        public void Solve1()
        {
            var cnt = Bags.Sum(a => a.CanHaveGold() ? 1 : 0);
            Console.WriteLine(cnt);
        }

        public void Solve2()
        {
            var goldBag = Bags.FirstOrDefault(a => a.Color == "shiny gold");
            Console.WriteLine(goldBag.CountBags());
        }
    }

    public class Bag
    {
        public Bag() => CanHold = new Dictionary<Bag, int>();

        public string Color { get; set; }

        public Dictionary<Bag, int> CanHold { get; set; }

        public bool CanHaveGold()
        {
            if (CanHold.Any(a => a.Key.Color == "shiny gold")) return true;

            foreach (var b in CanHold.Keys)
            {
                if (b.CanHaveGold())
                {
                    return true;
                }
            }

            return false;
        }

        public int CountBags()
        {
            var cnt = 0;

            cnt += CanHold.Values.Sum();

            foreach (var (bag, count) in CanHold)
            {
                cnt += count * bag.CountBags();
            }

            return cnt;
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Bag b)) return false;
            return b.Color == Color;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Color.GetHashCode();
            }
        }
    }
}
