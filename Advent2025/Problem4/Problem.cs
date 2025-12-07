namespace Advent2025.Problem4;

public class Problem(string filename = @"data\problem4-input.txt") : IProblem
{
  private const char RollChar = '@';
  private const char DebugChar = 'x';

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var map = Map.FromLines(lines);
    var count = CountAccessibleRolls(map, 3);

    Console.WriteLine($"Answer is: {count}");
  }

  private static int CountAccessibleRolls(Map map, int maxAdjacentRolls)
  {
    var debugCopy = new Map(map);

    int total = 0;

    for (int row = 0; row < map.Rows; row++)
    {
      for (int col = 0; col < map.Cols; col++)
      {
        var mapChar = map.GetLocation(row, col);
        if (mapChar == RollChar)
        {
          var count = map.CountAdjacent(row, col, RollChar);

          if (count <= maxAdjacentRolls)
          {
            debugCopy.SetLocation(row, col, DebugChar);
            total++;
          }
        }
      }
    }

    debugCopy.DumpConsole();

    return total;
  }
}
