
using AdventOfCode2024.Solutions._08;

namespace AdventOfCode2024.Solutions;

public class Day10 : IDay
{
    private readonly string _inputFilePath;
    private readonly Dictionary<GridPoint, int> _trailHeadsToScore = [];
    private readonly Dictionary<int, HashSet<GridPoint>> _heightToCoordinates = [];
    private readonly int[] _mapDimensions = new int[2];

    public Day10(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {

        var sumOfScores = 0;
        foreach (var trailHead in _heightToCoordinates[0])
        {
            sumOfScores += ScoreTrailDestinations(trailHead).Count;
        }

        return $"The sum of all the trialhead scores is: {sumOfScores}.";
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    public void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            int height;
            int row = 0;
            int col = 0;

            while ((height = streamReader.Read()) != -1)
            {
                if (height == '\r')
                {
                    continue;
                }

                if (height == '\n')
                {
                    row++;
                    _mapDimensions[1] = col;
                    col = 0;
                    continue;
                }

                _heightToCoordinates.TryGetValue(height - '0', out var coordinateSet);
                coordinateSet ??= [];
                coordinateSet.Add(new GridPoint(row, col));
                _heightToCoordinates[height - '0'] = coordinateSet;

                col++;
            }

            _mapDimensions[0] = row + 1;
        }
        else
        {
            Console.WriteLine($"File {_inputFilePath} cannot be found.");
        }
    }

    private int ScoreTrail(GridPoint point, int currentValue = 0)
    {
        var score = 0;

        if (currentValue == 9)
        {
            return 1;
        }

        var nextPoints = _heightToCoordinates[currentValue + 1].Intersect(BurstAroundPoint(point));

        foreach (var validMove in nextPoints)
        {
            score += ScoreTrail(validMove, currentValue + 1);
        }

        return score;
    }

    private HashSet<GridPoint> ScoreTrailDestinations(GridPoint point, int currentValue = 0, HashSet<GridPoint> trailDestinations = null)
    {
        trailDestinations ??= [];

        if (currentValue == 9)
        {
            trailDestinations.Add(point);
            return trailDestinations;
        }

        var nextPoints = _heightToCoordinates[currentValue + 1].Intersect(BurstAroundPoint(point));

        foreach (var validMove in nextPoints)
        {
            trailDestinations = trailDestinations.Union(ScoreTrailDestinations(validMove, currentValue + 1, trailDestinations)).ToHashSet();
        }

        return trailDestinations;
    }

    private bool PointWithinBounds(GridPoint point)
    {
        return point.Row >= 0
               && point.Column >= 0
               && point.Row <= _mapDimensions[0]
               && point.Column <= _mapDimensions[1];
    }

    private HashSet<GridPoint> BurstAroundPoint(GridPoint origin)
    {
        HashSet<GridPoint> burstCoordinates = [];

        HashSet<GridPoint> burstMoves = [
            new GridPoint(1, 0), // Down
            new GridPoint(-1, 0), // Up
            new GridPoint(0, 1), // Right
            new GridPoint(0, -1), // Left
        ];

        foreach (var move in burstMoves)
        {
            var potentialPoint = origin + move;
            if (PointWithinBounds(potentialPoint))
            {
                burstCoordinates.Add(potentialPoint);
            }
        }

        return burstCoordinates;
    }

    private void DebugProcessInput()
    {
        foreach (var height in _heightToCoordinates.Keys)
        {
            Console.WriteLine($"{height}: {string.Join(',', _heightToCoordinates[height])}");
        }

        Console.WriteLine($"Dimensions are: {_mapDimensions[0]}, {_mapDimensions[1]}");
    }

}
