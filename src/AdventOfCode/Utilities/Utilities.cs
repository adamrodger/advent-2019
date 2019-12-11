namespace AdventOfCode.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Utilities
    {
        public static T[] Numbers<T>(this string input)
        {
            MatchCollection matches = Regex.Matches(input, @"-?\d+");
            return matches.Cast<Match>().Select(m => m.Value).Select(m => (T)Convert.ChangeType(m, typeof(T))).ToArray();
        }

        public static void ForEach<T>(this T[,] grid, Action<T> cellAction, Action lineAction = null)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    cellAction(grid[y, x]);
                }

                lineAction?.Invoke();
            }
        }

        public static void ForEach<T>(this T[,] grid, Action<int, int, T> action)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    action(x, y, grid[y, x]);
                }
            }
        }

        public static void ForEachChar(this string[] input, Action<int, int, char> cellAction)
        {
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    cellAction(x, y, input[y][x]);
                }
            }
        }

        public static IEnumerable<TResult> Select<T, TResult>(this T[,] grid, Func<T, TResult> func)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    T item = grid[y, x];

                    yield return func(item);
                }
            }
        }

        public static IEnumerable<T> Search<T>(this T[,] grid, Predicate<T> predicate)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    T item = grid[y, x];

                    if (predicate(item))
                    {
                        yield return item;
                    }
                }
            }
        }

        public static IEnumerable<T> Where<T>(this T[,] grid, Func<int, int, T, bool> predicate)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    T item = grid[y, x];

                    if (predicate(x, y, item))
                    {
                        yield return item;
                    }
                }
            }
        }

        public static string Print<T>(this T[,] grid)
        {
            var builder = new StringBuilder(grid.GetLength(0) * (grid.GetLength(1) + Environment.NewLine.Length));
            grid.ForEach(cell => builder.Append(cell), () => builder.AppendLine());
            builder.AppendLine();

            string result = builder.ToString();
            Debug.Write(result);
            return result;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) where TValue : new()
        {
            if (!@this.TryGetValue(key, out TValue value))
            {
                value = new TValue();
                @this.Add(key, value);
            }

            return value;
        }

        public static IEnumerable<T> Adjacent4<T>(this T[,] grid, int x, int y)
        {
            if (y - 1 > 0)
            {
                yield return grid[y - 1, x];
            }

            if (x - 1 > 0)
            {
                yield return grid[y, x - 1];
            }

            if (x + 1 < grid.GetLength(1))
            {
                yield return grid[y, x + 1];
            }

            if (y + 1 < grid.GetLength(0))
            {
                yield return grid[y + 1, x];
            }
        }

        public static char[,] ToGrid(this string[] input)
        {
            // y,x remember, not x,y
            char[,] grid = new char[input.Length, input[0].Length];

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    grid[y, x] = input[y][x];
                }
            }

            return grid;
        }
    }
}
