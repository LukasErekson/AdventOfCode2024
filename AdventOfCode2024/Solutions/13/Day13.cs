using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;
using Utilities.UtilityClasses;
using AdventOfCode2024.Solutions._13;

namespace AdventOfCode2024.Solutions;

public class Day13 : IDay
{
    private readonly string _inputFilePath;
    private readonly List<ClawMachine> _clawMachines = [];

    public Day13(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        ProcessInput();
    }

    public string PartOne()
    {
        var totalTokens = 0;

        foreach (var clawMachine in _clawMachines)
        {
            try
            {
                var solution = SolveClawMachine(clawMachine);

                var aPresses = solution.Row;
                var bPresses = solution.Column;

                if (aPresses < 0 || aPresses > 100 || bPresses < 0 || bPresses > 100)
                {
                    continue;
                }

                totalTokens += 3 * aPresses + bPresses;
            }
            catch (Exception)
            {
                continue;
            }
        }

        return $"The total token cost would be {totalTokens}.";
    }

    public string PartTwo()
    {
        long totalTokens = 0;

        foreach (var clawMachine in _clawMachines)
        {
            try
            {
                var solution = SolveClawMachinePart2(clawMachine);

                var aPresses = solution[0];
                var bPresses = solution[1];

                totalTokens += 3 * aPresses + bPresses;
            }
            catch (Exception)
            {
                continue;
            }
        }

        return $"The total token cost would be {totalTokens}.";
    }

    private void ProcessInput()
    {
        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.StartsWith("Button A:"))
                {
                    var aButtonOffsets = Regex.Matches(line, @"\d+");
                    var aButton = new GridPoint(int.Parse(aButtonOffsets[0].Value), int.Parse(aButtonOffsets[1].Value));

                    line = streamReader.ReadLine();
                    if (line != null)
                    {
                        var bButtonOffsets = Regex.Matches(line, @"\d+");
                        var bButton = new GridPoint(int.Parse(bButtonOffsets[0].Value), int.Parse(bButtonOffsets[1].Value));

                        line = streamReader.ReadLine();
                        if (line != null)
                        {
                            var prizeCoordinates = Regex.Matches(line, @"\d+");
                            var prizeLocation = new GridPoint(int.Parse(prizeCoordinates[0].Value), int.Parse(prizeCoordinates[1].Value));

                            _clawMachines.Add(new ClawMachine
                            {
                                ButtonA = aButton,
                                ButtonB = bButton,
                                PrizeLocation = prizeLocation,
                            });
                        }
                    }
                }
            }
        }
    }

    private static GridPoint SolveClawMachine(ClawMachine clawMachine)
    {
        var matrixArray = new double[2, 2]
        {
            { clawMachine.ButtonA.Row, clawMachine.ButtonB.Row },
            { clawMachine.ButtonA.Column, clawMachine.ButtonB.Column }
        };
        var M = Matrix<double>.Build;
        var V = Vector<double>.Build;

        var matrix = M.DenseOfArray(matrixArray);
        var prize = V.DenseOfArray([clawMachine.PrizeLocation.Row, clawMachine.PrizeLocation.Column]);

        var matrixInverse = matrix.Inverse();

        var solution = matrixInverse.Multiply(prize);

        if (IsInteger(solution.At(0)) && IsInteger(solution.At(1)))
        {
            return new GridPoint((int)Math.Round(solution.At(0)), (int)Math.Round(solution.At(1)));
        }

        throw new Exception("No Integer solution");
    }

    private static long[] SolveClawMachinePart2(ClawMachine clawMachine)
    {
        var matrixArray = new double[2, 2]
        {
            { clawMachine.ButtonA.Row, clawMachine.ButtonB.Row },
            { clawMachine.ButtonA.Column, clawMachine.ButtonB.Column }
        };
        var M = Matrix<double>.Build;
        var V = Vector<double>.Build;

        var matrix = M.DenseOfArray(matrixArray);
        var prize = V.DenseOfArray([clawMachine.PrizeLocation.Row + 10000000000000, clawMachine.PrizeLocation.Column + 10000000000000]);

        var matrixInverse = matrix.Inverse();

        var solution = matrixInverse.Multiply(prize);

        if (IsInteger(solution.At(0)) && IsInteger(solution.At(1)))
        {
            return [(long)Math.Round(solution.At(0)), (long)Math.Round(solution.At(1))];
        }

        throw new Exception("No Integer solution");
    }

    public static bool IsInteger(double number)
    {
        var asString = number.ToString("0.##");
        return !asString.Contains('.') || asString[asString.IndexOf('.')..] == "00";
    }
}
