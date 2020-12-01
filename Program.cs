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
            Invoke(() => solution.Solve1(), out var timeSpan1);
            Console.WriteLine($"\nElapsed Time: {timeSpan1.TotalMilliseconds} ms\n");

            Console.WriteLine("Part 2: \n");
            Invoke(() => solution.Solve2(), out var timeSpan2);
            Console.WriteLine($"\nElapsed Time: {timeSpan2.TotalMilliseconds} ms\n");
        }

        public static void Invoke(Action action, out TimeSpan timeSpan)
        {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            timeSpan = stopwatch.Elapsed;
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
