namespace AdventOfCode2024.Solutions;

public class Day4 : IDay
{
    private string _inputFilePath;
    private List<string> _wordSearchGrid = [];
    private int[,,] _xmasIndexOffsets = new int[8, 3, 2]
        {
            { {0, 1}, {0, 2}, {0, 3} },// Forward
            { {0, -1}, {0, -2}, {0, -3} },// Backward 
            { {-1, 0}, {-2, 0}, {-3, 0} },// Up
            { {1, 0}, {2, 0}, {3, 0} },// Down
            { {-1, -1}, {-2, -2}, {-3, -3} },// Diagonal Up Backward
            { {-1, 1}, {-2, 2}, {-3, 3} },// Diagonal Up Forward
            { {1, -1}, {2, -2}, {3, -3} },// Diagonal Down Backward
            { {1, 1}, {2, 2}, {3, 3} } // Diagonal Down Forward
        };
    private int[,,] _masXIndexOffsets = new int[4, 4, 2]
        {
            { { -1, -1 }, { 1, 1 }, { -1, 1 }, { 1, -1 } },
            { { 1, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 } },
            { { 1, -1 }, { -1, 1 }, { 1, 1 }, { -1, -1 } },
            { { -1, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 } },
        };

    public Day4(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ReadInput();
    }

    public string PartOne()
    {
        return $"The number of 'XMAS's is: {CountXmas()}.";
    }

    public string PartTwo()
    {
        return $"The number of X-'MAS's is: {CountMasX()}.";
    }


    private void ReadInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                _wordSearchGrid.Add(line);
            }
        }
    }

    private int CountXmas()
    {
        int xmasCount = 0;
        for (int row = 0; row < _wordSearchGrid.Count; row++)
        {
            for (int col = 0; col < _wordSearchGrid[0].Length; col++)
            {
                if (_wordSearchGrid[row][col] == 'X')
                {
                    xmasCount += NumXmas(row, col);
                }
            }
        }

        return xmasCount;
    }

    private int NumXmas(int row, int column)
    {
        var numXmas = 0;
        char[] letters = ['M', 'A', 'S'];

        for (int direction = 0; direction < 8; direction++)
        {
            for (int step = 0; step < 3; step++)
            {
                int rowOffset = _xmasIndexOffsets[direction, step, 0];
                int colOffset = _xmasIndexOffsets[direction, step, 1];

                int checkRow = row + rowOffset;
                int checkCol = column + colOffset;
                if (checkRow < 0 || checkRow >= _wordSearchGrid.Count || checkCol >= _wordSearchGrid[0].Length || checkCol < 0 || _wordSearchGrid[checkRow][checkCol] != letters[step])
                {
                    break;
                }

                if (step == 2)
                {
                    numXmas++;
                }
            }
        }

        return numXmas;
    }

    private int CountMasX()
    {
        int masXCount = 0;
        for (int row = 0; row < _wordSearchGrid.Count; row++)
        {
            for (int col = 0; col < _wordSearchGrid[0].Length; col++)
            {
                if (_wordSearchGrid[row][col] == 'A')
                {
                    masXCount += NumMasX(row, col);
                }
            }
        }

        return masXCount;
    }

    private int NumMasX(int row, int column)
    {
        var numMasX = 0;
        char[] letters = ['M', 'S', 'M', 'S'];

        for (int direction = 0; direction < 4; direction++)
        {
            for (int step = 0; step < 4; step++)
            {
                int rowOffset = _masXIndexOffsets[direction, step, 0];
                int colOffset = _masXIndexOffsets[direction, step, 1];

                int checkRow = row + rowOffset;
                int checkCol = column + colOffset;
                if (checkRow < 0 || checkRow >= _wordSearchGrid.Count || checkCol >= _wordSearchGrid[0].Length || checkCol < 0 || _wordSearchGrid[checkRow][checkCol] != letters[step])
                {
                    break;
                }

                if (step == 3)
                {
                    numMasX++;
                }
            }
        }

        return numMasX;
    }
}