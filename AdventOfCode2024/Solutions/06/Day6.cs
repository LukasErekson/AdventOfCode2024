namespace AdventOfCode2024.Solutions._06;

public class Day6 : IDay
{
    private readonly string _inputFilePath;
    private readonly List<char[]> _map = [];
    private Guard _guard;
    private Dictionary<Tuple<int, int>, HashSet<char>> _positionsToDirections = [];

    public Day6(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        while (!Step()) { }

        return $"The number of unique locations the guard will visit is: {_positionsToDirections.Count()}.";
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    private void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? row;

            while ((row = streamReader.ReadLine()) != null)
            {
                // Build the map
                _map.Add(row.ToCharArray());

                // Add the guard
                var guardInRow = Guard.GuardChars.Intersect(row);
                if (guardInRow.Count() == 1)
                {
                    _guard = new Guard(row: _map.Count - 1, column: row.IndexOf(guardInRow.First()), guardInRow.First());
                }
            }
        }
    }

    private bool Step()
    {
        var nextMove = _guard.NextMove();
        bool isNextMoveExit = nextMove[0] <= -1 || nextMove[1] <= -1 || nextMove[0] >= _map.Count || nextMove[1] >= _map[0].Length;
        bool guardCanMove = isNextMoveExit || _map[nextMove[0]][nextMove[1]] != '#';

        while (!guardCanMove)
        {
            _guard.Turn();
            nextMove = _guard.NextMove();
            isNextMoveExit = nextMove[0] <= -1 || nextMove[1] <= -1 || nextMove[0] >= _map.Count || nextMove[1] >= _map[0].Length;
            guardCanMove = isNextMoveExit || _map[nextMove[0]][nextMove[1]] != '#';
        }

        var positionCopy = new Tuple<int, int>(_guard.Position[0], _guard.Position[1]);

        if (_positionsToDirections.ContainsKey(positionCopy))
        {
            _positionsToDirections[positionCopy].Add(_guard.GuardDirection);
        }
        else
        {
            _positionsToDirections[positionCopy] = [_guard.GuardDirection];
        }

        _guard.MoveForward();

        return isNextMoveExit;
    }

    private string MapString()
    {
        var map = "";
        foreach (var row in _map)
        {
            map += new string(row) + '\n';
        }

        map += $"\n{_guard.Position[0]}, {_guard.Position[1]}, {_guard.GuardDirection}\n";

        return map;
    }
}
