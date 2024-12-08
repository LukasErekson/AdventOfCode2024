namespace AdventOfCode2024.Solutions;

public class Day7 : IDay
{
    private string _inputFilePath;
    private long _sumWithAddAndMultiply;
    private long _sumWithConcatenate;

    public Day7(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        return $"The sum of valid questions is with add and multiply is: {_sumWithAddAndMultiply}.";
    }

    public string PartTwo()
    {
        return $"The sum of valid equations with add, multiply, and concatinate is: {_sumWithAddAndMultiply + _sumWithConcatenate}";
    }

    public void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            static long multiply(long x, long y) => x * y;
            static long add(long x, long y) => x + y;
            static long concatenate(long x, long y) => long.Parse(x.ToString() + y.ToString());

            List<Func<long, long, long>> part1List = [multiply, add];
            List<Func<long, long, long>> part21List = [concatenate, multiply, add];

            var operators = new List<Func<long, long, long>> { (x, y) => x * y, (x, y) => x + y };

            while ((line = streamReader.ReadLine()) != null)
            {
                var resultAndOperands = line.Split(": ");
                var result = long.Parse(resultAndOperands[0]);
                var operands = resultAndOperands[1].Split(' ').Select(long.Parse);

                if (ValidEquationWithWithOperators(result, operands, part1List))
                {
                    _sumWithAddAndMultiply += result;
                }
                else if (ValidEquationWithWithOperators(result, operands, part21List))
                {
                    _sumWithConcatenate += result;
                }
            }
        }
    }

    private bool ValidEquationWithWithOperators(long testResult, IEnumerable<long> operands, IEnumerable<Func<long, long, long>> operators)
    {
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
        if (length == 1)
        {
            return list.Select(t => new T[] { t });
        }

        return GetPermutationsWithRept(list, length - 1).SelectMany(t => list, (t1, t2) => t1.Concat([t2]));
    }
}
