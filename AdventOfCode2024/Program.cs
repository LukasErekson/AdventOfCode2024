using AdventOfCode2024;
using AdventOfCode2024.Solutions;


if (args.Length == 0 || string.Equals(args[0], "all", StringComparison.InvariantCultureIgnoreCase))
{
    var completedDays = new List<IDay>()
        {
            new Day1("./Inputs/01/input.txt"),
            new Day2("./Inputs/02/input.txt"),
            new Day3("./Inputs/03/input.txt"),
            new Day4("./Inputs/04/input.txt"),
            new Day5("./Inputs/05/input.txt"),
            new Day6("./Inputs/06/input.txt"),
            new Day7("./Inputs/07/input.txt"),
            new Day8("./Inputs/08/input.txt"),
            new Day9("./Inputs/09/input.txt"),
            new Day10("./Inputs/10/input.txt"),
            new Day11("./Inputs/11/input.txt"),
            new Day12("./Inputs/12/input.txt"),
            new Day13("./Inputs/13/input.txt"),
        };

    for (int day = 0; day < completedDays.Count; day++)
    {
        Console.WriteLine($"Day {day + 1}");
        Console.Write($"===================================");
        Console.WriteLine(completedDays.ElementAt(day).Solution());
        Console.WriteLine();
    }

}
else
{
    var dayNum = int.Parse(args[0]);
    var inputFileString = $"./Inputs/{dayNum:00}/input.txt";

    if (args.Length == 2 && string.Equals(args[1], "sample", StringComparison.InvariantCultureIgnoreCase))
    {
        inputFileString = $"./Inputs/{dayNum:00}/sample_input.txt";
    }
    else if (args.Length == 2)
    {
        inputFileString = $"./Inputs/{dayNum:00}/{args[1]}";
    }

    IDay buildDay(int dayNumber)
    {
        return dayNumber switch
        {
            1 => new Day1(inputFileString),
            2 => new Day2(inputFileString),
            3 => new Day3(inputFileString),
            4 => new Day4(inputFileString),
            5 => new Day5(inputFileString),
            6 => new Day6(inputFileString),
            7 => new Day7(inputFileString),
            8 => new Day8(inputFileString),
            9 => new Day9(inputFileString),
            10 => new Day10(inputFileString),
            11 => new Day11(inputFileString),
            12 => new Day12(inputFileString),
            13 => new Day13(inputFileString),
            _ => throw new NotImplementedException($"Day {dayNumber} is not implemented yet!"),
        };
    }
    try
    {
        IDay day = buildDay(int.Parse(args[0]));

        Console.WriteLine($"Day {dayNum}");
        Console.Write($"===================================");
        Console.WriteLine(day.Solution());
        Console.WriteLine();
    }
    catch (NotImplementedException notImplementedError)
    {
        Console.WriteLine(notImplementedError.Message);
    }
}