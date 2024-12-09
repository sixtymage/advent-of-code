namespace Advent2024.Problem5;

public class Problem(string filename = @"data\problem5-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var (rules, updates) = ExtractRulesAndUpdates(lines);

    var correctlyOrderedUpdates = FindCorrectlyOrderedUpdates(rules, updates);
    var middlePageSum = CalculateMiddlePageSum(correctlyOrderedUpdates);
    Console.WriteLine($"Similarity score is: {middlePageSum}");
  }

  private static (Rule[] rules, Update[] updates) ExtractRulesAndUpdates(string[] lines)
  {
    throw new NotImplementedException();
  }

  private static List<Update> FindCorrectlyOrderedUpdates(object rules, object updates)
  {
    throw new NotImplementedException();
  }

  private static int CalculateMiddlePageSum(object correctlyOrderedUpdates)
  {
    throw new NotImplementedException();
  }
}
