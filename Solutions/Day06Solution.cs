using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day06Solution : ISolution
    {
        private List<Group> Groups { get; set; }

        public async Task ReadInput(string file)
        {
            var lines = (await File.ReadAllLinesAsync(file));
            var g = new Group();
            Groups = new List<Group> { g };
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    g = new Group();
                    Groups.Add(g);
                    continue;
                }
                g.Answers.Add(line);
            }

        }

        public void Solve1()
        {
            var cnt = Groups.Select(a => a.Answers.SelectMany(a => a).Distinct().Count()).Sum();
            Console.WriteLine(cnt);
        }

        public void Solve2()
        {
            var cnt = 0;
            foreach (var g in Groups)
            {
                var c = (from a in g.Answers.SelectMany(p => p)
                         group a by a into h
                         select new
                         {
                             Queston = h.Key,
                             Count = h.Count()
                         }).Where(m => m.Count == g.Answers.Count()).Count();
                cnt += c;
            }

            Console.WriteLine(cnt);
        }
    }

    public class Group
    {
        public Group()
        {
            Answers = new List<string>();
        }
        public List<string> Answers { get; set; }
    }
}
