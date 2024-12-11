namespace InputUtilities;

public class GridInput
{

    public static void ReadByCharAndOutputBoundaries(string inputFilePath, Action<char, int, int> processChar, out int rowCount, out int columnCount)
    {
        rowCount = 0;
        columnCount = 0;
        if (File.Exists(inputFilePath))
        {
            using var streamReader = new StreamReader(inputFilePath);
            int value;
            int currentRow = 0;
            int currentColumn = 0;

            while ((value = streamReader.Read()) != -1)
            {
                if (value == '\r')
                {
                    continue;
                }

                if (value == '\n')
                {
                    currentRow++;
                    columnCount = currentColumn;
                    currentColumn = 0;
                    continue;
                }

                processChar((char)value, currentRow, currentColumn);

                currentColumn++;
            }

            rowCount = currentRow + 1;
        }
        else
        {
            Console.WriteLine($"File {inputFilePath} cannot be found.");
        }

    }

}
