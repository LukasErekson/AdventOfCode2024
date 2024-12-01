
using _01.Day1;

var inputFileName = "../part_1_input.txt";
var solution = new Day1();

if (File.Exists(inputFileName))
{
    using var streamReader = new StreamReader(inputFileName);
    string? line;

    while ((line = streamReader.ReadLine()) != null)
    {
        solution.ProcessLine(line);
    }

}

Console.WriteLine($"The total distance between the two lists is: {solution.CalculateDistance()}");
Console.WriteLine($"The similarity score of the two lists is: {solution.CalculateSimilarityScore()}");