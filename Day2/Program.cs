using System.Diagnostics;

namespace Day2;

class Program
{
    static void Main(string[] args)
    {
        List<string> lines = File.ReadAllLines("./input.txt").ToList();
        List<LinkedList<int>> reports = lines.Select(e => new LinkedList<int>(e.Split(' ').ToList().Select(int.Parse).ToList())).ToList();
        int validReports = 0;
        foreach (LinkedList<int> report in reports)
        {
            bool isValid = true;

            if (report.First == null)
            {
                continue;
            }
            
            LinkedListNode<int>? currentNode = report.First;
            while (currentNode != null)
            {
                var previousNode = currentNode.Previous;
                if (previousNode == null)
                {
                    currentNode = currentNode.Next;
                    continue;
                }
                    
                int dif = int.Abs(previousNode.Value - currentNode.Value);
                if (dif is < 1 or > 3)
                {
                    isValid = false;
                    break;
                }

                LinkedListNode<int>? nextNode = currentNode.Next;
                if (nextNode == null)
                {
                    break;
                }

                if (
                     !(previousNode.Value < currentNode.Value && currentNode.Value < nextNode.Value) &&
                     !(previousNode.Value > currentNode.Value && currentNode.Value > nextNode.Value)
                )
                {
                    isValid = false;
                    break;
                }
                currentNode = nextNode;
            
            }

            if (isValid)
            {
                validReports++;
            }
        }
        Console.WriteLine($"Number of valid reports {validReports}");
    }
}