namespace Advent2024.Problem1;

public class Problem(string filename = @"data\problem1-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var (first, second) = ExtractLists(lines);

    SortLists(first, second);
    ValidateLengths(first, second);

    var distanceScore = CalculateDistanceScore(first, second);
    var similarityScore = CalculateSimilarityScore(first, second);

    Console.WriteLine($"Total distance is: {distanceScore}");
    Console.WriteLine($"Similarity score is: {similarityScore}");
  }

  private object CalculateSimilarityScore(int[] first, int[] second)
  {
    var total = 0L;
    for (var i = 0; i < first.Length; i++)
    {
      var count = second.Count(x => x == first[i]);
      total += first[i] * count;
    }

    return total;
  }

  private static long CalculateDistanceScore(int[] first, int[] second)
  {
    var total = 0L;
    for (var i=0; i<first.Length; i++)
    {
      var delta = Math.Abs(first[i] - second[i]);
      total += delta;
    }

    return total;
  }

  private static void ValidateLengths(int[] first, int[] second)
  {
    if (first.Length != second.Length)
    {
      throw new InvalidDataException("Input arrays do not have the same length");
    }
  }

  private static void SortLists(int[] first, int[] second)
  {
    Array.Sort(first);
    Array.Sort(second);
  }

  private static (int[], int[]) ExtractLists(string[] lines)
  {
    var numbers = lines
      .Select(x =>
      {
        var s = x.Split("   ");
        return (int.Parse(s[0]), int.Parse(s[1]));
      })
      .ToArray();

    var first = numbers.Select(x => x.Item1).ToArray();
    var second = numbers.Select(x => x.Item2).ToArray();
    return (first, second);
  }
}
