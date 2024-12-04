namespace AdventOfCode2024;

/// <summary>
/// Basic interface that all Advent Of Code days should take from.
/// </summary>
public interface IDay
{
    string PartOne();
    string PartTwo();
    string Solution()
    {
        var solutionString = "";

        try
        {
            solutionString += $"\n{PartOne()}";
        }
        catch (NotImplementedException)
        {
            solutionString += "\nPart 1 is not implemented yet.";
        }

        try
        {
            solutionString += $"\n{PartTwo()}";
        }
        catch (NotImplementedException)
        {
            solutionString += "\nPart 2 is not implemented yet.";
        }

        return solutionString;
    }
}
