namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputLine = File.ReadAllLines("input.txt").First();
            List<int> diskmap = inputLine.Select(c => int.Parse(c.ToString())).ToList();

            List<int> fileblocks = new List<int>();
            int freespace = 0;
            int id = 0;

            bool isEmpty = false;
            foreach (var digit in diskmap)
            {
                if (isEmpty)
                {
                    for (int j = 0; j < digit; j++)
                        fileblocks.Add(-1);
                }
                else
                {
                    for (int j = 0; j < digit; j++)
                        fileblocks.Add(id);
                    id++;
                }

                isEmpty = !isEmpty; // Toggle between filled and empty states
            }

            int FreeSpaceIndex(int maxIndex)
            {
                for (int i = freespace; i < maxIndex; i++)
                {
                    if (fileblocks[i] == -1)
                        return i;
                }
                return -1;
            }

            for (int i = fileblocks.Count - 1; i >= 0; i--)
            {
                if (fileblocks[i] >= 0)
                {
                    int freespaceIndex = FreeSpaceIndex(i);
                    if (freespaceIndex < 0)
                        break; // No available free space found

                    fileblocks[freespaceIndex] = fileblocks[i];
                    fileblocks[i] = -1;

                    freespace = freespaceIndex + 1;
                }
            }

            long checksum = 0;
            for (int i = 0; i < fileblocks.Count; i++)
            {
                if (fileblocks[i] == -1)
                    break; // Stop at the first free space

                checksum += (long)fileblocks[i] * i;
            }

            Console.WriteLine($"Checksum: {checksum}");
        }
    }
}
