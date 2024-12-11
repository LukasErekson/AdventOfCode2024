namespace InputUtilities;

public class GridInput
{

    public static void ReadIntoGrid(string inputFilePath, Action<char, int, int> processChar, out int row, out int col)
    {
        row = 0;
        col = 0;
        if (File.Exists(inputFilePath))
        {
            using var streamReader = new StreamReader(inputFilePath);
            int value;
            int rows = 0;
            int cols = 0;

            while ((value = streamReader.Read()) != -1)
            {
                if (value == '\r')
                {
                    continue;
                }

                if (value == '\n')
                {
                    rows++;
                    col = cols;
                    cols = 0;
                    continue;
                }

                processChar((char)value, rows, cols);

                cols++;
            }

            row = rows + 1;
        }
        else
        {
            Console.WriteLine($"File {inputFilePath} cannot be found.");
        }

    }

}
