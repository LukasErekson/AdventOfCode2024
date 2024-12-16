using System.Runtime.InteropServices;
using Utilities.InputUtilities;
using Utilities.UtilityClasses;

namespace AdventOfCode2024.Solutions;

public class Day15 : IDay
{
    private readonly string _inputFilePath;
    private int _rowCount = 0;
    private int _columnCount = 0;
    private HashSet<GridPoint> _boxLocations = [];
    private GridPoint _robotLocation = new(-1, -1);
    private HashSet<GridPoint> _boundaryLocations = [];
    private List<char> _directions = [];

    public Day15(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        ProcessMoves();
        long sum = 0;
        foreach (var boxPosition in _boxLocations)
        {
            sum += 100 * boxPosition.Row + boxPosition.Column;
        }
        return $"The sum of the GPS coordinates is {sum}.";
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    private void ProcessInput()
    {
        void processChar(char c, int row, int col)
        {
            var point = new GridPoint(row, col);

            if (c == '@')
            {
                _robotLocation = point;
            }
            else if (c == '#')
            {
                _boundaryLocations.Add(point);
            }
            else if (c == 'O')
            {
                _boxLocations.Add(point);
            }
            else
            {
                return;
            }
        }

        int rowCount;
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;
            int currentRow = 0;
            int currentColumn = 0;

            while ((line = streamReader.ReadLine()) != "")
            {
                if (line == null)
                {
                    break;
                }

                foreach (char c in line)
                {
                    processChar(c, currentRow, currentColumn);
                    currentColumn++;
                }
                _columnCount = currentColumn;
                currentColumn = 0;
                currentRow++;
            }

            rowCount = currentRow;

            _rowCount = rowCount;

            while ((line = streamReader.ReadLine()) != null)
            {
                _directions.AddRange(line);
            }
        }
        else
        {
            Console.WriteLine($"File {_inputFilePath} cannot be found.");
        }
    }

    private void ProcessMoves()
    {
        foreach (char direction in _directions)
        {
            GridPoint gridDirection = direction switch
            {
                '^' => new GridPoint(-1, 0),
                '>' => new GridPoint(0, 1),
                'v' => new GridPoint(1, 0),
                _ => new GridPoint(0, -1), // <
            };
            var newSpot = _robotLocation + gridDirection;

            if (_boundaryLocations.Contains(newSpot))
            {
                continue;
            }

            else if (_boxLocations.Contains(newSpot))
            {
                List<GridPoint> boxesToPush = [newSpot];
                while (_boxLocations.Contains(newSpot + gridDirection))
                {
                    newSpot += gridDirection;
                    boxesToPush.Add(newSpot);
                }

                if (_boundaryLocations.Contains(boxesToPush.Last() + gridDirection)) // Do nothing
                {
                    continue;
                }

                foreach (var boxPosition in boxesToPush)
                {
                    _boxLocations.Remove(boxPosition);
                }

                foreach (var boxPosition in boxesToPush)
                {
                    _boxLocations.Add(boxPosition + gridDirection);
                }

            }
            _robotLocation += gridDirection;
        }
    }
}
