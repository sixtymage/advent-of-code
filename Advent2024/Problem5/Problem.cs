namespace Advent2024.Problem5;

public class Problem(string filename = @"data\problem5-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var (rules, updates) = ExtractRulesAndUpdates(lines);

    SolvePart1(rules, updates);
    SolvePart2(rules, updates);
  }

  private static void SolvePart2(PageOrderingRule[] rules, ManualUpdate[] updates)
  {
    var correctlyOrderedUpdates = FindAndCorrectIncorrectlyOrderedUpdates(rules, updates);
    var middlePageSum = CalculateMiddlePageSum(correctlyOrderedUpdates);
    Console.WriteLine($"Middle page sum (Part 2): {middlePageSum}");
  }

  private static List<ManualUpdate> FindAndCorrectIncorrectlyOrderedUpdates(PageOrderingRule[] rules, ManualUpdate[] updates)
  {
    var correctlyOrderedUpdates = new List<ManualUpdate>();
    foreach (var update in updates)
    {
      var applicableRules = FindApplicableRules(rules, update).ToArray();
      if (AreRulesSatisfied(applicableRules, update))
      {
        continue;
      }

      var correctlyOrderedUpdate = CorrectUpdate(applicableRules, update);
      correctlyOrderedUpdates.Add(correctlyOrderedUpdate);
    }

    return correctlyOrderedUpdates;
  }

  private static ManualUpdate CorrectUpdate(PageOrderingRule[] rules, ManualUpdate incorrectlyOrderedUpdate)
  {
    while (true)
    {
      var violatedRule = FindViolatedRule(rules, incorrectlyOrderedUpdate);
      if (violatedRule is null)
      {
        return incorrectlyOrderedUpdate;
      }

      var candidateReorderedPages = violatedRule.GetCandidateCorrection(incorrectlyOrderedUpdate.Pages);
      incorrectlyOrderedUpdate = new ManualUpdate(candidateReorderedPages);
    }
  }

  private static PageOrderingRule? FindViolatedRule(PageOrderingRule[] rules, ManualUpdate update)
  {
    return rules.FirstOrDefault(rule => !rule.IsSatisfied(update.Pages));
  }

  private static void SolvePart1(PageOrderingRule[] rules, ManualUpdate[] updates)
  {
    var correctlyOrderedUpdates = FindCorrectlyOrderedUpdates(rules, updates);
    var middlePageSum = CalculateMiddlePageSum(correctlyOrderedUpdates);
    Console.WriteLine($"Middle page sum (Part 1): {middlePageSum}");
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
    var correctlyOrderedUpdates = new List<ManualUpdate>();
    foreach (var update in updates)
    {
      var applicableRules = FindApplicableRules(rules, update).ToArray();
      if (AreRulesSatisfied(applicableRules, update))
      {
        correctlyOrderedUpdates.Add(update);
      }
    }

    return correctlyOrderedUpdates;
  }

  private static bool AreRulesSatisfied(PageOrderingRule[] applicableRules, ManualUpdate update)
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
