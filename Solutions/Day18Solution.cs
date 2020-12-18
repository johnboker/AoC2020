using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day18Solution : ISolution
    {
        List<string> Expressions { get; set; }
        public async Task ReadInput(string file)
        {
            Expressions = (await File.ReadAllLinesAsync(file)).ToList();
        }
        public void Solve1()
        {
            var calculator = new Calculator();
            decimal sum = 0;
            foreach (var expression in Expressions)
            {
                var n = calculator.Solve1(expression);
                sum += n;
                Console.WriteLine($"{expression} = {n}");
            }
            Console.WriteLine(sum);
        }
        public void Solve2()
        {
            var calculator = new Calculator();
            decimal sum = 0;
            foreach (var expression in Expressions)
            {
                var n = calculator.Solve2(expression);
                sum += n;
                Console.WriteLine($"{expression} = {n}");
            }
            Console.WriteLine(sum);
        }

        public class Calculator
        {


            public ulong Solve1(string expression)
            {
                return Convert.ToUInt64(Reduce(expression));
            }

            public ulong Solve2(string expression)
            {
                var arr = $"({expression})".ToList();
                var cnt = arr.Count();
                for (var i = 0; i < cnt; i++)
                {
                    if (arr[i] == '+')
                    {
                        var insertLeftAt = FindPreviousParenInsertLocation(i, arr);
                        var insertRightAt = FindNextParenInsertLocation(i, arr);

                        arr.Insert(insertLeftAt, '(');
                        if (insertRightAt >= arr.Count())
                        {
                            arr.Append(')');
                        }
                        else
                        {
                            arr.Insert(insertRightAt, ')');
                        }
                        cnt += 2;
                        i += 1;
                    }
                }

                return Convert.ToUInt64(Reduce(new string(arr.ToArray())));
            }

            public int FindPreviousParenInsertLocation(int start, List<char> arr)
            {
                var pCount = 0;
                for (var i = start; i >= 0; i--)
                {
                    if (arr[i] == ' ') continue;
                    else if (arr[i] == ')') pCount++;
                    else if (arr[i] == '(') pCount--;
                    else if (arr[i] >= '0' && arr[i] <= '9' && pCount == 0)
                    {
                        return i;
                    }
                    if (pCount == 0 && arr[i] == '(') return i;
                }
                return -1;
            }

            public int FindNextParenInsertLocation(int start, List<char> arr)
            {
                var pCount = 0;
                for (var i = start; i < arr.Count(); i++)
                {
                    if (arr[i] == ' ') continue;
                    else if (arr[i] == ')') pCount++;
                    else if (arr[i] == '(') pCount--;
                    else if (arr[i] >= '0' && arr[i] <= '9' && pCount == 0)
                    {
                        return i + 2;
                    }
                    if (pCount == 0 && arr[i] == ')') return i + 2;
                }
                return -1;
            }

            public string Reduce(string expression)
            {
                ulong currentValue = 0;
                var op = ".";
                for (var i = 0; i < expression.Length; i++)
                {
                    var c = expression[i].ToString();
                    if (c == " ")
                    {
                        continue;
                    }
                    else if (c == "+" || c == "*")
                    {
                        op = c;
                        continue;
                    }

                    if (c == "(")
                    {
                        var end = FindMatchingParen(i, expression);
                        var subExpression = expression[(i + 1)..end];
                        i = end;
                        c = Reduce(subExpression);
                    }

                    var n = Convert.ToUInt64(c);
                    if (op == ".")
                    {
                        currentValue = n;
                    }
                    else if (op == "+")
                    {
                        currentValue += n;
                    }
                    else if (op == "*")
                    {
                        currentValue *= n;
                    }
                }
                return currentValue.ToString();
            }

            public int FindMatchingParen(int start, string expression)
            {
                var pcount = 0;
                for (var i = start; i < expression.Length; i++)
                {
                    if (expression[i] == ' ') continue;
                    else if (expression[i] == '(') pcount++;
                    else if (expression[i] == ')') pcount--;

                    if (pcount == 0)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }
    }
}