namespace Day5;

class Program
{
    private static Dictionary<int, List<int>?> rules = new Dictionary<int, List<int>?>();
    static void Main(string[] args)
    {
        List<string> inputLines = File.ReadAllLines("./input.txt").ToList();
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
        int invalidSum = 0;
        foreach (List<int> page in pages)
        {
            var valid = IsValid(page);
            if (valid.isValid)
            {
                sum += page[page.Count / 2];
            }
            else if (valid.invalidItem.HasValue)
            {
                var correctPage = Correction(valid.invalidItem.Value,page);
                invalidSum += correctPage[correctPage.Count / 2];
            }
        }
        Console.WriteLine($"total sum of valid pages is: {sum}");
        Console.WriteLine($"total sum of invalid items is: {invalidSum}");
    }

    static List<int> Correction((int invalid, int invaliRuleIndex) wrongItem, List<int> incorrectPage)
    {
        var item = incorrectPage[wrongItem.invalid];
        incorrectPage.RemoveAt(wrongItem.invalid);
        incorrectPage.Insert(wrongItem.invaliRuleIndex,item);
        var valid = IsValid(incorrectPage);
        if (valid.isValid)
        {
            return incorrectPage;
        }

        return Correction(valid.invalidItem.Value, incorrectPage);
    }

    static (bool isValid,(int invalidPageNumberIndex,int invaliRuleIndex)? invalidItem) IsValid(List<int> page)
    {
        bool valid = true;
        int invalidRuleIndex = int.MaxValue;
        foreach (var pageNumber in page.Select((value,i) => (value,i)))
        {
            if (!rules.TryGetValue(pageNumber.value,out List<int>? rule))
            {
                continue;
            }
            
            rule?.ForEach(e =>
            {
                var ruleIndex = page.FindIndex(v => v == e);
                if (pageNumber.i >= ruleIndex && ruleIndex != -1)
                {
                    if (ruleIndex <= invalidRuleIndex && invalidRuleIndex != -1)
                    {
                        invalidRuleIndex = ruleIndex;
                    }
                    valid = false;
                }
            });
            if (!valid)
            {
                return (false, (pageNumber.i,invalidRuleIndex));
            }
        }

        return (true,null);
    }
}