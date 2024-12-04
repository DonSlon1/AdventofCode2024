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
        while (match.Success)
        {
            Match digits = Regex.Match(match.Value, "\\d{1,3}");
            int digitOne = int.Parse(digits.Value);
            digits = digits.NextMatch();
            int digitTwo = int.Parse(digits.Value);
            sum += digitOne * digitTwo;
            match = match.NextMatch();
        }
        Console.WriteLine($"Total sum is: {sum}");
    }
}