using System;

namespace AdventOfCode2024.Solutions._05;

public class Day5 : IDay
{
    private readonly string _inputFilePath;
    private Dictionary<string, List<string>> _pageNumberToPredecessors = [];
    private List<string> _pageUpdates = [];

    public Day5(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        var validUpdates = ValidUpdates();
        var middleNumberSum = 0;

        foreach (var updateList in validUpdates)
        {
            middleNumberSum += int.Parse(FindMiddlePageNumber(updateList));
        }

        return $"The sum of the middle numbers of the valid updates is {middleNumberSum}.";
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
                if (line == "")
                {
                    continue;
                }

                if (line.Contains('|'))
                {
                    string[] orderRule = line.Split('|');

                    if (!_pageNumberToPredecessors.ContainsKey(orderRule[0]))
                    {
                        _pageNumberToPredecessors[orderRule[0]] = [];
                    }

                    _pageNumberToPredecessors[orderRule[0]].Add(orderRule[1]);
                }
                else
                {
                    _pageUpdates.Add(line);
                }
            }
        }
    }

    private List<string> ValidUpdates()
    {
        List<string> validUpdates = [];

        foreach (string updateList in _pageUpdates)
        {
            var validList = true;
            string[] updates = updateList.Split(',');
            var seenNumbers = new HashSet<string>();
            foreach (string update in updates)
            {
                seenNumbers.Add(update);
                if (!_pageNumberToPredecessors.ContainsKey(update))
                {
                    continue;
                }

                if (_pageNumberToPredecessors[update].Any(seenNumbers.Contains))
                {
                    validList = false;
                    break;
                }
            }

            if (validList)
            {
                validUpdates.Add(updateList);
            }
        }

        return validUpdates;
    }

    private static string FindMiddlePageNumber(string update)
    {
        string[] updates = update.Split(',');

        return updates[updates.Length / 2];
    }
}
