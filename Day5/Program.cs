namespace Day5;

class Program
{
    static void Main(string[] args)
    {
        List<string> inputLines = File.ReadAllLines("./input.txt").ToList();
        Dictionary<int, List<int>?> rules = new Dictionary<int, List<int>?>();
        inputLines.Slice(0, inputLines.FindIndex(x => x == "")).ForEach(s =>
        {
            string[] parts = s.Split('|');
            if (parts.Length != 2)
            {
                throw new FormatException($"Invalid rule format: {s}");
            }

            int first = int.Parse(parts[0]);
            int second = int.Parse(parts[1]);
            if (!rules.ContainsKey(first))
            {
                rules[first] = new List<int>();
            }
            rules[first].Add(second);
        });
        List<List<int>> pages = inputLines[(inputLines.FindIndex(x => x == "")+1)..].Select(e => e.Split(',').Select(value => int.Parse(value)).ToList()).ToList();
        Console.WriteLine("Pages");
        pages.ForEach(e => e.ForEach(Console.WriteLine));
        int sum = 0;
        foreach (List<int> page in pages)
        {
            bool valid = true;
            foreach (var pageNumber in page.Select((value,i) => (value,i)))
            {
                if (!rules.TryGetValue(pageNumber.value,out List<int>? rule))
                {
                    continue;
                }
                
                rule?.ForEach(e =>
                {
                    if (pageNumber.i >= page.FindIndex(v => v == e) && page.FindIndex(v => v == e) != -1)
                    {
                        valid = false;
                    }
                });
            }
            
            if (valid)
            {
                sum += page[page.Count / 2];
            }
        }
        Console.WriteLine($"total sum of valid pages is: {sum}");
    }
}