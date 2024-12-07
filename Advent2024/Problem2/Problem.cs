using System.Collections.ObjectModel;

namespace Advent2024.Problem2;

public class Problem(string filename = @"data\problem2-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var reports = ExtractReports(lines).AsReadOnly();

    var result1 = AnalyseReports(reports, 0);
    var result2 = AnalyseReports(reports, 1);

    ShowResult(result1);
    ShowResult(result2);
  }

  private static void ShowResult(Result result)
  {
    Console.WriteLine($"With tolerance {result.Tolerance} the number of safe reports is {result.NumSafeReports}");
  }

  private static Result AnalyseReports(ReadOnlyCollection<Report> reports, int tolerance)
  {
    var count = 0;
    foreach (var report in reports)
    {
      if (AreLevelsSafe(report.Levels, tolerance))
      {
        count++;
      }
    }

    return new Result
    {
      Tolerance = tolerance,
      NumSafeReports = count
    };
  }

  private static bool AreLevelsSafe(int[] levels, int tolerance)
  {
    if (AreLevelsSafe(levels))
    {
      return true;
    }

    // if we cannot tolerate any more reading removals we are done
    if (tolerance == 0 || levels.Length == 2)
    {
      return false;
    }

    // otherwise lower the tolerance...
    tolerance--;

    // and try all possible ways of removing a reading
    for (var i = 0; i < levels.Length; i++)
    {
      var alternateLevels = RemoveReading(levels, i);
      if (AreLevelsSafe(alternateLevels, tolerance))
      {
        return true;
      }
    }

    return false;
  }

  private static int[] RemoveReading(int[] levels, int i)
  {
    if (i < 0 || i >= levels.Length)
    {
      throw new ArgumentOutOfRangeException(nameof(i));
    }

    // use the slicing syntax to join the respective parts, skipping the specified index
    return levels[..i].Concat(levels[(i + 1)..]).ToArray();
  }

  private static bool AreLevelsSafe(int[] levels)
  {
    // track the direction the levels are trending
    var direction = 0;

    // check pairs of readings until we spot unsafe readings
    for (var  i = 1; i < levels.Length; i++)
    {
      if (!AreLevelReadingsSafe(levels[i - 1], levels[i], ref direction))
      {
        return false;
      }
    }

    return true;
  }

  private static bool AreLevelReadingsSafe(int leftReading, int rightReading, ref int direction)
  {
    // figure out the difference between the adjacent level readings, if any
    long difference = rightReading - leftReading;

    // now check if the difference is too much
    if (Math.Abs(difference) is < 1 or > 3)
    {
      return false;
    }

    // now check if the level violates the prevailing direction if one exists
    if (direction != 0 && difference * direction < 0)
    {
      return false;
    }

    // update the direction if it has not yet been set
    direction = difference > 0 ? 1 : -1;

    // the level is safe
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
