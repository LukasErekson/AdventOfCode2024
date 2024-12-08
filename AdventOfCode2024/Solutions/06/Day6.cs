namespace AdventOfCode2024.Solutions;

public class Day6 : IDay
{
    private readonly string _inputFilePath;
    private readonly List<char[]> _map = [];
    private readonly int[] _guardStartingPosition = new int[2];
    private char _guardStartingDirection;

    private Guard _guard = new(0, 0, '^');
    private HashSet<Tuple<int, int>> _positionsVisited = [];
    private Dictionary<Tuple<int, int>, HashSet<char>> _doubleVisitedLocationsToDirections = [];

    public Day6(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        while (!Step(_map)) { }

        return $"The number of unique locations the guard will visit is: {_positionsVisited.Count}.";
    }

    public string PartTwo()
    {
        return $"The number of places one more obstacle would cause an cycle is: {BruteForcePart2()}";
    }

    private void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? row;

            while ((row = streamReader.ReadLine()) != null)
            {
                var charRow = row.ToCharArray();
                // Add the guard
                var guardInRow = Guard.GuardChars.Intersect(row);
                if (guardInRow.Count() == 1)
                {
                    var column = row.IndexOf(guardInRow.First());
                    _guard = new Guard(row: _map.Count, column: column, guardInRow.First());
                    // charRow[column] = '.';
                    _guardStartingPosition[0] = _map.Count;
                    _guardStartingPosition[1] = column;
                    _guardStartingDirection = guardInRow.First();
                }

                // Build the map
                _map.Add(charRow);
            }
        }
    }

    private bool Step(List<char[]> mapToTraverse)
    {
        var nextMove = _guard.NextMove();
        var guardPositionTuple = new Tuple<int, int>(_guard.Position[0], _guard.Position[1]);

        bool isNextMoveExit = nextMove[0] <= -1 || nextMove[1] <= -1 || nextMove[0] >= mapToTraverse.Count || nextMove[1] >= mapToTraverse[0].Length;
        bool guardCanMove = isNextMoveExit || mapToTraverse[nextMove[0]][nextMove[1]] != '#';

        while (!guardCanMove)
        {
            _guard.Turn();
            nextMove = _guard.NextMove();
            guardPositionTuple = new Tuple<int, int>(_guard.Position[0], _guard.Position[1]);

            bool stuckInLoop = _doubleVisitedLocationsToDirections.ContainsKey(guardPositionTuple) && _doubleVisitedLocationsToDirections[guardPositionTuple].Contains(_guard.GuardDirection);
            if (stuckInLoop)
            {
                throw new InfiniteLoopException("Stuck in a loop!");
            }

            isNextMoveExit = nextMove[0] <= -1 || nextMove[1] <= -1 || nextMove[0] >= mapToTraverse.Count || nextMove[1] >= mapToTraverse[0].Length;
            guardCanMove = isNextMoveExit || mapToTraverse[nextMove[0]][nextMove[1]] != '#';
        }

        if (!_positionsVisited.Add(guardPositionTuple))
        {
            _doubleVisitedLocationsToDirections.TryGetValue(guardPositionTuple, out var visitedLocationDirection);
            visitedLocationDirection ??= [];
            visitedLocationDirection.Add(_guard.GuardDirection);
            _doubleVisitedLocationsToDirections[guardPositionTuple] = visitedLocationDirection;
        }

        _guard.MoveForward();

        return isNextMoveExit;
    }

    private int BruteForcePart2()
    {
        var validObstructionCount = 0;
        for (int row = 0; row < _map.Count; row++)
        {
            for (int col = 0; col < _map[0].Length; col++)
            {
                if (_map[row][col] == '#' || Guard.GuardChars.Contains(_map[row][col]))
                {
                    continue;
                }

                var newMap = new List<char[]>();
                foreach (char[] charRow in _map)
                {
                    char[] newMapCharRow = new char[charRow.Length];
                    Array.Copy(charRow, newMapCharRow, charRow.Length);
                    newMap.Add(newMapCharRow);
                }

                newMap[row][col] = '#';
                _doubleVisitedLocationsToDirections = [];
                _positionsVisited = [];
                _guard = new Guard(_guardStartingPosition[0], _guardStartingPosition[1], _guardStartingDirection);

                try
                {
                    while (!Step(newMap)) { }
                }
                catch (InfiniteLoopException)
                {
                    validObstructionCount++;
                }
            }
        }
        return validObstructionCount;
    }

    private string MapString(List<char[]> map)
    {
        var mapAsString = "";
        foreach (var row in map)
        {
            mapAsString += new string(row) + '\n';
        }

        mapAsString += $"\n{_guard.Position[0]}, {_guard.Position[1]}, {_guard.GuardDirection}\n";

        return mapAsString;
    }
}
