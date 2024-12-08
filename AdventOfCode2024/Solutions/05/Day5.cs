namespace AdventOfCode2024.Solutions;

public class Day5 : IDay
{
    private readonly string _inputFilePath;
    private Dictionary<string, List<string>> _pageNumberToPredecessors = [];
    private List<string> _pageUpdates = [];
    private List<string> _validUpdates = [];
    private List<string> _invalidUpdates = [];

    public Day5(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        var middleNumberSum = 0;

        foreach (var updateList in _validUpdates)
        {
            middleNumberSum += FindMiddlePageNumber(updateList);
        }

        return $"The sum of the middle numbers of the valid updates is {middleNumberSum}.";
    }

    public string PartTwo()
    {
        var middleNumberSum = 0;

        foreach (var update in _invalidUpdates)
        {
            var values = update.Split(',').ToList();
            values.Sort(new PageOrderer(_pageNumberToPredecessors));
            middleNumberSum += FindMiddlePageNumber(string.Join(',', values));
        }

        return $"After sorting, the sum of the middle values for previously invalid updates is {middleNumberSum}";
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
            ValidateUpdates();
        }
    }

    private void ValidateUpdates()
    {
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
                _validUpdates.Add(updateList);
            }
            else
            {
                _invalidUpdates.Add(updateList);
            }
        }
    }

    private int FindMiddlePageNumber(string update)
    {
        string[] updates = update.Split(',');

        return int.Parse(updates[updates.Length / 2]);
    }
}