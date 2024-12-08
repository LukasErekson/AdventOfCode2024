namespace AdventOfCode2024.Solutions;
public class Day1 : IDay
{
    private readonly string _inputFilePath;
    private readonly Dictionary<int, int> _leftLocationIdToFrequency = [];
    private readonly Dictionary<int, int> _rightLocationIdToFrequency = [];

    public Day1(string inputFilePath)
    {
        _inputFilePath = inputFilePath;
        BuildDictionaries();
    }

    public string PartOne()
    {
        return $"The total distance between the two lists is: {CalculateDistance()}.";
    }

    public string PartTwo()
    {
        return $"The similarityscore of the two lists is {CalculateSimilarityScore()}.";
    }

    /// <summary>
    /// Build the internal dictionaries that reperesent the counts from the
    /// left and the right list of location IDs.
    /// </summary>
    private void BuildDictionaries()
    {

        if (File.Exists(_inputFilePath))
        {
            using var streamReader = new StreamReader(_inputFilePath);
            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                ProcessLine(line);
            }

        }
    }

    /// <summary>
    /// Add the values from the line to their repsective frequency dictionaries. 
    /// </summary>
    /// <param name="line">A string consisting of 2 integers separated by a delimeter.</param>
    /// <param name="delimiter">The delimiter to use when parsing the integers from line.</param>
    public void ProcessLine(string line, string delimiter = "  ")
    {
        string[] integers = line.Split(delimiter);
        int locId1 = int.Parse(integers[0]);
        int locId2 = int.Parse(integers[1]);

        _leftLocationIdToFrequency.TryGetValue(locId1, out int currentCount1);
        _leftLocationIdToFrequency[locId1] = currentCount1 + 1;

        _rightLocationIdToFrequency.TryGetValue(locId2, out int currentCount2);
        _rightLocationIdToFrequency[locId2] = currentCount2 + 1;
    }

    /// <summary>
    /// Calculate the distance between the left and right location ID lists as determined in
    /// Advent Of Code 2024 day 1, part 1.
    /// See https://adventofcode.com/2024/day/1
    /// </summary>
    /// <returns>The total distance between the left and the right location ID lists</returns>
    private int CalculateDistance()
    {
        int totalDistance = 0;
        var leftLocIds = _leftLocationIdToFrequency.Keys.Order().ToList();
        var rightLocIds = _rightLocationIdToFrequency.Keys.Order().ToList();

        var leftLocIdtoFreqCopy = _leftLocationIdToFrequency.ToDictionary((entry) => entry.Key, (entry) => entry.Value);
        var rightLocIdtoFreqCopy = _rightLocationIdToFrequency.ToDictionary((entry) => entry.Key, (entry) => entry.Value);

        var currentLocId1Key = 0;
        var currentLocId2Key = 0;

        while (true)
        {
            if (currentLocId1Key == leftLocIds.Count)
            {
                break;
            }

            int locId1Key = leftLocIds[currentLocId1Key];
            int locId2Key = rightLocIds[currentLocId2Key];


            int valueToAdd = Math.Abs(locId1Key - locId2Key);
            int timesToAdd = Math.Min(leftLocIdtoFreqCopy[locId1Key], rightLocIdtoFreqCopy[locId2Key]);

            totalDistance += valueToAdd * timesToAdd;

            leftLocIdtoFreqCopy[locId1Key] -= timesToAdd;
            rightLocIdtoFreqCopy[locId2Key] -= timesToAdd;

            if (leftLocIdtoFreqCopy[locId1Key] == 0)
            {
                currentLocId1Key += 1;
            }

            if (rightLocIdtoFreqCopy[locId2Key] == 0)
            {
                currentLocId2Key += 1;
            }

        }

        return totalDistance;
    }

    /// <summary>
    /// Calculate the similarity score between the left and right location ID lists as determined in
    /// of Advent Of Code 2024 day 1, part 2.
    /// See https://adventofcode.com/2024/day/1
    /// </summary>
    /// <returns>The similarity score between the left and the right locaiton ID lists.</returns>
    private int CalculateSimilarityScore()
    {
        int similarityScore = 0;

        foreach (var locId in _leftLocationIdToFrequency.Keys)
        {
            _rightLocationIdToFrequency.TryGetValue(locId, out int secondListFrequency);

            similarityScore += locId * secondListFrequency * _leftLocationIdToFrequency[locId];
        }

        return similarityScore;
    }
}
