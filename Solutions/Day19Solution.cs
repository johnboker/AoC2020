using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day19Solution : ISolution
    {
        List<Rule> Rules { get; set; }
        List<string> Messages { get; set; }

        public async Task ReadInput(string file)
        {
            var lines = (await File.ReadAllLinesAsync(file));

            var i = 0;
            Rules = new List<Rule>();
            Messages = new List<string>();

            var m = false;

            foreach (var line in lines)
            {
                if (!m)
                {
                    var parts = line.Split(":");
                    var r = parts[1].Trim();

                    var rule = new Rule
                    {
                        Number = int.Parse(parts[0]),
                        Definition = parts[1].Trim(' ', '"')
                    };

                    Rules.Add(rule);
                }
                else
                {
                    Messages.Add(line);
                }

                i++;

                if (i < lines.Length && lines[i] == "")
                {
                    i++;
                    m = true;
                }
            }

            Rules = Rules.OrderBy(a => a.Number).ToList();
        }
        public void Solve1()
        {
            var regexRule = $"^{Rules[0].Resolve(Rules)}$";
            var regex = new Regex(regexRule);
            var cnt = 0;
            foreach (var message in Messages)
            {
                var isMatch = regex.IsMatch(message);
                cnt += isMatch ? 1 : 0;
                //Console.WriteLine($"{message} {(isMatch ? "match" : "")} ");
            }

            Console.WriteLine(cnt);
        }
        public void Solve2()
        {
            Rules[8].Definition = "42 | 42 8";
            Rules[11].Definition = "42 31 | 42 11 31";

            var regexRule = $"^{Rules[0].Resolve(Rules)}$";
            var regex = new Regex(regexRule);
            var cnt = 0;
            foreach (var message in Messages)
            {
                var isMatch = regex.IsMatch(message);
                cnt += isMatch ? 1 : 0;
                //Console.WriteLine($"{message} {(isMatch ? "match" : "")} ");
            }

            Console.WriteLine(cnt);
        }

        public class Rule
        {
            public int Number { get; set; }
            public string Definition { get; set; }

            public string Resolve(List<Rule> rules, int depth = 0)
            {
                if (depth == 15) return "";
                if (Definition == "a" || Definition == "b") return Definition;

                if (Definition.Contains("|"))
                {
                    var parts = Definition.Split("|");
                    var d1 = parts[0].Trim().Split(" ").Select(a => int.Parse(a)).Select(a => rules[a].Resolve(rules, depth + 1));
                    var d2 = parts[1].Trim().Split(" ").Select(a => int.Parse(a)).Select(a => rules[a].Resolve(rules, depth + 1));

                    return $"(?:{string.Join("", d1)}|{string.Join("", d2)})";
                }
                else
                {
                    var numbers = Definition.Split(" ").Select(a => int.Parse(a));
                    var resolvedNumbers = numbers.Select(a => rules[a].Resolve(rules, depth + 1));
                    return $"({string.Join("", resolvedNumbers)})";
                }
            }
        }
    }
}