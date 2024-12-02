namespace Day2;

class Program
{
    static void Main(string[] args)
    {
        // Read all lines from input.txt and parse them into lists of integers
        var lines = File.ReadAllLines("./input.txt");
        var reports = lines.Select(line => line.Split(' ').Select(int.Parse).ToList()).ToList();

        int validReports = 0;

        foreach (var report in reports)
        {
            if (IsValidReport(report))
            {
                // Report is already valid
                validReports++;
            }
            else
            {
                // Attempt to remove each level one by one to apply the Problem Dampener
                bool madeValid = false;
                for (int i = 0; i < report.Count; i++)
                {
                    var modifiedReport = new List<int>(report);
                    modifiedReport.RemoveAt(i);

                    if (IsValidReport(modifiedReport))
                    {
                        validReports++;
                        madeValid = true;
                        break; // No need to check further removals
                    }
                }

                // If no single removal makes the report valid, it's unsafe
                if (!madeValid)
                {
                    // Optionally, handle unsafe reports if needed
                }
            }
        }

        Console.WriteLine($"Number of safe reports: {validReports}");
    }

    /// <summary>
    /// Checks if a report is valid based on the following rules:
    /// 1. All levels are either strictly increasing or strictly decreasing.
    /// 2. The difference between any two adjacent levels is at least 1 and at most 3.
    /// </summary>
    /// <param name="report">List of integer levels.</param>
    /// <returns>True if the report is valid; otherwise, false.</returns>
    static bool IsValidReport(List<int> report)
    {
        if (report.Count < 2)
        {
            // A single level is trivially valid
            return true;
        }

        bool isIncreasing = true;
        bool isDecreasing = true;

        for (int i = 1; i < report.Count; i++)
        {
            int diff = report[i] - report[i - 1];

            // Check difference constraints
            if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
            {
                return false;
            }

            if (diff <= 0)
            {
                isIncreasing = false;
            }

            if (diff >= 0)
            {
                isDecreasing = false;
            }

            // Early termination if neither increasing nor decreasing
            if (!isIncreasing && !isDecreasing)
            {
                return false;
            }
        }

        // The report is valid if it's either entirely increasing or entirely decreasing
        return isIncreasing || isDecreasing;
    }
}