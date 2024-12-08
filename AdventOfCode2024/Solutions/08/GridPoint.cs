namespace AdventOfCode2024.Solutions._08;

public class GridPoint(int row, int column)
{
    public int Row { get; set; } = row;
    public int Column { get; set; } = column;

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
}