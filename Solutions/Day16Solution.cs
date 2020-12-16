using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day16Solution : ISolution
    {
        public List<Rule> Rules { get; set; }
        public Ticket MyTicket { get; set; }
        public List<Ticket> NearbyTickets { get; set; }

        public async Task ReadInput(string file)
        {
            var lines = await File.ReadAllLinesAsync(file);
            var regex = new Regex(@"^(?<name>.*): (?<r1>[\d]+)-(?<r2>[\d]+) or (?<r3>[\d]+)-(?<r4>[\d]+)$");
            Rules = new List<Rule>();

            int index = 0;
            var line = "";
            do
            {
                line = lines[index];
                var m = regex.Matches(line)[0];
                var name = m.Groups["name"].Value;
                var r1 = Convert.ToInt32(m.Groups["r1"].Value);
                var r2 = Convert.ToInt32(m.Groups["r2"].Value);
                var r3 = Convert.ToInt32(m.Groups["r3"].Value);
                var r4 = Convert.ToInt32(m.Groups["r4"].Value);

                var rule = new Rule
                {
                    Name = name,
                    Ranges = new List<(int Min, int Max)> { (r1, r2), (r3, r4) },
                    Columns = new List<int>()
                };
                Rules.Add(rule);
                index++;
            } while (!string.IsNullOrWhiteSpace(lines[index]));
            index += 2;
            MyTicket = new Ticket { Numbers = lines[index].Split(",").Select(a => Convert.ToInt32(a)).ToList() };
            index += 3;

            NearbyTickets = new List<Ticket>();
            for (int i = index; i < lines.Length; i++)
            {
                var ticket = new Ticket { Numbers = lines[i].Split(",").Select(a => Convert.ToInt32(a)).ToList() };
                NearbyTickets.Add(ticket);
            }


        }
        public void Solve1()
        {
            var sum = NearbyTickets.SelectMany(t => t.Numbers.Where(n => Rules.All(r => !r.IsValid(n)))).Sum();
            Console.WriteLine(sum);
        }


        public void Solve2()
        {
            var validTickets = NearbyTickets.Where(a => a.IsValid(Rules)).ToList();
            var columns = validTickets.First().Numbers.Count();
            for (var i = 0; i < columns; i++)
            {
                for (var r = 0; r < Rules.Count(); r++)
                {
                    var rule = Rules[r];
                    var ruleValidForAllTickets = validTickets.All(a => rule.IsValid(a.Numbers[i]));
                    if (ruleValidForAllTickets)
                    {
                        Rules[r].Columns.Add(i);
                    }
                }
            }

            var doneRules = new List<Rule>();

            while (Rules.Any(r => r.Columns.Count() > 1))
            {
                var singleRule = Rules.FirstOrDefault(a => a.Columns.Count() == 1);
                doneRules.Add(singleRule);
                Rules.Remove(singleRule);
                Rules.ForEach(r => r.Columns.Remove(singleRule.Columns[0]));
            }

            var sum = 1D;
            foreach (var r in doneRules.Where(a => a.Name.StartsWith("departure")))
            {
                sum *= MyTicket.Numbers[r.Columns[0]];
            }

            Console.WriteLine(sum);
        }

        public class Ticket
        {
            public List<int> Numbers { get; set; }
            public bool IsValid(List<Rule> rules)
            {
                return Numbers.All(n => rules.Any(r => r.IsValid(n)));
            }
            public void Print()
            {
                Console.WriteLine(string.Join(",", Numbers));
            }
        }

        public class Rule
        {
            public string Name { get; set; }
            public List<(int Min, int Max)> Ranges { get; set; }
            public bool IsValid(int n)
            {
                return Ranges.Any(a => n >= a.Min && n <= a.Max);
            }

            public List<int> Columns { get; set; }
        }
    }
}