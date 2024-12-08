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

    IDay buildDay(int dayNumber)
    {
        switch (dayNumber)
        {
            case 1:
                return new Day1(inputFileString);
            case 2:
                return new Day2(inputFileString);
            case 3:
                return new Day3(inputFileString);
            case 4:
                return new Day4(inputFileString);
            case 5:
                return new Day5(inputFileString);
            case 6:
                return new Day6(inputFileString);
            case 7:
                return new Day7(inputFileString);
        }
        throw new NotImplementedException($"Day {dayNumber} is not implemented yet!");
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