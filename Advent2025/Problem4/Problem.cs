

namespace Advent2025.Problem4;

public class Problem(string filename = @"data\problem4-input.txt") : IProblem
{
  private const char RollChar = '@';
  private const char MarkChar = 'x';
  private const char EmptyChar = '.';

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var map = Map.FromLines(lines);
    SolvePart1(map, false);
    SolvePart2(map, false);
  }

  private static void SolvePart2(Map map, bool debug)
  {
    var count = FindNonReducibleCount(map, 3, debug);
    Console.WriteLine($"Part 2 answer is: {count}");
  }

  private static int FindNonReducibleCount(Map map, int maxAdjacentRolls, bool debug)
  {
    int totalCount = 0;
    while (true)
    {
      if (debug)
      {
        Console.WriteLine("Current map:");
        map.DumpConsole();
      }

      var updatedMap = MarkAccessibleRolls(map, maxAdjacentRolls);

      if (debug)
      {
        Console.WriteLine("Marked map:");
        updatedMap.DumpConsole();
      }

      int count = updatedMap.Count(MarkChar);

      if (debug)
      {
        Console.WriteLine($"Found {count} accessible rolls");
      }

      if (count == 0)
      {
        return totalCount;
      }

      totalCount += count;
      updatedMap.Replace(MarkChar, EmptyChar);
      map = updatedMap;
    }

    throw new NotImplementedException();
  }

  private static Map MarkAccessibleRolls(Map map, int maxAdjacentRolls)
  {
    var updatedMap = new Map(map);

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
            updatedMap.SetLocation(row, col, MarkChar);
          }
        }
      }
    }

    return updatedMap;
  }

  private static void SolvePart1(Map map, bool debug)
  {
    var count = CountAccessibleRolls(map, 3, debug);
    Console.WriteLine($"Part 1 answer is: {count}");
  }

  private static int CountAccessibleRolls(Map map, int maxAdjacentRolls, bool debug)
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
            debugCopy.SetLocation(row, col, MarkChar);
            total++;
          }
        }
      }
    }

    if (debug)
    {
      debugCopy.DumpConsole();
    }

    return total;
  }
}
