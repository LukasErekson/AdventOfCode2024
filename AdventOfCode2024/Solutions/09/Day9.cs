using AdventOfCode2024.Solutions._09;

namespace AdventOfCode2024.Solutions;

public class Day9 : IDay
{
    private readonly string _inputFilePath;

    private readonly List<int?> _fileBlocksInts = [];
    private List<FileBlock> _fileBlocks = [];

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
            if (!fileBlocksPushedToEnd[i].HasValue)
            {
                continue;
            }
            partOneCheckSum += i * fileBlocksPushedToEnd[i].Value;
        }

        return $"The checksum for the filesystem after pushing all empty space to the end is {partOneCheckSum}.";
    }

    public string PartTwo()
    {
        DefragFileBLockList();

        long checksum = 0;
        var fileBlockAsIntList = new List<int>();

        foreach (var block in _fileBlocks)
        {
            var blockValue = block.Id ?? 0;

            for (int i = 0; i < block.BlockSize; i++)
            {
                fileBlockAsIntList.Add(blockValue);
            }
        }

        for (int i = 0; i < fileBlockAsIntList.Count; i++)
        {
            checksum += i * fileBlockAsIntList[i];
        }
        return $"The checksum for the defragmented filesystem is {checksum}.";
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
                        _fileBlocksInts.Add(fileId);
                    }

                    _fileBlocks.Add(new FileBlock(spaceSize.Value, fileId));

                    fileId++;
                }
                else
                {
                    for (int i = 0; i < spaceSize; i++)
                    {
                        _fileBlocksInts.Add(null);
                    }
                    _fileBlocks.Add(new FileBlock(spaceSize.Value, null));
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
        List<int?> compactFiles = _fileBlocksInts.ConvertAll(val => val);

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

    private void DefragFileBLockList()
    {
        var leftIndex = 1;
        var rightIndex = _fileBlocks.Count - 1;

        while (rightIndex > 1 && _fileBlocks[rightIndex].Id != 0)
        {

            if (_fileBlocks[rightIndex].Id is null)
            {
                rightIndex--;
                continue;
            }

            if (leftIndex >= rightIndex)
            {
                leftIndex = 0;
                rightIndex--;
            }

            if (_fileBlocks[leftIndex].Id != null)
            {
                leftIndex++;
                continue;
            }

            var blockWithId = _fileBlocks[rightIndex];
            var emptyBlock = _fileBlocks[leftIndex];

            if (emptyBlock.BlockSize >= blockWithId.BlockSize)
            {
                var blockSizeDifference = emptyBlock.BlockSize - blockWithId.BlockSize;

                _fileBlocks[leftIndex] = new FileBlock(blockWithId.BlockSize, blockWithId.Id);
                var newEmptyBlock = new FileBlock(blockWithId.BlockSize);
                _fileBlocks[rightIndex] = newEmptyBlock;

                if (blockSizeDifference > 0)
                {
                    _fileBlocks.Insert(leftIndex + 1, new FileBlock(blockSizeDifference));
                }
                rightIndex--;
                leftIndex = 0;
            }

            leftIndex++;
        }
    }

    public override string ToString()
    {
        var memoryBlockString = "";

        foreach (var block in _fileBlocksInts)
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

    public string FileBlockListToString(List<FileBlock> list)
    {
        var memoryBlockString = "";

        foreach (var block in list)
        {
            memoryBlockString += $"({block})";
        }

        return memoryBlockString;
    }
}