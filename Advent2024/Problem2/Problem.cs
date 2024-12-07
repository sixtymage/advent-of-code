using System.Collections.ObjectModel;

namespace Advent2024.Problem2;

public class Problem(string filename = @"data\problem2-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var reports = ExtractReports(lines);
    var safeReportCount = AnalyseReports(reports.AsReadOnly());

    Console.WriteLine($"The count of safe reports is {safeReportCount}");
  }

  private static int AnalyseReports(ReadOnlyCollection<Report> reports)
  {
    var count = 0;
    foreach (var report in reports)
    {
      if (AreLevelsSafe(report.Levels))
      {
        count++;
      }
    }
    return count;
  }

  private static bool AreLevelsSafe(int[] levels)
  {
    // check difference between adjacent entries is at least 1 and no more than 3if (levels.Length == 0)
    // all pairs are strictly increasing or strictly decreasing
    long? direction = null;
    for (var  i = 1; i < levels.Length; i++)
    {
      long difference = levels[i] - levels[i - 1];
      if (Math.Abs(difference) is < 1 or > 3)
      {
        return false;
      }

      // have we set the direction yet?
      if (direction is not null)
      {
        // the direction and this difference must have the same sign
        if (difference * direction < 0)
        {
          return false;
        }

        continue;
      }

      // if we have a definite direction, set it
      direction = difference != 0 ? difference : null;
    }

    return true;
  }

  private static List<Report> ExtractReports(string[] lines)
  {
    var reports = new List<Report>();
    foreach (var line in lines)
    {
      var levels = line.Split(" ").Select(int.Parse).ToArray();
      ValidateLevels(levels);

      reports.Add(new Report
      {
        Levels = levels,
      });
    }

    return reports;
  }

  private static void ValidateLevels(int[] levels)
  {
    if (levels.Length >= 2)
    {
      return;
    }
    var description = string.Join(", ", levels);
    throw new InvalidDataException($"The number of levels must be at least 2 (levels: [{description}])");
  }
}
