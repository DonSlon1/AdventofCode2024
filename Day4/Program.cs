using System;
using System.Collections.Generic;
using System.IO;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Read all lines from the input file
                List<string> lines = new List<string>(File.ReadAllLines("./input.txt"));

                // Validate that there's content to process
                if (lines.Count == 0)
                {
                    Console.WriteLine("Input file is empty.");
                    return;
                }

                // Determine the maximum width of the grid
                int maxWidth = GetMaxWidth(lines);

                // Convert lines to a 2D grid for easier traversal
                char[,] grid = ConvertToGrid(lines, maxWidth);

                // Words to search for
                string[] patterns = { "XMAS", "SAMX" };

                // Perform the search and get the total count
                int totalOccurrences = SearchGrid(grid, patterns);

                Console.WriteLine($"Total occurrences of 'XMAS' and 'SAMX' is: {totalOccurrences}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The file 'input.txt' was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Determines the maximum width among all lines.
        /// </summary>
        private static int GetMaxWidth(List<string> lines)
        {
            int max = 0;
            foreach (string line in lines)
            {
                if (line.Length > max)
                    max = line.Length;
            }
            return max;
        }

        /// <summary>
        /// Converts the list of strings into a 2D character grid.
        /// Pads shorter lines with spaces to ensure uniform width.
        /// </summary>
        private static char[,] ConvertToGrid(List<string> lines, int maxWidth)
        {
            int height = lines.Count;
            int width = maxWidth;
            char[,] grid = new char[height, width];

            for (int row = 0; row < height; row++)
            {
                string currentLine = lines[row];
                for (int col = 0; col < width; col++)
                {
                    if (col < currentLine.Length)
                        grid[row, col] = currentLine[col];
                    else
                        grid[row, col] = ' '; // Padding with space
                }
            }

            return grid;
        }

        /// <summary>
        /// Searches the grid for specified patterns in all four directions.
        /// </summary>
        private static int SearchGrid(char[,] grid, string[] patterns)
        {
            int count = 0;
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);
            int patternLength = patterns[0].Length; // Assuming all patterns are the same length

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    foreach (string pattern in patterns)
                    {
                        // Check Horizontal (Left to Right)
                        if (col + patternLength <= width)
                        {
                            if (MatchPattern(grid, row, col, 0, 1, pattern))
                                count++;
                        }

                        // Check Vertical (Top to Bottom)
                        if (row + patternLength <= height)
                        {
                            if (MatchPattern(grid, row, col, 1, 0, pattern))
                                count++;
                        }

                        // Check Diagonal (Top-Left to Bottom-Right)
                        if (row + patternLength <= height && col + patternLength <= width)
                        {
                            if (MatchPattern(grid, row, col, 1, 1, pattern))
                                count++;
                        }

                        // Check Diagonal (Top-Right to Bottom-Left)
                        if (row + patternLength <= height && col - patternLength + 1 >= 0)
                        {
                            if (MatchPattern(grid, row, col, 1, -1, pattern))
                                count++;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Checks if a pattern matches starting from (row, col) in the specified direction.
        /// </summary>
        private static bool MatchPattern(char[,] grid, int row, int col, int rowDir, int colDir, string pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (grid[row, col] != pattern[i])
                    return false;

                row += rowDir;
                col += colDir;
            }
            return true;
        }
    }
}
