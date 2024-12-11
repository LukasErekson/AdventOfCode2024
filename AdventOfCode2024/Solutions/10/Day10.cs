using AdventOfCode2024.Solutions._08;
using Utilities.InputUtilities;

namespace AdventOfCode2024.Solutions;

public class Day10 : IDay
{
    private readonly string _inputFilePath;
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
            sumOfScores += ScoreTrailHead(trailHead).Count;
        }

        return $"The sum of all the trailhead scores is: {sumOfScores}.";
    }

    public string PartTwo()
    {
        var sumOfRatings = 0;

        foreach (var trailHead in _heightToCoordinates[0])
        {
            sumOfRatings += RateTrialHead(trailHead);
        }

        return $"The sum of all the trailhead ratings is: {sumOfRatings}.";
    }

    public void ProcessInput()
    {
        void ProcessChar(char height, int row, int col)
        {
            var coordinateSet = _heightToCoordinates.GetValueOrDefault(height - '0', []);
            coordinateSet.Add(new GridPoint(row, col));
            _heightToCoordinates[height - '0'] = coordinateSet;
        }

        GridInput.ReadByCharAndOutputBoundaries(_inputFilePath, ProcessChar, out _mapDimensions[0], out _mapDimensions[1]);
    }

    private int RateTrialHead(GridPoint point, int currentValue = 0)
    {
        var score = 0;

        if (currentValue == 9)
        {
            return 1;
        }

        var nextPoints = _heightToCoordinates[currentValue + 1].Intersect(BurstAroundPoint(point));

        foreach (var validMove in nextPoints)
        {
            score += RateTrialHead(validMove, currentValue + 1);
        }

        return score;
    }

    private HashSet<GridPoint> ScoreTrailHead(GridPoint point, int currentValue = 0, HashSet<GridPoint>? trailDestinations = null)
    {
        trailDestinations ??= [];

        if (currentValue == 9)
        {
            trailDestinations.Add(point);
            return trailDestinations;
        }

        var nextPoints = _heightToCoordinates[currentValue + 1].Intersect(BurstAroundPoint(point).Where(PointWithinBounds));

        foreach (var validMove in nextPoints)
        {
            trailDestinations = trailDestinations.Union(ScoreTrailHead(validMove, currentValue + 1, trailDestinations)).ToHashSet();
        }

        return trailDestinations;
    }

    private bool PointWithinBounds(GridPoint point)
    {
        return point.Row >= 0
               && point.Column >= 0
               && point.Row < _mapDimensions[0]
               && point.Column < _mapDimensions[1];
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
            burstCoordinates.Add(origin + move);
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
