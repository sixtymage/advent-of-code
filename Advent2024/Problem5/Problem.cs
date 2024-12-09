namespace Advent2024.Problem5;

public class Problem(string filename = @"data\problem5-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var (rules, updates) = ExtractRulesAndUpdates(lines);

    var correctlyOrderedUpdates = FindCorrectlyOrderedUpdates(rules, updates);
    var middlePageSum = CalculateMiddlePageSum(correctlyOrderedUpdates);
    Console.WriteLine($"Middle page sum: {middlePageSum}");
  }

  private static (PageOrderingRule[] rules, ManualUpdate[] updates) ExtractRulesAndUpdates(string[] lines)
  {
    var rules = new List<PageOrderingRule>();
    var updates = new List<ManualUpdate>();

    var expectRule = true;
    foreach (var line in lines)
    {
      if (line.Length == 0)
      {
        expectRule = false;
        continue;
      }

      if (expectRule)
      {
        rules.Add(ExtractRule(line));
      }
      else
      {
        updates.Add(ExtractUpdate(line));
      }
    }
    return (rules.ToArray(), updates.ToArray());
  }

  private static PageOrderingRule ExtractRule(string line)
  {
    var split = line.Split("|");
    var before = int.Parse(split[0]);
    var after = int.Parse(split[1]);
    return new PageOrderingRule(before, after);
  }

  private static ManualUpdate ExtractUpdate(string line)
  {
    var pages = line.Split(",").Select(int.Parse).ToArray();
    return new ManualUpdate(pages);
  }

  private static List<ManualUpdate> FindCorrectlyOrderedUpdates(PageOrderingRule[] rules, ManualUpdate[] updates)
  {
    var orderedUpdates = new List<ManualUpdate>();
    foreach (var update in updates)
    {
      var applicableRules = FindApplicableRules(rules, update);
      if (AreRulesSatisfied(applicableRules, update))
      {
        orderedUpdates.Add(update);
      }
    }

    return orderedUpdates;
  }

  private static bool AreRulesSatisfied(IEnumerable<PageOrderingRule> applicableRules, ManualUpdate update)
  {
    return applicableRules.All(x => x.IsSatisfied(update.Pages));
  }

  private static IEnumerable<PageOrderingRule> FindApplicableRules(PageOrderingRule[] rules, ManualUpdate update)
  {
    return rules.Where(r => r.IsForPages(update.Pages));
  }

  private static int CalculateMiddlePageSum(List<ManualUpdate> updates)
  {
    return updates.Sum(x => x.MiddlePage);
  }
}
