using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AoC2020.Solutions;

namespace AoC2020
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var day = DateTime.Now.Day;

            if (args.Length > 0)
            {
                day = Convert.ToInt32(args[0]);
            }

            var file = $"input/day{day:00}.txt";

            if (args.Length > 1)
            {
                file = $"input/{args[1]}";
                if (!File.Exists(file))
                {
                    Console.WriteLine($"Input file not found ({file})");
                    return;
                }
            }

            var solution = CreateSolutionForDay(day);

            if (solution == null)
            {
                Console.WriteLine($"Day solution not found ({day})");
                return;
            }

            await solution.ReadInput(file);

            Console.WriteLine("Part 1: \n");
            var ms1 = BenchmarkTime(solution.Solve1);
            Console.WriteLine($"\nElapsed Time: {ms1} ms\n");

            Console.WriteLine("Part 2: \n");
            var ms2 = BenchmarkTime(solution.Solve2);
            Console.WriteLine($"\nElapsed Time: {ms2} ms\n");
        }

        public static double BenchmarkTime(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        private static ISolution CreateSolutionForDay(int day)
        {
            var className = $"Day{day:00}Solution";
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == className);
            return type == null ? null : Activator.CreateInstance(type) as ISolution;
        }
    }
}
