using _01.Day1;

var inputFilePath = "../01/inputs/part_1_input.txt";
var solution = new Day1(inputFilePath);

Console.WriteLine($"The total distance between the two lists is: {solution.PartOne()}");
Console.WriteLine($"The similarity score of the two lists is: {solution.PartTwo()}");