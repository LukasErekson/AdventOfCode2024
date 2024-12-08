namespace AdventOfCode2024.Solutions._06;

public class Guard
{
    public char GuardDirection { get; set; }
    public int[] Position { get; set; } = new int[2];

    public static char[] GuardChars { get; } = ['^', '>', 'v', '<'];
    public static readonly Dictionary<char, int[]> DirectionToMove = new()
    {
        { '^', new int[2] { -1, 0 } },
        { '>', new int[2] { 0, 1 } },
        { 'v', new int[2] { 1, 0 } },
        { '<', new int[2] { 0, -1 } },
    };

    public Guard(int row, int column, char initialDirection)
    {
        Position[0] = row;
        Position[1] = column;
        GuardDirection = initialDirection;
    }

    public void MoveForward()
    {
        var movementSteps = DirectionToMove[GuardDirection];
        Position[0] += movementSteps[0];
        Position[1] += movementSteps[1];
    }

    public int[] NextMove()
    {
        var movementSteps = DirectionToMove[GuardDirection];
        return [Position[0] + movementSteps[0], Position[1] + movementSteps[1]];
    }

    public void Turn()
    {
        GuardDirection = GuardChars[(Array.IndexOf(GuardChars, GuardDirection) + 1) % 4];
    }

}
