using Utilities.InputUtilities;
using Utilities.UtilityClasses;

namespace AdventOfCode2024.Solutions;

public class Day8 : IDay
{
    private readonly string _inputFilePath;
    private readonly int[] _gridDimensions = new int[2];

    private Dictionary<char, List<GridPoint>> _frequencyToCoordinates = [];
    private Dictionary<char, HashSet<GridPoint>> _frequencyToNonHarmonicAntinode = [];
    private Dictionary<char, HashSet<GridPoint>> _frequencyToHarmonicAntinode = [];

    public Day8(string inputFileName)
    {
        _inputFilePath = inputFileName;
        ProcessInput();
        FindAntinodes();
    }

    public string PartOne()
    {
        HashSet<GridPoint> uniqueAntinodeLocations = [];

        foreach (char frequencyChar in _frequencyToNonHarmonicAntinode.Keys)
        {
            foreach (var point in _frequencyToNonHarmonicAntinode[frequencyChar])
            {
                if (PointWithinBounds(point))
                {
                    uniqueAntinodeLocations.Add(point);
                }
            }
        }

        return $"The number of unique locations of antinodes is: {uniqueAntinodeLocations.Count}.";
    }

    public string PartTwo()
    {
        HashSet<GridPoint> uniqueAntinodeLocations = [];

        foreach (char frequencyChar in _frequencyToHarmonicAntinode.Keys)
        {
            foreach (var point in _frequencyToHarmonicAntinode[frequencyChar])
            {
                if (PointWithinBounds(point))
                {
                    uniqueAntinodeLocations.Add(point);
                }
            }
        }
        return $"The number of unique locations of antinodes with harmonics is: {uniqueAntinodeLocations.Count}.";
    }

    private void ProcessInput()
    {
        void processChar(char c, int row, int col)
        {
            if (c != '.')
            {
                var coordinateList = _frequencyToCoordinates.TryGetValue(c, out var entry) ? entry : [];
                coordinateList.Add(new GridPoint(row, col));
                _frequencyToCoordinates[c] = coordinateList;
            }
        }

        GridInput.ReadByCharAndOutputBoundaries(_inputFilePath, processChar, out _gridDimensions[0], out _gridDimensions[1]);
    }


    private bool PointWithinBounds(GridPoint point)
    {
        return point.Row >= 0
               && point.Column >= 0
               && point.Row < _gridDimensions[0]
               && point.Column < _gridDimensions[1];
    }

    private void FindAntinodes()
    {
        foreach (var frequencyChar in _frequencyToCoordinates.Keys)
        {
            var coordinateList = _frequencyToCoordinates[frequencyChar];

            _frequencyToNonHarmonicAntinode[frequencyChar] = [];
            _frequencyToHarmonicAntinode[frequencyChar] = [];
            for (int i = 0; i < coordinateList.Count - 1; i++)
            {
                var point1 = coordinateList[i];
                _frequencyToHarmonicAntinode[frequencyChar].Add(point1);
                for (int j = i + 1; j < coordinateList.Count; j++)
                {
                    var point2 = coordinateList[j];
                    _frequencyToHarmonicAntinode[frequencyChar].Add(point2);

                    var difference = point1 - point2;

                    _frequencyToNonHarmonicAntinode[frequencyChar].Add(point2 - difference);
                    _frequencyToNonHarmonicAntinode[frequencyChar].Add(point1 + difference);

                    var harmonicPoint = point1 + difference;

                    while (PointWithinBounds(harmonicPoint))
                    {
                        _frequencyToHarmonicAntinode[frequencyChar].Add(harmonicPoint);

                        harmonicPoint += difference;
                    }

                    harmonicPoint = point2 - difference;

                    while (PointWithinBounds(harmonicPoint))
                    {
                        _frequencyToHarmonicAntinode[frequencyChar].Add(harmonicPoint);

                        harmonicPoint -= difference;
                    }
                }
            }
        }
    }
}