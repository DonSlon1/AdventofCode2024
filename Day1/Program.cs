namespace Day1;

class Program
{
    static void Main(string[] args)
    {
        List<string> lines = File.ReadAllLines("./input.txt").ToList();
        List<List<int>> list = lines.Select(e => e.Split("   ").ToList().Select(element => int.Parse(element)).ToList())
            .ToList();
        List<int> listOne = new List<int>();
        List<int> listTwo = new List<int>();
        list.ForEach(e =>
        {
            listOne.Add(e[0]);
            listTwo.Add(e[1]);
        });
        listOne.Sort();
        listTwo.Sort();
        int dif = 0;
        for (int i = 0; i < listOne.Count; i++)
        {
            dif += int.Abs(listOne[i] - listTwo[i]);
        }

        Console.WriteLine($"Dif: {dif}");

        Dictionary<int, int> existingIds = new Dictionary<int, int>();
        listTwo.ForEach(e => {
            if (!existingIds.TryAdd(e, 1))
            {
                existingIds[e]++;
            }
        });
        int similarityScore = 0;
        listOne.ForEach(e => {
            if (existingIds.ContainsKey(e))
            {
                similarityScore += e * existingIds.GetValueOrDefault(e,0);
            }
        });
        Console.WriteLine($"Similarity Score: {similarityScore}");
    }
}
