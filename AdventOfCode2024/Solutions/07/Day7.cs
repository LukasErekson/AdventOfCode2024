namespace AdventOfCode2024.Solutions;

public class Day7 : IDay
{
    private string _inputFilePath;
    private long _sumOfPart1;

    public Day7(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        return $"The sum of valid questions is: {_sumOfPart1}.";
    }

    public string PartTwo()
    {
        throw new NotImplementedException();
    }

    public void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                var resultAndOperands = line.Split(": ");
                var result = long.Parse(resultAndOperands[0]);
                var operands = resultAndOperands[1].Split(' ').Select(long.Parse);

                if (ValidEquationWithAddOrMultiply(result, operands))
                {
                    _sumOfPart1 += result;
                }
            }
        }
    }

    private bool ValidEquationWithAddOrMultiply(long testResult, IEnumerable<long> operands)
    {
        var operators = new List<Func<long, long, long>> { (x, y) => x * y, (x, y) => x + y };

        var operations = operands.Count() - 1;

        foreach (var permutation in GetPermutationsWithRept(operators, operations))
        {
            var value = operands.First();
            var remainingOperands = operands.Skip(1);

            for (int i = 0; i < operations; i++)
            {
                value = permutation.ElementAt(i)(value, remainingOperands.ElementAt(i));

                if (value > testResult)
                {
                    break;
                }
            }

            if (value == testResult)
            {
                return true;
            }
        }

        return false;
    }

    // Code from Pengyang, https://stackoverflow.com/questions/1952153/what-is-the-best-way-to-find-all-combinations-of-items-in-an-array
    static IEnumerable<IEnumerable<T>> GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetPermutationsWithRept(list, length - 1)
            .SelectMany(t => list,
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }
}
