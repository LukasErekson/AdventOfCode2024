namespace Utilities.UtilityClasses;

public class GridPoint(int row, int column) : IEquatable<GridPoint>
{
    public int Row { get; } = row;
    public int Column { get; } = column;
    public static readonly List<GridPoint> DirectionsNoDiagonals = [
        new GridPoint(1, 0), // down 
        new GridPoint(-1, 0), // up 
        new GridPoint(0, -1), // left
        new GridPoint(0, 1), // right
    ];
    public static readonly List<GridPoint> DirectionsDiagonals = [
        new GridPoint(1, 1), // down-right
        new GridPoint(-1, 1), // up-right 
        new GridPoint(1, -1), // down-left
        new GridPoint(-1, -1), // up-left
    ];

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

    public static bool WithinBounds(GridPoint point, int rows, int cols)
    {
        return point.Row >= 0
               && point.Column >= 0
               && point.Row < rows
               && point.Column < cols;
    }

    public static List<GridPoint> Burst(GridPoint point, bool includeDiagonals = false)
    {
        List<GridPoint> burstLocations = [];

        foreach (var direction in DirectionsNoDiagonals)
        {
            burstLocations.Add(point + direction);
        }

        if (includeDiagonals)
        {
            burstLocations = [.. burstLocations, .. DiagonalBurst(point)];
        }

        return burstLocations;
    }

    public static List<GridPoint> DiagonalBurst(GridPoint point)
    {
        List<GridPoint> burstLocations = [];

        foreach (var direction in DirectionsNoDiagonals)
        {
            burstLocations.Add(point + direction);
        }

        return burstLocations;
    }

    public static List<GridPoint> BurstWithinBounds(GridPoint point, int row, int col, bool includeDiagonals = false)
    {
        return Burst(point, includeDiagonals).Where(p => WithinBounds(p, row, col)).ToList();
    }

    public static List<GridPoint> DiagonalBurstWithinBounds(GridPoint point, int row, int col)
    {
        return DiagonalBurst(point).Where(p => WithinBounds(p, row, col)).ToList();
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