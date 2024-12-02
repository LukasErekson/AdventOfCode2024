using AdventOfCode2024;
namespace _02.Day2;
public class Day2 : IDay<int, int>
{
    public int SafeReports { get; set; } = 0;
    private readonly string _inputFilePath;

    public Day2(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessData();
    }

    public int PartOne()
    {
        throw new NotImplementedException();
    }

    public int PartTwo()
    {
        throw new NotImplementedException();
    }

    private void ProcessData()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                var isSafe = ProcessLine(line);
                if (isSafe)
                {
                    SafeReports++;
                }
            }
        }
    }

    private static bool ProcessLine(string? line)
    {
        if (line == null)
        {
            return false;
        }

        IEnumerable<int> levels = line.Split(' ').ToList().Select(int.Parse);

        bool increasing = levels.ElementAt(0) < levels.ElementAt(1);
        int currentIndex = 0;

        while (currentIndex + 1 < levels.Count())
        {
            var difference = levels.ElementAt(currentIndex + 1) - levels.ElementAt(currentIndex);
            if (increasing && (difference > 3 || difference < 0))
            {
                return false;
            }
            else if (difference < -3 || difference > 0)
            {
                return false;
            }

            currentIndex++;
        }

        return true;
    }
}