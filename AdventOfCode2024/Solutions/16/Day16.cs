
using Utilities.InputUtilities;
using Utilities.UtilityClasses;

namespace AdventOfCode2024.Solutions;

public class Day16 : IDay
{
    private string _inputFilePath;
    private HashSet<GridPointDirection> _unvistedNodes = [];
    private Dictionary<GridPointDirection, int> _nodeToMinScore = [];
    private GridPoint _endPosition = new(-1, -1);
    private static readonly HashSet<char> _validChars = ['S', 'E', '.'];
    private static readonly Dictionary<Directions, GridPoint> _directionToMoveDelta = new()
    {
        { Directions.Up, new GridPoint(-1,  0) },
        { Directions.Down, new GridPoint( 1,  0) },
        { Directions.Right,  new GridPoint( 0,  1) },
        { Directions.Left,  new GridPoint( 0, -1) },
    };

    public Day16(string inputFilePath)
    {
        _inputFilePath = inputFilePath;

        void processChar(char c, int row, int col)
        {
            if (!_validChars.Contains(c))
            {
                return;
            }

            GridPoint point = new(row, col);

            foreach (Directions direction in Enum.GetValues(typeof(Directions)))
            {
                GridPointDirection pointDirection = new(point, direction);
                _unvistedNodes.Add(pointDirection);
                _nodeToMinScore[pointDirection] = int.MaxValue;
            }

            if (c == 'S')
            {
                GridPointDirection pointDirection = new(point, Directions.Right);
                _nodeToMinScore[pointDirection] = 0;
                return;
            }
            else if (c == 'E')
            {
                _endPosition = point;
            }
        }

        GridInput.ReadByChar(_inputFilePath, processChar);
    }

    public string PartOne()
    {
        CalculateScores();

        int minScore = _nodeToMinScore.Where(node => node.Key.Location == _endPosition)
                                      .Min(node => node.Value);

        return $"The minimum score is {minScore}.";
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    private void CalculateScores()
    {
        var currentNode = _unvistedNodes.MinBy(node => _nodeToMinScore[node]);

        if (currentNode is null)
        {
            return;
        }

        var currentScore = _nodeToMinScore[currentNode];

        while (currentNode.Location != _endPosition && currentScore < int.MaxValue && _unvistedNodes.Count > 0)
        {

            List<GridPointDirection> unvisitedNeighbors = [];

            foreach (var item in _directionToMoveDelta)
            {
                GridPointDirection neighbor = new(currentNode.Location + item.Value, item.Key);
                if (_unvistedNodes.Contains(neighbor))
                {
                    unvisitedNeighbors.Add(neighbor);
                }
            }

            foreach (var neighbor in unvisitedNeighbors)
            {
                var scoreThroughCurrent = currentScore + ComputeCost(currentNode.Direction, neighbor.Direction);

                if (scoreThroughCurrent < _nodeToMinScore[neighbor])
                {
                    _nodeToMinScore[neighbor] = scoreThroughCurrent;
                }
            }

            _unvistedNodes.Remove(currentNode);

            currentNode = _unvistedNodes.MinBy(node => _nodeToMinScore[node]);

            if (currentNode is null)
            {
                break;
            }

            currentScore = _nodeToMinScore.GetValueOrDefault(currentNode, int.MaxValue);
        }
    }

    private static int ComputeCost(Directions current, Directions newDirection)
    {
        if (GridPointDirection.DirectionToOppositeDirection[current] == newDirection)
        {
            return 2001;
        }
        else if (current == newDirection)
        {
            return 1;
        }

        return 1001;
    }
}