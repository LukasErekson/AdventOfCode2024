using AdventOfCode2024.Solutions._06;
using Utilities.InputUtilities;

namespace AdventOfCode2024.Solutions;

public class Day6 : IDay
{
    private readonly string _inputFilePath;
    private readonly List<char[]> _map = [];
    private readonly int[] _guardStartingPosition = new int[2];
    private readonly int[] _mapBoundaries = new int[2];
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
        string rowString = "";
        void processChar(char c, int row, int col)
        {
            if (col == 0)
            {
                _map.Add(rowString.ToCharArray());
                rowString = "";
            }

            if (Guard.GuardChars.Contains(c))
            {
                _guard = new Guard(row, col, c);
                _guardStartingPosition[0] = row;
                _guardStartingPosition[1] = col;
                _guardStartingDirection = c;
            }
            rowString += c;
        }
        GridInput.ReadByCharAndOutputBoundaries(_inputFilePath, processChar, out _mapBoundaries[0], out _mapBoundaries[1]);
        _map.Add(rowString.ToCharArray());
        _map.RemoveAt(0);
    }

    private bool Step(List<char[]> mapToTraverse)
    {
        var nextMove = _guard.NextMove();
        var guardPositionTuple = new Tuple<int, int>(_guard.Position[0], _guard.Position[1]);

        bool isNextMoveExit = nextMove[0] <= -1 || nextMove[1] <= -1 || nextMove[0] >= _mapBoundaries[0] || nextMove[1] >= _mapBoundaries[1];
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
            var visitedLocationDirection = _doubleVisitedLocationsToDirections.GetValueOrDefault(guardPositionTuple, []);
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
        mapAsString += $"Map Dimensions: {_mapBoundaries[0]}, {_mapBoundaries[1]}";

        return mapAsString;
    }
}
