using System.Numerics;

namespace AdventOfCode2024.Solutions;

public class Day11 : IDay
{
    private string _inputFilePath;
    private List<long> _numberedStones = [];

    public Day11(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        var numberedStonesCopy = _numberedStones.ConvertAll(n => n);
        int numBlinks = 25;

        for (int i = 0; i < numBlinks; i++)
        {
            Blink(numberedStonesCopy);
        }

        return $"After {numBlinks} blinks, there are {numberedStonesCopy.Count} stones.";
    }

    public string PartTwo()
    {
        Dictionary<long, long> numberToStoneCount = _numberedStones.ToDictionary(number => number, number => (long)1);

        int numBlinks = 75;
        long sum = 0;
        for (int i = 0; i < numBlinks; i++)
        {
            Console.WriteLine($"Working on blink# {i + 1}");
            numberToStoneCount = BlinkWithDictionary(numberToStoneCount);
        }

        foreach (var count in numberToStoneCount.Keys)
        {
            sum += numberToStoneCount[count];
        }
        return $"After {numBlinks} blinks, there are {sum} stones.";
    }

    private void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line = streamReader.ReadLine();

            if (line != null)
            {
                _numberedStones = line.Split(' ').Select(long.Parse).ToList();
            }
        }
        else
        {
            Console.WriteLine($"File {_inputFilePath} does not exist.");
        }
    }

    private static List<long> Blink(List<long> numberedStones)
    {
        List<long> listCopy = numberedStones.ConvertAll(n => n);
        int numberedStonesIndex = 0;

        for (int i = 0; i < listCopy.Count; i++)
        {
            var stoneNum = listCopy[i];
            if (stoneNum == 0)
            {
                numberedStones[numberedStonesIndex] = 1;
            }
            else if (stoneNum.ToString().Length % 2 == 0)
            {
                var stoneNumString = stoneNum.ToString();
                var leftNum = int.Parse(stoneNumString[..(stoneNumString.Length / 2)]);
                var rightNum = int.Parse(stoneNumString[(stoneNumString.Length / 2)..]);

                numberedStones[numberedStonesIndex] = rightNum;
                numberedStones.Insert(numberedStonesIndex, leftNum);
                numberedStonesIndex++;
            }
            else
            {
                numberedStones[numberedStonesIndex] *= 2024;
            }
            numberedStonesIndex++;
        }

        return numberedStones;
    }

    private static Dictionary<long, long> BlinkWithDictionary(Dictionary<long, long> numberToStoneCount)
    {
        Dictionary<long, long> newDictionary = [];

        foreach (var stoneNum in numberToStoneCount.Keys)
        {
            if (stoneNum == 0)
            {
                newDictionary[1] = newDictionary.GetValueOrDefault(1, 0) + numberToStoneCount[stoneNum];
            }
            else if (stoneNum.ToString().Length % 2 == 0)
            {
                var stoneNumString = stoneNum.ToString();
                var leftNum = long.Parse(stoneNumString[..(stoneNumString.Length / 2)]);
                var rightNum = long.Parse(stoneNumString[(stoneNumString.Length / 2)..]);

                newDictionary[leftNum] = newDictionary.GetValueOrDefault(leftNum, 0) + numberToStoneCount[stoneNum];
                newDictionary[rightNum] = newDictionary.GetValueOrDefault(rightNum, 0) + numberToStoneCount[stoneNum];
            }
            else
            {
                newDictionary[stoneNum * 2024] = newDictionary.GetValueOrDefault(stoneNum * 2024, 0) + numberToStoneCount[stoneNum];
            }
        }

        return newDictionary;
    }

    private static string RockListToString(List<long> numberedStones)
    {
        var listAsString = "";

        foreach (var stoneNum in numberedStones)
        {
            listAsString += $"{stoneNum} ";
        }

        return listAsString;
    }
}
