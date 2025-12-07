
namespace Advent2025.Problem4
{
  internal class Map
  {
    public int Rows { get; init; }

    public int Cols { get; init; }

    private char[,] _locations;

    public static Map FromLines(string[] lines)
    {
      (int rows, int cols) = GetSize(lines);
      var locations = GetLocations(lines);
      return new Map(rows, cols, locations);
    }

    public Map(Map map)
    {
      Rows = map.Rows;
      Cols = map.Cols;
      _locations = CopyLocations(map._locations);
    }

    public int CountAdjacent(int row, int col, char matchingChar)
    {
      int count = 0;

      count = DoesLocationMatch(row - 1, col - 1, matchingChar) ? count + 1 : count;
      count = DoesLocationMatch(row - 1, col, matchingChar) ? count + 1 : count;
      count = DoesLocationMatch(row - 1, col + 1, matchingChar) ? count + 1 : count;
      count = DoesLocationMatch(row, col - 1, matchingChar) ? count + 1 : count;
      count = DoesLocationMatch(row, col + 1, matchingChar) ? count + 1 : count;
      count = DoesLocationMatch(row + 1, col - 1, matchingChar) ? count + 1 : count;
      count = DoesLocationMatch(row + 1, col, matchingChar) ? count + 1 : count;
      count = DoesLocationMatch(row + 1, col + 1, matchingChar) ? count + 1 : count;

      return count;
    }

    public char GetLocation(int row, int col)
    {
      return _locations[row, col];
    }

    public void SetLocation(int row, int col, char c)
    {
      _locations[row, col] = c;
    }

    public void DumpConsole()
    {
      for (var row = 0; row < Rows; row++)
      {
        for (var col = 0; col < Cols; col++)
        {
          Console.Write(GetLocation(row, col));
        }
        Console.WriteLine();
      }
    }

    private static char[,] CopyLocations(char[,] locations)
    {
      var rows = locations.GetLength(0);
      var cols = locations.GetLength(1);

      var copy = new char[rows, cols];
      for (var row = 0; row < rows; row++)
      {
        for (var col = 0; col < cols; col++)
        {
          copy[row, col] = locations[row, col];
        }
      }

      return copy;
    }

    private Map(int rows, int cols, char[,] locations)
    {
      Rows = rows;
      Cols = cols;
      _locations = locations;
    }

    private bool DoesLocationMatch(int row, int col, char matchingChar)
    {
      if (!DoesLocationExist(row, col))
      {
        return false;
      }

      return GetLocation(row, col) == matchingChar;
    }

    private bool DoesLocationExist(int row, int col)
    {
      return row >= 0 && row < Rows && col >= 0 && col < Cols;
    }

    private static char[,] GetLocations(string[] lines)
    {
      int rows = lines.Length;
      int cols = lines[0].Length;

      var locations = new char[rows, cols];

      for (int row=0; row<rows; row++)
      {
        for (int col=0; col<cols; col++)
        {
          locations[row, col] = lines[row][col];
        }
      }

      return locations;
    }

    private static (int rows, int cols) GetSize(string[] lines)
    {
      if (lines.Any(l => l.Length != lines[0].Length))
      {
        throw new InvalidDataException("Invalid input - all lines must be the same length");
      }

      return (lines.Length, lines[0].Length);
    }
  }
}
