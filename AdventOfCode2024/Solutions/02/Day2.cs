namespace AdventOfCode2024.Solution_02.Day2;
public class Day2 : IDay<int, int>
{
    public int SafeReports { get; set; } = 0;
    public int SafeReportsWithDampener { get; set; } = 0;
    private readonly string _inputFilePath;

    public Day2(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessData();
    }

    public int PartOne()
    {
        return SafeReports;
    }

    public int PartTwo()
    {
        return SafeReportsWithDampener;
    }

    public string Solution()
    {
        return $"The number of safe reports is {PartOne()}.\nThe number of safe reports WITH the dampener is {PartTwo()}.";
    }

    private void ProcessData()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                IEnumerable<int> levels = line.Split(' ')
                    .Select(int.Parse)
                    .ToList();

                var isSafe = ProcessIsSafe(levels);
                if (isSafe)
                {
                    SafeReports++;
                    SafeReportsWithDampener++;
                }
                else
                {
                    for (int i = 0; i < levels.Count(); i++)
                    {
                        var subSet = levels.ToList();
                        subSet.RemoveAt(i);
                        if (ProcessIsSafe(subSet))
                        {
                            SafeReportsWithDampener++;
                            break;
                        }
                    }
                }
            }
        }
    }

    private static bool ProcessIsSafe(IEnumerable<int> levels)
    {
        bool increasing = levels.ElementAt(0) < levels.ElementAt(1);
        int currentIndex = 0;

        while (currentIndex + 1 < levels.Count())
        {
            var difference = levels.ElementAt(currentIndex + 1) - levels.ElementAt(currentIndex);
            if (increasing)
            {
                if (difference > 3 || difference <= 0)
                {
                    return false;
                }
            }
            else
            {
                if (difference < -3 || difference >= 0)
                {
                    return false;
                }
            }
            currentIndex++;
        }

        return true;
    }
}