using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day14Solution : ISolution
    {
        public List<Memory> Memories { get; set; }

        public async Task ReadInput(string file)
        {
            Memories = new List<Memory>();
            var lines = await File.ReadAllLinesAsync(file);
            var regex = new Regex(@"^mem\[(?<loc>[\d]+)\] = (?<val>[\d]+)$");
            var mask = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    mask = line[7..];
                }
                else
                {
                    var m = regex.Matches(line)[0];
                    var loc = Convert.ToUInt64(m.Groups["loc"].Value);
                    var val = Convert.ToUInt64(m.Groups["val"].Value);
                    var memory = new Memory
                    {
                        Mask = mask,
                        Address = loc,
                        ValueString = Convert.ToString((long)val, 2).PadLeft(36, '0'),
                        AddressString = Convert.ToString((long)loc, 2).PadLeft(36, '0')
                    };
                    Memories.Add(memory);
                }
            }
        }

        public void Solve1()
        {
            var memory = new Dictionary<decimal, string>();
            Memories.ForEach(m => memory[m.Address] = m.ApplyMask1());
            Console.WriteLine(memory.Sum(a => (decimal)Convert.ToUInt64(a.Value, 2)));
        }

        public void Solve2()
        {
            var memory = new Dictionary<decimal, string>();
            Memories.ForEach(m => Expand(m.ApplyMask2()).ForEach(a => memory[Convert.ToUInt64(a, 2)] = m.ValueString));
            Console.WriteLine(memory.Sum(a => (decimal)Convert.ToUInt64(a.Value, 2)));
        }

        public static List<string> Expand(string bits)
        {
            var result = new List<string>() { bits };
            int i = 0;
            while (i < result.Count())
            {
                var s = result[i];
                var idx = s.IndexOf('X');
                if (idx >= 0)
                {
                    var a = s.ToArray();
                    a[idx] = '0';
                    result[i] = new string(a);
                    a[idx] = '1';
                    result.Add(new string(a));
                    i--;
                }
                i++;
            }
            return result;
        }
    }

    public class Memory
    {
        public string Mask { get; set; }
        public decimal Address { get; set; }
        public string AddressString { get; set; }
        public string ValueString { get; set; }

        public string ApplyMask2()
        {
            return new string(Mask.Select((c, i) => c == '0' ? AddressString[i] : c == '1' ? '1' : 'X').ToArray());
        }

        public string ApplyMask1()
        {
            return new string(Mask.Select((c, i) => c == 'X' ? ValueString[i] : c == '1' ? '1' : '0').ToArray());
        }
    }
}