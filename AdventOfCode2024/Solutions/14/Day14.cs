using System;
using MathNet.Numerics.RootFinding;
using Utilities.UtilityClasses;

namespace AdventOfCode2024.Solutions;

public class Day14 : IDay
{
    private readonly string _inputFilePath;
    private readonly List<GridPoint> _robotPositions = [];
    private readonly List<GridPoint> _robotVelocities = [];
    private readonly int _mapWidth;
    private readonly int _mapHeight;

    public Day14(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();

        if (_inputFilePath.Contains("sample"))
        {
            _mapHeight = 7;
            _mapWidth = 11;
        }
        else
        {
            _mapHeight = 103;
            _mapWidth = 101;
        }
    }

    public string PartOne()
    {
        var newPositions = Step(100);
        Dictionary<int, int> quadrantToRobotPositions = [];
        quadrantToRobotPositions[0] = 0;
        quadrantToRobotPositions[1] = 0;
        quadrantToRobotPositions[2] = 0;
        quadrantToRobotPositions[3] = 0;
        quadrantToRobotPositions[4] = 0;

        foreach (var robotPosition in newPositions)
        {
            quadrantToRobotPositions[Quadrant(robotPosition)] += 1;
        }

        quadrantToRobotPositions[0] = 1; // Discard 0

        var product = 1;
        foreach (var robotCount in quadrantToRobotPositions.Values)
        {
            product *= robotCount;
        }

        return $"The product of the quadrant counts is {product}";
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
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                string[] positionAndVelocity = line.Split(' ');
                string[] position = positionAndVelocity[0][(positionAndVelocity[0].IndexOf('=') + 1)..].Split(',');
                string[] velocity = positionAndVelocity[1][(positionAndVelocity[1].IndexOf('=') + 1)..].Split(',');

                _robotPositions.Add(new GridPoint(int.Parse(position[1]), int.Parse(position[0])));
                _robotVelocities.Add(new GridPoint(int.Parse(velocity[1]), int.Parse(velocity[0])));
            }
        }
    }

    private List<GridPoint> Step(int seconds = 1)
    {
        List<GridPoint> newRobotPositions = [];
        for (int i = 0; i < _robotPositions.Count; i++)
        {
            var newPos = _robotPositions[i] + (_robotVelocities[i] * seconds);
            var newRow = newPos.Row % _mapHeight;
            var newCol = newPos.Column % _mapWidth;
            if (newRow < 0)
            {
                newRow += _mapHeight;
            }
            if (newCol < 0)
            {
                newCol += _mapWidth;
            }
            newRobotPositions.Add(new GridPoint(newRow, newCol));
        }

        return newRobotPositions;
    }

    private int Quadrant(GridPoint point)
    {
        var midHeight = (_mapHeight - 1) / 2;
        var midWidth = (_mapWidth - 1) / 2;

        if (point.Row == midHeight || point.Column == midWidth)
        {
            return 0;
        }

        if (point.Row > midHeight)
        {
            if (point.Column < midWidth)
            {
                return 1;
            }
            return 2;
        }
        if (point.Column < midWidth)
        {
            return 3;
        }
        return 4;
    }
}
