using System;
using Utilities.InputUtilities;
using Utilities.UtilityClasses;

namespace AdventOfCode2024.Solutions;

public class Day12 : IDay
{
    private readonly string _inputFilePath;
    private readonly Dictionary<char, HashSet<GridPoint>> _plantTypeToCoordinates = [];
    private readonly Dictionary<char, List<HashSet<GridPoint>>> _plantTypeToRegions = [];
    private readonly char[,] _map;
    private readonly int[] _mapBoundaries = new int[2];
    private HashSet<GridPoint> _unvisitedPoints = [];

    public Day12(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();

        _map = new char[_mapBoundaries[0], _mapBoundaries[1]];
        foreach (var plant in _plantTypeToCoordinates.Keys)
        {
            foreach (var coordinate in _plantTypeToCoordinates[plant])
            {
                _map[coordinate.Row, coordinate.Column] = plant;
            }
        }

        FormRegions();
    }

    public string PartOne()
    {
        var sum = 0;
        foreach (var plant in _plantTypeToRegions.Keys)
        {
            foreach (var regionCoordinates in _plantTypeToRegions[plant])
            {
                var perimeter = PerimeterOfRegion(regionCoordinates, plant);
                var area = regionCoordinates.Count;

                sum += perimeter * area;
            }
        }

        return $"The sum of the perimeter x area is {sum}.";
    }

    public string PartTwo()
    {
        var sum = 0;
        foreach (var plant in _plantTypeToRegions.Keys)
        {
            foreach (var regionCoordinates in _plantTypeToRegions[plant])
            {
                var sides = CountCorners(regionCoordinates, plant);
                var area = regionCoordinates.Count;

                sum += sides * area;
            }
        }

        return $"The sum of the perimeter x area is {sum}.";
    }

    private void ProcessInput()
    {
        void processChar(char plant, int row, int col)
        {
            var coordinateList = _plantTypeToCoordinates.GetValueOrDefault(plant, []);
            coordinateList.Add(new GridPoint(row, col));
            _plantTypeToCoordinates[plant] = coordinateList;
            _unvisitedPoints.Add(new GridPoint(row, col));
        }
        GridInput.ReadByCharAndOutputBoundaries(_inputFilePath, processChar, out _mapBoundaries[0], out _mapBoundaries[1]);
    }

    private void FormRegions()
    {
        while (_unvisitedPoints.Count > 0)
        {
            var currentPoint = _unvisitedPoints.First();
            _unvisitedPoints.Remove(currentPoint);
            var currentPlant = _map[currentPoint.Row, currentPoint.Column];

            List<GridPoint> pointsInSameRegion = [currentPoint, .. SamePlantsInBurst(currentPoint, currentPlant)];
            int visitPointsIndex = 0;

            while (visitPointsIndex < pointsInSameRegion.Count)
            {
                currentPoint = pointsInSameRegion[visitPointsIndex];
                _unvisitedPoints.Remove(currentPoint);

                pointsInSameRegion = [.. pointsInSameRegion, .. SamePlantsInBurst(currentPoint, currentPlant).Where(p => !pointsInSameRegion.Contains(p))];

                visitPointsIndex++;
            }

            var regionsList = _plantTypeToRegions.GetValueOrDefault(currentPlant, []);
            regionsList.Add([.. pointsInSameRegion]);
            _plantTypeToRegions[currentPlant] = regionsList;
        }
    }

    private List<GridPoint> SamePlantsInBurst(GridPoint point, char plant)
    {
        return GridPoint.BurstWithinBounds(point, _mapBoundaries[0], _mapBoundaries[1], includeDiagonals: false)
                        .Where(p => _plantTypeToCoordinates[plant].Contains(p))
                        .ToList();
    }
    private int PerimeterOfRegion(HashSet<GridPoint> regionCoordinates, char plant)
    {
        var totalPerimeter = 0;
        foreach (var point in regionCoordinates)
        {
            totalPerimeter += PerimeterOfSinglePlant(point, plant);
        }

        return totalPerimeter;
    }

    private int PerimeterOfSinglePlant(GridPoint point, char plant)
    {
        var perimeter = 0;

        foreach (var newPoint in GridPoint.Burst(point, includeDiagonals: false))
        {
            if (!GridPoint.WithinBounds(newPoint, _mapBoundaries[0], _mapBoundaries[1]) || !_plantTypeToCoordinates[plant].Contains(newPoint))
            {
                perimeter++;
            }
        }

        return perimeter;
    }

    private int CountCorners(HashSet<GridPoint> regionPoints, char plantType)
    {
        // A region of only 1 has 4.
        if (regionPoints.Count == 1)
        {
            return 4;
        }

        var corners = 0;

        foreach (var centerPoint in regionPoints)
        {
            var burstLocations = GridPoint.BurstWithinBounds(centerPoint, _mapBoundaries[0], _mapBoundaries[1]).Where(regionPoints.Contains).ToList();

            if (burstLocations.Count == 1)
            {
                corners += 2;
            }
            else
            {
                HashSet<GridPoint> pointsToCheck = [];
                for (int i = 0; i < burstLocations.Count - 1; i++)
                {
                    for (int j = i + 1; j < burstLocations.Count; j++)
                    {
                        var newPoint = burstLocations[i] + burstLocations[j] - centerPoint;
                        if (GridPoint.WithinBounds(newPoint, _mapBoundaries[0], _mapBoundaries[1])
                            && newPoint != centerPoint)
                        {
                            pointsToCheck.Add(newPoint);
                        }
                    }
                }

                // This means we have an L shape; there is always at least 1 corner.
                if (pointsToCheck.Count == 1)
                {
                    corners++;
                }

                corners += pointsToCheck.Where(p => _map[p.Row, p.Column] != plantType).Count();
            }
        }

        return corners;
    }

    private void PrintMap()
    {
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                Console.Write(_map[i, j]);
            }
            Console.Write('\n');
        }
    }
}
