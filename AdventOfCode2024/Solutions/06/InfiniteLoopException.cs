namespace AdventOfCode2024.Solutions;
public class InfiniteLoopException : Exception
{
    public InfiniteLoopException() { }
    public InfiniteLoopException(string message) : base(message) { }
    public InfiniteLoopException(string message, Exception innerException) : base(message, innerException) { }
}
