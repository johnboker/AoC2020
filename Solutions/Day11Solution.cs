using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2020.Solutions
{
    public class Day11Solution : ISolution
    {
        public char[,] Seats { get; set; }
        public async Task ReadInput(string file)
        {
            var lines = await File.ReadAllLinesAsync(file);
            Seats = new char[lines.Count(), lines.First().Length];

            for (var r = 0; r < lines.Count(); r++)
            {
                var line = lines[r];
                for (var c = 0; c < line.Length; c++)
                {
                    Seats[r, c] = line[c];
                }
            }
        }

        private void PrintSeats(char[,] seats)
        {
            var occupied = 0;
            for (var r = 0; r < seats.GetLength(0); r++)
            {
                for (var c = 0; c < seats.GetLength(1); c++)
                {
                    if (seats[r, c] == '#') occupied++;
                    Console.Write(seats[r, c]);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Occupied: {occupied}\n");
        }

        private Bitmap CreateBitmap(char[,] seats)
        {
            var bitmap = new Bitmap(seats.GetLength(1), seats.GetLength(0));
            for (var r = 0; r < seats.GetLength(0); r++)
            {
                for (var c = 0; c < seats.GetLength(1); c++)
                {
                    var color = Color.Black;
                    if (seats[r, c] == 'L')
                    {
                        color = Color.Green;
                    }
                    else if (seats[r, c] == '#')
                    {
                        color = Color.Red;
                    }
                    bitmap.SetPixel(c, r, color);
                }
            }
            return bitmap;
        }

        private int CountOccupiedAdjacentSeats(char[,] seats, int r, int c)
        {
            var rMax = seats.GetLength(0);
            var cMax = seats.GetLength(1);
            var cnt = 0;

            if (r - 1 >= 0 && c - 1 >= 0 && seats[r - 1, c - 1] == '#') cnt++;
            if (r - 1 >= 0 && seats[r - 1, c] == '#') cnt++;
            if (r - 1 >= 0 && c + 1 < cMax && seats[r - 1, c + 1] == '#') cnt++;

            if (c - 1 >= 0 && seats[r, c - 1] == '#') cnt++;
            if (c + 1 < cMax && seats[r, c + 1] == '#') cnt++;

            if (r + 1 < rMax && c - 1 >= 0 && seats[r + 1, c - 1] == '#') cnt++;
            if (r + 1 < rMax && seats[r + 1, c] == '#') cnt++;
            if (r + 1 < rMax && c + 1 < cMax && seats[r + 1, c + 1] == '#') cnt++;

            return cnt;
        }


        private int CountOccupiedVisibleSeats(char[,] seats, int row, int col)
        {
            var rMax = seats.GetLength(0);
            var cMax = seats.GetLength(1);
            var cnt = 0;
            int c;

            // horizontal
            for (c = col + 1; c < cMax; c++)
            {
                if (seats[row, c] == 'L') break;
                if (seats[row, c] == '#')
                {
                    cnt++;
                    break;
                }
            }

            for (c = col - 1; c >= 0; c--)
            {
                if (seats[row, c] == 'L') break;
                if (seats[row, c] == '#')
                {
                    cnt++;
                    break;
                }
            }

            int r;
            // vertical
            for (r = row + 1; r < rMax; r++)
            {
                if (seats[r, col] == 'L') break;
                if (seats[r, col] == '#')
                {
                    cnt++;
                    break;
                }
            }

            for (r = row - 1; r >= 0; r--)
            {
                if (seats[r, col] == 'L') break;
                if (seats[r, col] == '#')
                {
                    cnt++;
                    break;
                }
            }


            //diagonal bottom right
            r = row + 1;
            c = col + 1;
            while (r < rMax && c < cMax)
            {
                if (seats[r, c] == 'L') break;
                if (seats[r, c] == '#')
                {
                    cnt++;
                    break;
                }
                r++;
                c++;
            }


            //diagonal bottom left
            r = row + 1;
            c = col - 1;
            while (r < rMax && c >= 0)
            {
                if (seats[r, c] == 'L') break;
                if (seats[r, c] == '#')
                {
                    cnt++;
                    break;
                }
                r++;
                c--;
            }

            //diagonal top left
            r = row - 1;
            c = col - 1;
            while (r >= 0 && c >= 0)
            {
                if (seats[r, c] == 'L') break;
                if (seats[r, c] == '#')
                {
                    cnt++;
                    break;
                }
                r--;
                c--;
            }

            //diagonal top left
            r = row - 1;
            c = col + 1;
            while (r >= 0 && c < cMax)
            {
                if (seats[r, c] == 'L') break;
                if (seats[r, c] == '#')
                {
                    cnt++;
                    break;
                }
                r--;
                c++;
            }

            return cnt;
        }

        public void Solve1()
        {
            var seats = (char[,])Seats.Clone();

            int changes;

            using var gif = AnimatedGif.AnimatedGif.Create("animation-part1.gif", 100);


            do
            {
                var next = (char[,])seats.Clone();

                changes = 0;
                for (var r = 0; r < Seats.GetLength(0); r++)
                {
                    for (var c = 0; c < Seats.GetLength(1); c++)
                    {
                        if (seats[r, c] == 'L')
                        {
                            var cnt = CountOccupiedAdjacentSeats(seats, r, c);
                            if (cnt == 0)
                            {
                                next[r, c] = '#';
                                changes++;
                            }
                        }

                        if (seats[r, c] == '#')
                        {
                            var cnt = CountOccupiedAdjacentSeats(seats, r, c);
                            if (cnt >= 4)
                            {
                                next[r, c] = 'L';
                                changes++;
                            }
                        }
                    }
                }
                var bitmap = CreateBitmap(seats);
                gif.AddFrame(bitmap, delay: -1, quality: AnimatedGif.GifQuality.Bit8);

                seats = next;
            }
            while (changes > 0);

            PrintSeats(seats);
        }

        public void Solve2()
        {
            var seats = (char[,])Seats.Clone();

            int changes;

            using var gif = AnimatedGif.AnimatedGif.Create("animation-part2.gif", 100);

            do
            {
                var next = (char[,])seats.Clone();

                changes = 0;
                for (var r = 0; r < Seats.GetLength(0); r++)
                {
                    for (var c = 0; c < Seats.GetLength(1); c++)
                    {
                        if (seats[r, c] == 'L')
                        {
                            var cnt = CountOccupiedVisibleSeats(seats, r, c);
                            if (cnt == 0)
                            {
                                next[r, c] = '#';
                                changes++;
                            }
                        }

                        if (seats[r, c] == '#')
                        {
                            var cnt = CountOccupiedVisibleSeats(seats, r, c);
                            if (cnt >= 5)
                            {
                                next[r, c] = 'L';
                                changes++;
                            }
                        }
                    }
                }

                var bitmap = CreateBitmap(seats);
                gif.AddFrame(bitmap, delay: -1, quality: AnimatedGif.GifQuality.Bit8);

                seats = next;
            }
            while (changes > 0);

            PrintSeats(seats);
        }
    }
}
