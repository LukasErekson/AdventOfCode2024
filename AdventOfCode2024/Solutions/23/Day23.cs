using System;

namespace AdventOfCode2024.Solutions;

public class Day23 : IDay
{
    private string _inputFilePath;
    private Dictionary<string, HashSet<string>> _computerToConnections = [];
    private HashSet<string> _connectionTriplets = [];
    private int _tripletWithTCount = 0;

    public Day23(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        FindTriplietConnections();
        return $"The number of triplet connections that have a computer with 't' in the name is: {_tripletWithTCount}.";
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                string[] connections = line.Split('-');

                var leftComputerConnections = _computerToConnections.GetValueOrDefault(connections[0], []);
                var rightComputerConnections = _computerToConnections.GetValueOrDefault(connections[1], []);

                leftComputerConnections.Add(connections[1]);
                rightComputerConnections.Add(connections[0]);

                _computerToConnections[connections[0]] = leftComputerConnections;
                _computerToConnections[connections[1]] = rightComputerConnections;
            }
        }
        else
        {
            Console.WriteLine($"Cannot find file {_inputFilePath}");
        }
    }

    private void FindTriplietConnections()
    {
        foreach (var leftComputer in _computerToConnections.Keys)
        {
            foreach (var rightComputer in _computerToConnections[leftComputer])
            {
                foreach (var commonConnection in GetCommonConnections(leftComputer, rightComputer))
                {
                    List<string> tripletConnections = [leftComputer, rightComputer, commonConnection];
                    tripletConnections.Sort();
                    bool anyStartWithT = tripletConnections.Any(x => x.StartsWith('t'));
                    // Console.WriteLine($"{leftComputer} {rightComputer} {commonConnection}");

                    var newTriplet = string.Join('-', tripletConnections);
                    if (_connectionTriplets.Add(newTriplet) && anyStartWithT)
                    {
                        _tripletWithTCount += 1;
                    }
                }
            }
        }
    }

    private IEnumerable<string> GetCommonConnections(string leftComputer, string rightComputer)
    {
        return _computerToConnections.Keys.Where(computer => _computerToConnections[computer].Contains(leftComputer) && _computerToConnections[computer].Contains(rightComputer));
    }
}
