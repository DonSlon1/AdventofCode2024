namespace Day8;
class Program
{
    // Struct to represent a position on the grid
    struct Position
    {
        public int Row { get; }
        public int Col { get; }

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        // Override Equals for correct HashSet behavior
        public override bool Equals(object obj)
        {
            if (obj is Position)
            {
                Position other = (Position)obj;
                return this.Row == other.Row && this.Col == other.Col;
            }
            return false;
        }

        // Override GetHashCode for correct HashSet behavior
        public override int GetHashCode()
        {
            return Row * 397 ^ Col;
        }
    }

    static void Main(string[] args)
    {
        // Read the map input from the file
        List<string> map = File.ReadAllLines("input.txt").ToList();

        if (map.Count == 0)
        {
            Console.WriteLine("0");
            return;
        }

        int numRows = map.Count;
        int numCols = 0;
        foreach (var row in map)
        {
            if (row.Length > numCols)
                numCols = row.Length;
        }

        // Identify all antennas
        // Dictionary: Key = frequency, Value = list of Positions
        Dictionary<char, List<Position>> frequencyAntennas = new Dictionary<char, List<Position>>();

        for (int row = 0; row < numRows; row++)
        {
            string currentRow = map[row];
            for (int col = 0; col < currentRow.Length; col++)
            {
                char cell = currentRow[col];
                if (cell != '.')
                {
                    if (!frequencyAntennas.ContainsKey(cell))
                        frequencyAntennas[cell] = new List<Position>();
                    frequencyAntennas[cell].Add(new Position(row, col));
                }
            }
        }

        // Set to store unique antinode positions
        HashSet<Position> antinodes = new HashSet<Position>();

        // Process each frequency group
        foreach (var kvp in frequencyAntennas)
        {
            char frequency = kvp.Key;
            List<Position> antennas = kvp.Value;

            if (antennas.Count < 2)
                continue; // Need at least two antennas to form antinodes

            // Iterate through all unique pairs of antennas
            for (int i = 0; i < antennas.Count; i++)
            {
                for (int j = i + 1; j < antennas.Count; j++)
                {
                    Position antennaA = antennas[i];
                    Position antennaB = antennas[j];

                    // Calculate deltaRow and deltaCol
                    int deltaRow = antennaB.Row - antennaA.Row;
                    int deltaCol = antennaB.Col - antennaA.Col;

                    // Calculate GCD of deltaRow and deltaCol
                    int gcd = GCD(Math.Abs(deltaRow), Math.Abs(deltaCol));

                    if (gcd == 0)
                        continue; // Same position, skip

                    // Calculate stepRow and stepCol
                    int stepRow = deltaRow / gcd;
                    int stepCol = deltaCol / gcd;

                    // Enumerate all positions on the line in both directions
                    // Starting from antennaA
                    EnumerateLine(antennaA, stepRow, stepCol, numRows, numCols, antinodes);

                    // Starting from antennaB (other direction)
                    EnumerateLine(antennaB, -stepRow, -stepCol, numRows, numCols, antinodes);
                }
            }
        }

        // Count unique antinodes
        Console.WriteLine(antinodes.Count);
    }

    /// <summary>
    /// Enumerates all positions on a line starting from a given position,
    /// stepping by stepRow and stepCol, within the map bounds, and adds them to the antinodes set.
    /// </summary>
    /// <param name="start">Starting Position.</param>
    /// <param name="stepRow">Row step direction.</param>
    /// <param name="stepCol">Column step direction.</param>
    /// <param name="numRows">Total number of rows in the map.</param>
    /// <param name="numCols">Total number of columns in the map.</param>
    /// <param name="antinodes">HashSet to store unique antinodes.</param>
    static void EnumerateLine(Position start, int stepRow, int stepCol, int numRows, int numCols, HashSet<Position> antinodes)
    {
        int currentRow = start.Row;
        int currentCol = start.Col;

        while (true)
        {
            // Move one step
            currentRow += stepRow;
            currentCol += stepCol;

            // Check bounds
            if (currentRow < 0 || currentRow >= numRows || currentCol < 0 || currentCol >= numCols)
                break;

            // Add to antinodes
            Position pos = new Position(currentRow, currentCol);
            antinodes.Add(pos);
        }
    }

    /// <summary>
    /// Calculates the Greatest Common Divisor (GCD) of two integers using the Euclidean algorithm.
    /// </summary>
    /// <param name="a">First integer.</param>
    /// <param name="b">Second integer.</param>
    /// <returns>GCD of a and b.</returns>
    static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}
