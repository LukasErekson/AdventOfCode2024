namespace _01.Day1
{
    public class Day1
    {
        public Dictionary<int, int> LocationIdToFreq1 { get; set; } = [];
        public Dictionary<int, int> LocationIdToFreq2 { get; set; } = [];

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

            LocationIdToFreq1.TryGetValue(locId1, out int currentCount1);
            LocationIdToFreq1[locId1] = currentCount1 + 1;

            LocationIdToFreq2.TryGetValue(locId2, out int currentCount2);
            LocationIdToFreq2[locId2] = currentCount2 + 1;
        }


        public int CalculateDistance()
        {
            int totalDistance = 0;
            var firstLocIdKeys = LocationIdToFreq1.Keys.Order().ToList();
            var secondLocIdKeys = LocationIdToFreq2.Keys.Order().ToList();

            var locIdToFreq1Copy = LocationIdToFreq1.ToDictionary((entry) => entry.Key, (entry) => entry.Value);
            var locIdToFreq2Copy = LocationIdToFreq2.ToDictionary((entry) => entry.Key, (entry) => entry.Value);

            var currentLocId1Key = 0;
            var currentLocId2Key = 0;

            while (true)
            {
                if (currentLocId1Key == firstLocIdKeys.Count)
                {
                    break;
                }

                int locId1Key = firstLocIdKeys[currentLocId1Key];
                int locId2Key = secondLocIdKeys[currentLocId2Key];


                int valueToAdd = Math.Abs(locId1Key - locId2Key);
                int timesToAdd = Math.Min(locIdToFreq1Copy[locId1Key], locIdToFreq2Copy[locId2Key]);

                totalDistance += valueToAdd * timesToAdd;

                locIdToFreq1Copy[locId1Key] -= timesToAdd;
                locIdToFreq2Copy[locId2Key] -= timesToAdd;

                if (locIdToFreq1Copy[locId1Key] == 0)
                {
                    currentLocId1Key += 1;
                }

                if (locIdToFreq2Copy[locId2Key] == 0)
                {
                    currentLocId2Key += 1;
                }

            }

            return totalDistance;
        }

        public int CalculateSimilarityScore()
        {
            int similarityScore = 0;

            foreach (var locId in LocationIdToFreq1.Keys)
            {
                LocationIdToFreq2.TryGetValue(locId, out int secondListFrequency);

                similarityScore += locId * secondListFrequency * LocationIdToFreq1[locId];
            }

            return similarityScore;
        }
    }
}