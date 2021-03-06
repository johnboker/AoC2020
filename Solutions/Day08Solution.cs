﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day08Solution : ISolution
    {
        public Computer Computer1 { get; set; }
        public async Task ReadInput(string file)
        {
            Computer1 = new Computer();
            var input = await File.ReadAllLinesAsync(file);
            var regex = new Regex(@"^(?<op>.*) (?<arg>[\-+]?[0-9]+)$");
            foreach (var i in input)
            {
                var m = regex.Matches(i)[0];
                var op = m.Groups["op"].Value;
                var arg = Convert.ToInt32(m.Groups["arg"].Value);
                Computer1.Instructions.Add(new Instruction()
                {
                    Operation = op,
                    Argument = arg
                });
            }
        }

        public void Solve1()
        {
            Computer1.Run();
            Console.WriteLine(Computer1.Accumulator);
        }

        public void Solve2()
        {
            for (var i = 0; i < Computer1.Instructions.Count(); i++)
            {
                if (SwapJmpNop(Computer1.Instructions[i]))
                {
                    if (Computer1.Run() == 0)
                    {
                        Console.WriteLine(Computer1.Accumulator);
                        break;
                    }
                    SwapJmpNop(Computer1.Instructions[i]);
                }
            }
        }

        private bool SwapJmpNop(Instruction instruction)
        {
            if (instruction.Operation == "jmp" || instruction.Operation == "nop")
            {
                instruction.Operation = instruction.Operation == "jmp" ? "nop" : "jmp";
                return true;
            }
            return false;
        }

        public class Computer
        {
            public Computer()
            {
                Instructions = new List<Instruction>();
            }

            public int PC { get; set; }
            public int Accumulator { get; set; }
            public List<Instruction> Instructions { get; set; }

            public void Reset()
            {
                Accumulator = 0;
                PC = 0;
                Instructions.ForEach(i => i.VisitCount = 0);
            }

            public int Run()
            {
                Reset();
                while (PC < Instructions.Count())
                {
                    var instruction = Instructions[PC];
                    instruction.VisitCount++;

                    if (instruction.VisitCount > 1)
                    {
                        return PC - Instructions.Count();
                    }

                    switch (instruction.Operation)
                    {
                        case "acc":
                            Accumulator += instruction.Argument;
                            break;

                        case "jmp":
                            PC += instruction.Argument;
                            continue;

                        case "nop":
                            break;
                    }

                    PC++;
                }
                return 0;
            }
        }

        public class Instruction
        {
            public int VisitCount { get; set; }
            public string Operation { get; set; }
            public int Argument { get; set; }
        }
    }
}
