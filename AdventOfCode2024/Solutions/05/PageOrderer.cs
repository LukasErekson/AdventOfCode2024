using System.Security.Cryptography;

namespace AdventOfCode2024.Solutions._05;
public class PageOrderer : StringComparer
{
    private Dictionary<string, List<string>> _pageNumberToPredecessors = [];

    public PageOrderer(Dictionary<string, List<string>> pageNumberToPredecessors)
    {
        _pageNumberToPredecessors = pageNumberToPredecessors;
    }

    public override int Compare(string? x, string? y)
    {
        var xEntry = new List<string>();
        var yEntry = new List<string>();
        _pageNumberToPredecessors.TryGetValue(x, out xEntry);
        _pageNumberToPredecessors.TryGetValue(y, out yEntry);

        if (xEntry != null && xEntry.Contains(y))
        {
            return -1;
        }
        else if (yEntry != null && yEntry.Contains(x))
        {
            return 1;
        }

        return 0;
    }

    public override bool Equals(string? x, string? y)
    {
        return x == y;
    }

    public override int GetHashCode(string obj)
    {
        return RandomNumberGenerator.GetInt32(int.MaxValue);
    }
}