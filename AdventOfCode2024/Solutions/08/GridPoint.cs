namespace AdventOfCode2024.Solutions._08;

public class GridPoint(int row, int column) : IEquatable<GridPoint>
{
    public int Row { get; } = row;
    public int Column { get; } = column;

    public static GridPoint operator +(GridPoint left, GridPoint right)
    {
        return new GridPoint(left.Row + right.Row, left.Column + right.Column);
    }

    public static GridPoint operator -(GridPoint left, GridPoint right)
    {
        return new GridPoint(left.Row - right.Row, left.Column - right.Column);
    }

    public static bool operator ==(GridPoint left, GridPoint right)
    {
        return left.Row == right.Row && left.Column == right.Column;
    }

    public static bool operator !=(GridPoint left, GridPoint right)
    {
        return left.Row != right.Row || left.Column != right.Column;
    }

    public override string ToString()
    {
        return $"({Row}, {Column})";
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public bool Equals(GridPoint? other)
    {
        return other is not null && this == other;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        try
        {
            return Equals((GridPoint)obj);
        }
        catch (InvalidCastException)
        {
            return false;
        }
    }
}