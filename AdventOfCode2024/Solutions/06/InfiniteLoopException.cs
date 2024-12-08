namespace AdventOfCode2024.Solutions._06;
public class InfiniteLoopException : Exception
{
    public InfiniteLoopException() { }
    public InfiniteLoopException(string message) : base(message) { }
    public InfiniteLoopException(string message, Exception innerException) : base(message, innerException) { }
}
