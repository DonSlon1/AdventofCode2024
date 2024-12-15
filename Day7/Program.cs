namespace Day7;

class Program
{
    static void Main(string[] args)
    {
        List<string> lines = File.ReadAllLines("input.txt").ToList();
        UInt64 result = 0;
        foreach (string line in lines)
        {
            Console.WriteLine(line);
            UInt64 total = UInt64.Parse(line.Split(':')[0]);
            List<UInt64> numbers = line.Split(": ")[1].Split(' ').ToList().Select(UInt64.Parse).ToList();
            List<List<char>> combinations = GetPermutationsWithRepetition(['+', '*', '|'], numbers.Count - 1);
            foreach (List<char> combination in combinations)
            {
                
                UInt64 combinedSum = numbers[0];
                for (int i = 1; i < numbers.Count; i++)
                {
                    if (combination[i-1] == '*')
                    {
                        combinedSum *= numbers[i];
                    }
                    else if (combination[i-1] == '+')
                    {
                        combinedSum += numbers[i];
                    }
                    else
                    {
                        combinedSum = UInt64.Parse($"{combinedSum}{numbers[i]}");
                    }
                }

                if (combinedSum == total)
                {
                    result += total;
                    break;
                }
            }
        }
        Console.WriteLine($"Total sum is: {result}");
    }
    
    public static List<List<T>> GetPermutationsWithRepetition<T>(List<T> elements, int k)
    {
        List<List<T>> results = new List<List<T>>();
        if (k <= 0)
            return results;

        // Start the recursive generation
        GeneratePermutations(elements, k, new List<T>(), results);
        return results;
    }

    private static void GeneratePermutations<T>(
        List<T> elements,
        int k,
        List<T> currentPermutation,
        List<List<T>> results)
    {
        if (currentPermutation.Count == k)
        {
            // Make a copy of the current permutation and add it to the results
            results.Add(new List<T>(currentPermutation));
            return;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            // Include the element at index i
            currentPermutation.Add(elements[i]);
            // Recursively build the permutation
            GeneratePermutations(elements, k, currentPermutation, results);
            // Exclude the last element to backtrack
            currentPermutation.RemoveAt(currentPermutation.Count - 1);
        }
    }
}