using Utilities.UtilityClasses;
namespace AdventOfCode2024.Solutions._13;

public class ClawMachine
{
    public required GridPoint ButtonA { get; set; }
    public required GridPoint ButtonB { get; set; }
    public required GridPoint PrizeLocation { get; set; }


    public override string ToString()
    {
        return $"A: {ButtonA}, B: {ButtonB}, Prize: {PrizeLocation}";
    }
}
