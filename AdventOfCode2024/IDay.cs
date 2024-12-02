namespace AdventOfCode2024;

/// <summary>
/// Basic interface that all Advent Of Code days should take from.
/// </summary>
/// <typeparam name="T">Type of the first part's solution. This is often a number.</typeparam>
/// <typeparam name="V">Type of the second part's solution. This is often a number.</typeparam>
public interface IDay<T, V>
{
    T PartOne();
    V PartTwo();
}
