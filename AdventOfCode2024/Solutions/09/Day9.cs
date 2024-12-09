
using System.Collections.Immutable;

namespace AdventOfCode2024.Solutions;

public class Day9 : IDay
{
    private readonly string _inputFilePath;

    private readonly List<int?> _fileBlocks = [];

    public Day9(string inputFileString)
    {
        _inputFilePath = inputFileString;
        ProcessInput();
    }

    public string PartOne()
    {
        var fileBlocksPushedToEnd = PushEmptySpaceToEnd();
        long partOneCheckSum = 0;

        for (int i = 0; i < fileBlocksPushedToEnd.Count; i++)
        {
            if (fileBlocksPushedToEnd[i] is null)
            {
                continue;
            }
            partOneCheckSum += i * fileBlocksPushedToEnd[i].Value;
        }

        return $"The checksum for the filesystem after pushing all empty space to the end is {partOneCheckSum}.";
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
            int? spaceSize;
            bool isFile = true;
            int fileId = 0;

            while ((spaceSize = streamReader.Read()) != -1)
            {
                spaceSize -= '0';
                if (isFile)
                {
                    for (int i = 0; i < spaceSize; i++)
                    {
                        _fileBlocks.Add(fileId);
                    }

                    fileId++;
                }
                else
                {
                    for (int i = 0; i < spaceSize; i++)
                    {
                        _fileBlocks.Add(null);
                    }
                }

                isFile = !isFile;
            }
        }
        else
        {
            Console.WriteLine($"File {_inputFilePath} does not exist.");
        }
    }

    private List<int?> PushEmptySpaceToEnd()
    {
        List<int?> compactFiles = _fileBlocks.ConvertAll(val => val);

        int leftIndex = 0;
        int rightIndex = compactFiles.Count - 1;

        while (leftIndex < rightIndex)
        {
            if (compactFiles[rightIndex] is null)
            {
                rightIndex--;
                continue;
            }

            if (compactFiles[leftIndex] is null)
            {
                compactFiles[leftIndex] = compactFiles[rightIndex];
                compactFiles[rightIndex] = null;
                rightIndex--;
            }
            leftIndex++;
        }

        return compactFiles;
    }

    public override string ToString()
    {
        var memoryBlockString = "";

        foreach (var block in _fileBlocks)
        {
            if (block is null)
            {
                memoryBlockString += '.';
                continue;
            }
            memoryBlockString += $"({block})";
        }

        return memoryBlockString;
    }

    public string FileBlockListToString<T>(List<T> list)
    {
        var memoryBlockString = "";

        foreach (var block in _fileBlocks)
        {
            if (block is null)
            {
                memoryBlockString += '.';
                continue;
            }
            memoryBlockString += $"({block})";
        }

        return memoryBlockString;
    }
}