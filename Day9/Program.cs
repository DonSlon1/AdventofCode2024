using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9 
{
    class Program
    {
        // Represents a file on the disk
        class FileEntry
        {
            public int Id { get; set; } 
            public int StartIndex { get; set; }
            public int Length { get; set; }

            public FileEntry(int id, int start, int length)
            {
                Id = id;
                StartIndex = start;
                Length = length;
            }
        }

        static void Main(string[] args)
        {
            // Read the disk map from input.txt
            string diskMap = File.ReadAllText("input.txt").Trim();

            // Parse the disk map into a list of blocks
            List<int> diskBlocks = ParseDiskMap(diskMap);

            // Initialize the list of files on the disk
            List<FileEntry> files = IdentifyFiles(diskBlocks);

            // Move the files according to Part Two's requirements
            MoveFiles(diskBlocks, files);

            // Calculate the checksum of the final disk state
            long checksum = CalculateChecksum(diskBlocks);

            // Output the checksum
            Console.WriteLine($"Filesystem Checksum: {checksum}");
        }

        static List<int> ParseDiskMap(string diskMap)
        {
            List<int> blocks = new List<int>();
            int currentFileId = 0;

            for (int i = 0; i < diskMap.Length; i++)
            {
                char c = diskMap[i];
                if (!char.IsDigit(c))
                {
                    throw new InvalidDataException($"Invalid character '{c}' in disk map.");
                }

                int runLength = c - '0'; // Convert char digit to integer

                if (i % 2 == 0)
                {
                    // Even index: File run
                    for (int j = 0; j < runLength; j++)
                    {
                        blocks.Add(currentFileId);
                    }
                    currentFileId++;
                }
                else
                {
                    // Odd index: Free space run
                    for (int j = 0; j < runLength; j++)
                    {
                        blocks.Add(-1);
                    }
                }
            }

            return blocks;
        }

        static List<FileEntry> IdentifyFiles(List<int> blocks)
        {
            List<FileEntry> files = new List<FileEntry>();
            int currentFileId = -1;
            int startIndex = -1;
            int length = 0;

            for (int i = 0; i < blocks.Count; i++)
            {
                int block = blocks[i];

                if (block != -1)
                {
                    if (block != currentFileId)
                    {
                        // New file encountered
                        if (currentFileId != -1)
                        {
                            // Save the previous file
                            files.Add(new FileEntry(currentFileId, startIndex, length));
                        }

                        currentFileId = block;
                        startIndex = i;
                        length = 1;
                    }
                    else
                    {
                        // Continuing the current file
                        length++;
                    }
                }
                else
                {
                    if (currentFileId != -1)
                    {
                        // Free space encountered after a file
                        files.Add(new FileEntry(currentFileId, startIndex, length));
                        currentFileId = -1;
                        startIndex = -1;
                        length = 0;
                    }
                }
            }

            // Add the last file if the disk ends with a file
            if (currentFileId != -1)
            {
                files.Add(new FileEntry(currentFileId, startIndex, length));
            }

            return files;
        }

        static void MoveFiles(List<int> blocks, List<FileEntry> files)
        {
            // Sort files in descending order of File ID
            List<FileEntry> sortedFiles = files.OrderByDescending(f => f.Id).ToList();

            foreach (var file in sortedFiles)
            {
                // Attempt to move the file
                int targetIndex = FindLeftmostFreeSpan(blocks, file.Length);

                if (targetIndex != -1 && targetIndex < file.StartIndex)
                {
                    // Move the file to the target index
                    // Set current blocks to -1
                    for (int i = file.StartIndex; i < file.StartIndex + file.Length; i++)
                    {
                        blocks[i] = -1;
                    }

                    // Set new blocks to file ID
                    for (int i = 0; i < file.Length; i++)
                    {
                        blocks[targetIndex + i] = file.Id;
                    }

                    // Update the file's new start index
                    file.StartIndex = targetIndex;
                }
                // Else: No suitable free span found to the left; do not move the file
            }
        }

        static int FindLeftmostFreeSpan(List<int> blocks, int fileLength)
        {
            for (int i = 0; i <= blocks.Count - fileLength; i++)
            {
                bool isFree = true;

                for (int j = 0; j < fileLength; j++)
                {
                    if (blocks[i + j] != -1)
                    {
                        isFree = false;
                        break;
                    }
                }

                if (isFree)
                {
                    return i;
                }
            }

            return -1; // No suitable free span found
        }

        static long CalculateChecksum(List<int> blocks)
        {
            long checksum = 0;

            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i] >= 0)
                {
                    checksum += (long)i * blocks[i];
                }
            }

            return checksum;
        }
    }
}
