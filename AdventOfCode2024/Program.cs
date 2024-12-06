using AdventOfCode2024;
using AdventOfCode2024.Solution._01.Day1;
using AdventOfCode2024.Solution._03.Day3;
using AdventOfCode2024.Solution_02.Day2;
using AdventOfCode2024.Solutions._04;
using AdventOfCode2024.Solutions._05;
using AdventOfCode2024.Solutions._06;

var completedDays = new List<IDay>()
    {
        new Day1("./Inputs/01/part_1_input.txt"),
        new Day2("./Inputs/02/report_levels.txt"),
        new Day3("./Inputs/03/input.txt"),
        new Day4("./Inputs/04/input_part1.txt"),
        new Day5("./Inputs/05/input.txt"),
        new Day6("./Inputs/06/input.txt"),
    };


for (int day = 0; day < completedDays.Count; day++)
{
    Console.WriteLine($"Day {day + 1}");
    Console.Write($"===================================");
    Console.WriteLine(completedDays.ElementAt(day).Solution());
    Console.WriteLine();
}