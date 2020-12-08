using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day04Solution : ISolution
    {
        private List<Passport> Passports { get; set; }

        public async Task ReadInput(string file)
        {
            Passports = new List<Passport>();
            var lines = (await File.ReadAllLinesAsync(file)).ToList();

            var pp = new Passport();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    Passports.Add(pp);
                    pp = new Passport();
                    continue;
                }
                var kvps = line.Split(' ').Select(a => a.Split(':'));
                foreach (var kvp in kvps)
                {
                    pp.Fields.Add(kvp[0], kvp[1]);
                }
            }
            Passports.Add(pp);
        }

        public void Solve1()
        {
            var count = Passports.Where(a => a.IsValid1()).Count();
            Console.WriteLine(count);
        }

        public void Solve2()
        {
            var count = Passports.Where(a => a.IsValid2()).Count();
            Console.WriteLine(count);
        }

        public class Passport
        {
            public Passport()
            {
                Fields = new Dictionary<string, string>();
            }
            public Dictionary<string, string> Fields { get; set; }

            public bool IsValid1()
            {
                if (Fields.Count() == 8) return true;
                if (Fields.Count() == 7 && !Fields.ContainsKey("cid")) return true;
                return false;
            }

            private readonly string[] ValidEyeColors = {
                "amb",
                "blu",
                "brn",
                "gry",
                "grn",
                "hzl",
                "oth"
            };

            public bool IsValid2()
            {
                if (IsValid1())
                {
                    var byr = Convert.ToInt32(Fields["byr"]);
                    if (byr < 1920 || byr > 2002) return false;

                    var iyr = Convert.ToInt32(Fields["iyr"]);
                    if (iyr < 2010 || iyr > 2020) return false;

                    var eyr = Convert.ToInt32(Fields["eyr"]);
                    if (eyr < 2020 || eyr > 2030) return false;

                    var hgt = Fields["hgt"];

                    if (!(hgt.EndsWith("in") || hgt.EndsWith("cm"))) return false;

                    var (min, max) = hgt.EndsWith("in") ? (min: 59, max: 76) : (min: 150, max: 193);
                    int.TryParse(hgt[0..^2], out var height);
                    if (height < min || height > max) return false;

                    var hcl = Fields["hcl"];
                    if (!(hcl.Length == 7 && hcl.StartsWith('#') && Regex.IsMatch(hcl[1..], @"\A\b[0-9a-fA-F]+\b\Z"))) return false;
                    if (!ValidEyeColors.Contains(Fields["ecl"])) return false;

                    var pid = Fields["pid"];
                    if (!(pid.Length == 9 && pid.All(char.IsDigit))) return false;

                    return true;
                }

                return false;
            }
        }
    }
}
