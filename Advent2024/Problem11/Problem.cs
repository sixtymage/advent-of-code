namespace Advent2024.Problem11;

public class Problem(string filename = @"data\problem11-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var stones = lines[0].Split(" ").Select(long.Parse).ToList();

    const int maxNumBlinks = 25;
    for (var i = 0; i < maxNumBlinks; i++)
    {
      stones = Blink(stones);
    }

    Console.WriteLine($"After {maxNumBlinks} blinks there are {stones.Count} stones");
  }

  private static List<long> Blink(List<long> stones)
  {
    var newStones = new List<long>();
    foreach (var stone in stones)
    {
      newStones.AddRange(ApplyRules(stone));
    }

    return newStones;
  }

  private static IEnumerable<long> ApplyRules(long stone)
  {
    if (stone == 0)
    {
      return [1];
    }

    var strStone = $"{stone}";
    if (strStone.Length % 2 != 0)
    {
      return [stone * 2024];
    }

    var left = strStone[..(strStone.Length / 2)];
    var right = strStone[(strStone.Length / 2)..];
    return [long.Parse(left), long.Parse(right)];
  }
}
