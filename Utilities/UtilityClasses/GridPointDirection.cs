namespace Utilities.UtilityClasses;

public enum Directions
{
    Up,
    Down,
    Left,
    Right,
}

public class GridPointDirection : IEquatable<GridPointDirection>
{
    public GridPoint Location { get; }
    public Directions Direction { get; }

    public static readonly Dictionary<Directions, Directions> DirectionToOppositeDirection = new()
    {
        { Directions.Up, Directions.Down },
        { Directions.Down, Directions.Up },
        { Directions.Left, Directions.Right },
        { Directions.Right, Directions.Left },
    };

    public GridPointDirection(GridPoint location, Directions direction)
    {
        Location = location;
        Direction = direction;
    }

    public GridPointDirection(long x, long y, Directions direction)
    {
        Location = new GridPoint(x, y);
        Direction = direction;
    }

    public bool Equals(GridPointDirection? other)
    {
        return other is not null && Location == other.Location && Direction == other.Direction;
    }

    public override string ToString()
    {
        return $"{Location}, {Direction}";
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}
