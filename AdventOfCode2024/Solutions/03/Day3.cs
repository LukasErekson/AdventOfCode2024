using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solutions;
public class Day3 : IDay
{
    private readonly string _inputFilePath;
    private int partOneSum = 0;
    private int partTwoSum = 0;

    public Day3(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ReadFile();
    }

    public string PartOne()
    {
        return $"Accounting for only complete mul() statements, the total is {partOneSum}.";
    }

    public string PartTwo()
    {
        return $"Accounting only for complete and valid mul() statements, the total is {partTwoSum}.";
    }

    private void ReadFile()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            var regex = new Regex(@"mul\((\d+),(\d+)\)");
            var regex2 = new Regex(@"(don't\(\))(.*?)mul\((\d+),(\d+)\)(.*?)do\(\)");

            while ((line = streamReader.ReadLine()) != null)
            {
                var matches = regex.Matches(line);

                foreach (Match match in matches)
                {
                    var firstNum = match.Groups[1];
                    var secondNum = match.Groups[2];

                    partOneSum += int.Parse(firstNum.Value) * int.Parse(secondNum.Value);
                }

                var matches2 = regex2.Matches(line);
                foreach (Match match in matches2)
                {
                    var mulMatches = regex.Matches(match.ToString());

                    foreach (Match mulMatch in mulMatches)
                    {
                        var firstNum = mulMatch.Groups[1];
                        var secondNum = mulMatch.Groups[2];

                        partTwoSum += int.Parse(firstNum.Value) * int.Parse(secondNum.Value);
                    }

                }

                partTwoSum = partOneSum - partTwoSum;
            }
        }
    }

}
