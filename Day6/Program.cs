namespace Day6;

// Define possible movement directions
public static class Directions
{
    public static readonly (int x, int y)[] Values = new (int x, int y)[]
    {
        (0, -1), // Up
        (1, 0),  // Right
        (0, 1),  // Down
        (-1, 0)  // Left
    };
}

// Enum to represent the type of the next move
public enum MoveType
{
    Collision,
    OutOfMap,
    Free,
    FreeOnExisting
}

// Class to simulate the guard's movement
public class GuardSimulator
{
    private readonly char[][] _map;
    private readonly (int x, int y) _startingPosition;
    private int _currentDirectionIndex; // Index in Directions.Values
    private (int x, int y) _currentPosition;
    private HashSet<(int x, int y, int direction)> _visitedStates;

    public GuardSimulator(char[][] map, (int x, int y) startingPosition)
    {
        // Deep copy of the map to prevent side effects
        _map = map.Select(row => row.ToArray()).ToArray();
        _startingPosition = startingPosition;
        Reset();
    }

    // Resets the simulator to the initial state
    public void Reset()
    {
        _currentPosition = _startingPosition;
        _currentDirectionIndex = 0; // Initially facing up
        _visitedStates = new HashSet<(int x, int y, int direction)>();
    }

    // Public property to get the current position
    public (int x, int y) CurrentPosition => _currentPosition;

    // Public method to turn right (90 degrees)
    public void TurnRight()
    {
        _currentDirectionIndex = (_currentDirectionIndex + 1) % Directions.Values.Length;
    }

    // Public method to move forward in the current direction
    public void MoveForward()
    {
        var direction = Directions.Values[_currentDirectionIndex];
        _currentPosition = (_currentPosition.x + direction.x, _currentPosition.y + direction.y);
    }

    // Public method to determine the type of the next move
    public MoveType DetermineNextMoveType()
    {
        var direction = Directions.Values[_currentDirectionIndex];
        int newX = _currentPosition.x + direction.x;
        int newY = _currentPosition.y + direction.y;

        if (newY < 0 || newY >= _map.Length || newX < 0 || newX >= _map[0].Length)
        {
            return MoveType.OutOfMap;
        }

        char nextCell = _map[newY][newX];
        if (nextCell == '.')
        {
            return MoveType.Free;
        }
        if (nextCell == 'X')
        {
            return MoveType.FreeOnExisting;
        }
        if (nextCell == '^' || nextCell == 'v' || nextCell == '<' || nextCell == '>')
        {
            return MoveType.FreeOnExisting;
        }
        if (nextCell == '#')
        {
            return MoveType.Collision;
        }

        throw new FormatException("Unsupported cell character encountered.");
    }

    // Simulates the guard's movement until it either exits the map or enters a loop
    public bool IsTrappedInLoop()
    {
        while (true)
        {
            // Record the current state
            var state = (_currentPosition.x, _currentPosition.y, _currentDirectionIndex);
            if (_visitedStates.Contains(state))
            {
                // Loop detected
                return true;
            }
            _visitedStates.Add(state);

            // Determine next move
            var nextMoveType = DetermineNextMoveType();

            if (nextMoveType == MoveType.Collision)
            {
                // Turn right
                TurnRight();
                continue; // Attempt to move in the new direction
            }
                
            if (nextMoveType == MoveType.Free || nextMoveType == MoveType.FreeOnExisting)
            {
                // Move forward
                MoveForward();
            }
            else if (nextMoveType == MoveType.OutOfMap)
            {
                // Guard exits the map
                return false;
            }
        }
    }
}

class Program
{
    private static char[][] _lines = null!;
    private static (int x, int y) _position;

    static void Main()
    {
        // Read and initialize the map
        _lines = File.ReadAllLines("input.txt")
            .Select(l => l.Select(e => e).ToArray()).ToArray();
        _position = FindGuard();
        // Optionally mark the starting position
        //_lines[_position.y][_position.x] = 'X';

        // Part One: Count unique positions visited
        int uniqueFieldsStand = CountUniquePositionsVisited(_lines, _position);
        Console.WriteLine($"Part One: Unique positions visited: {uniqueFieldsStand}");

        // Part Two: Find obstruction positions to trap the guard in a loop
        int validObstructionPositions = FindObstructionPositions(_lines, _position);
        Console.WriteLine($"Part Two: Valid obstruction positions: {validObstructionPositions}");
    }

    /// <summary>
    /// Reads the map and finds the guard's starting position.
    /// </summary>
    /// <returns>Guard's starting (x, y) position.</returns>
    static (int x, int y) FindGuard()
    {
        for (int y = 0; y < _lines.Length; y++)
        {
            for (int x = 0; x < _lines[y].Length; x++)
            {
                if (_lines[y][x] == '^' || _lines[y][x] == 'v' || _lines[y][x] == '<' || _lines[y][x] == '>')
                {
                    return (x, y);
                }
            }
        }

        throw new FormatException("Invalid board: Guard is not located.");
    }

    /// <summary>
    /// Counts the number of unique positions the guard visits before leaving the map.
    /// </summary>
    /// <param name="map">The map as a 2D char array.</param>
    /// <param name="startingPosition">Guard's starting position.</param>
    /// <returns>Number of unique positions visited.</returns>
    static int CountUniquePositionsVisited(char[][] map, (int x, int y) startingPosition)
    {
        var simulator = new GuardSimulator(map, startingPosition);
        int uniqueCount = 0;
        var visited = new HashSet<(int x, int y)>();

        // Mark the starting position as visited
        visited.Add(startingPosition);
        uniqueCount++;

        while (true)
        {
            var nextMoveType = simulator.DetermineNextMoveType();

            if (nextMoveType == MoveType.Collision)
            {
                // Turn right
                simulator.TurnRight();
                continue;
            }

            if (nextMoveType == MoveType.Free || nextMoveType == MoveType.FreeOnExisting)
            {
                // Move forward
                simulator.MoveForward();
                var currentPos = simulator.CurrentPosition;

                if (!visited.Contains(currentPos))
                {
                    visited.Add(currentPos);
                    uniqueCount++;
                    // Optionally mark the map with 'X' to indicate visited positions
                    //_lines[currentPos.y][currentPos.x] = 'X';
                }
            }

            if (nextMoveType == MoveType.OutOfMap)
            {
                break;
            }
        }

        return uniqueCount;
    }

    /// <summary>
    /// Finds all valid positions to place a new obstruction that traps the guard in a loop.
    /// </summary>
    /// <param name="map">The original map.</param>
    /// <param name="startingPosition">Guard's starting position.</param>
    /// <returns>Number of valid obstruction positions.</returns>
    static int FindObstructionPositions(char[][] map, (int x, int y) startingPosition)
    {
        int validCount = 0;
        int height = map.Length;
        int width = map[0].Length;

        // Identify all potential obstruction positions
        var potentialPositions = new List<(int x, int y)>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[y][x] == '.' && (x, y) != startingPosition)
                {
                    potentialPositions.Add((x, y));
                }
            }
        }

        Console.WriteLine($"Total potential obstruction positions to evaluate: {potentialPositions.Count}");

        foreach (var pos in potentialPositions)
        {
            // Create a new map with the obstruction placed
            var modifiedMap = map.Select(row => row.ToArray()).ToArray();
            modifiedMap[pos.y][pos.x] = '#';

            // Initialize simulator
            var simulator = new GuardSimulator(modifiedMap, startingPosition);

            // Check if the guard gets trapped in a loop
            bool isLoop = simulator.IsTrappedInLoop();

            if (isLoop)
            {
                validCount++;
            }
        }

        return validCount;
    }
}