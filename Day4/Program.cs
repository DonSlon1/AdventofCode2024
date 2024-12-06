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
                if (lines.Count < 3)
                {
                    Console.WriteLine("Input file does not contain enough lines to form an X-MAS pattern.");
                    return;
                }

                // Determine the maximum width of the grid
                int maxWidth = GetMaxWidth(lines);

                // Convert lines to a 2D grid for easier traversal
                char[,] grid = ConvertToGrid(lines, maxWidth);

                // Perform the X-MAS search and get the total count
                int totalXMas = CountXMasPatterns(grid);

                Console.WriteLine($"Total X-MAS patterns found: {totalXMas}");
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
                        grid[row, col] = char.ToUpper(currentLine[col]); // Convert to uppercase for consistency
                    else
                        grid[row, col] = '.'; // Use '.' as the placeholder for irrelevant characters
                }
            }

            return grid;
        }

        /// <summary>
        /// Counts the number of X-MAS patterns in the grid.
        /// </summary>
        private static int CountXMasPatterns(char[,] grid)
        {
            int count = 0;
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            // Iterate through each cell that can be the center of an X-MAS pattern
            for (int row = 1; row < height - 1; row++)
            {
                for (int col = 1; col < width - 1; col++)
                {
                    if (grid[row, col] == 'A') // Potential center of X-MAS
                    {
                        if (IsValidXMas(grid, row, col))
                            count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Checks if the given position is the center of a valid X-MAS pattern.
        /// </summary>
        private static bool IsValidXMas(char[,] grid, int row, int col)
        {
            // Define the relative positions for the diagonals
            // Diagonal 1: Top-Left to Bottom-Right
            (int dRow, int dCol)[] diag1 = { (-1, -1), (1, 1) };

            // Diagonal 2: Top-Right to Bottom-Left
            (int dRow, int dCol)[] diag2 = { (-1, 1), (1, -1) };

            // Check Diagonal 1
            bool diag1Valid = (IsMAS(grid, row, col, diag1[0].dRow, diag1[0].dCol, diag1[1].dRow, diag1[1].dCol));

            // Check Diagonal 2
            bool diag2Valid = (IsMAS(grid, row, col, diag2[0].dRow, diag2[0].dCol, diag2[1].dRow, diag2[1].dCol));

            // X-MAS is valid only if both diagonals have valid MAS/SAM sequences
            return diag1Valid && diag2Valid;
        }

        /// <summary>
        /// Checks if the characters at the specified diagonal positions form a MAS or SAM sequence.
        /// </summary>
        private static bool IsMAS(char[,] grid, int centerRow, int centerCol, int firstRowOffset, int firstColOffset, int secondRowOffset, int secondColOffset)
        {
            char firstChar = grid[centerRow + firstRowOffset, centerCol + firstColOffset];
            char secondChar = grid[centerRow + secondRowOffset, centerCol + secondColOffset];

            // Check for 'M' and 'S' in both possible orders
            return (firstChar == 'M' && secondChar == 'S') ||
                   (firstChar == 'S' && secondChar == 'M');
        }

        /// <summary>
        /// (Optional) Prints the grid to the console for debugging purposes.
        /// </summary>
        private static void PrintGrid(char[,] grid)
        {
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Console.Write(grid[row, col] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
