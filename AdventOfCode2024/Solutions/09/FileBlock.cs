using System;

namespace AdventOfCode2024.Solutions._09;

public class FileBlock(int blockSize, int? id = null) : IEquatable<FileBlock>
{
    public int? Id { get; } = id;
    public int BlockSize { get; } = blockSize;

    public bool Equals(FileBlock? other)
    {
        return Id == other?.Id && BlockSize == other?.BlockSize;
    }

    public override string ToString()
    {
        var blockString = "";
        var idString = Id.HasValue ? $"{Id.Value}" : ".";

        for (int i = 0; i < BlockSize; i++)
        {
            blockString += idString;
        }

        return blockString;
    }
}