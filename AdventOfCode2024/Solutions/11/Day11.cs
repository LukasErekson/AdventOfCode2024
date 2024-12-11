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
        // Console.WriteLine();
        // Console.WriteLine(RockListToString(numberedStonesCopy));
        for (int i = 0; i < numBlinks; i++)
        {
            Blink(numberedStonesCopy);
            // Console.WriteLine(RockListToString(numberedStonesCopy));
        }

        return $"After {numBlinks} blinks, there are {numberedStonesCopy.Count} stones.";
    }

    public string PartTwo()
    {
        List<List<long>> numberedStonesCopy = [_numberedStones.ConvertAll(n => n)];
        int numBlinks = 25;
        for (int i = 0; i < numBlinks; i++)
        {
            Console.WriteLine($"Working on chunk # {i + 1}");
            List<List<long>> replacementList = [];
            foreach (var smallerList in numberedStonesCopy)
            {
                List<List<long>> newList = [];
                _ = Parallel.ForEach(smallerList.Chunk(1000),
                    new ParallelOptions
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    },
                    chunk =>
                    {
                        var chunkList = chunk.ToList();
                        Blink(chunkList);
                        newList.Add(chunkList);
                    });
                foreach (var list in newList)
                {
                    replacementList.Add(list);
                }
            }
            numberedStonesCopy = replacementList;
        }

        BigInteger sum = 0;

        foreach (var list in numberedStonesCopy)
        {
            sum += list.Count;
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
