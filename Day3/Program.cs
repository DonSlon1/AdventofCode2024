using System.Collections;
using System.Text.RegularExpressions;

namespace Day3;

class Program
{
    static void Main(string[] args)
    {
        string lines = File.ReadAllText("./input.txt");
        var match = Regex.Match(lines, "mul\\(\\d{1,3},\\d{1,3}\\)");
        int sum = 0;
        int endOfLast = 0;
        while (match.Success)
        {
            String sub = lines.Substring(0,match.Index);
            MatchCollection subMatch = Regex.Matches(sub, "(?:don't|do)\\(\\)");
            if (subMatch.Count >= 1)
            {
                Match lastMatch = subMatch.Last();
                if (lastMatch.Success)
                {
                    if (lastMatch.Value.Contains("don't()"))
                    {
                        endOfLast = match.Index + match.Length; 
                        match = match.NextMatch();
                        continue;
                    }
                }
            }
            
            Match digits = Regex.Match(match.Value, "\\d{1,3}");
            int digitOne = int.Parse(digits.Value);
            digits = digits.NextMatch();
            int digitTwo = int.Parse(digits.Value);
            sum += digitOne * digitTwo;
            endOfLast = match.Index + match.Length; 
            match = match.NextMatch();
        }
        Console.WriteLine($"Total sum is: {sum}");
    }
}