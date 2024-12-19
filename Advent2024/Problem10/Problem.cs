namespace Advent2024.Problem10;

public class Problem(string filename = @"data\problem10-input.txt") : IProblem
{
  private const int LowestHeight = 0;
  private const int HighestHeight = 9;
  private const int TrailLength = HighestHeight - LowestHeight;

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var matrix = ExtractInput(lines);

    SolvePart1(matrix);
    SolvePart2(matrix);
  }

  private static void SolvePart1(Matrix<int> matrix)
  {
    // make a map
    var map = new Map(matrix);

    // find all the potential trailheads (cells with a 0)
    var candidateStartLocations = map.GetLocations(LowestHeight);

    // find all the potential ends of a trail (cells with a 9)
    var candidateEndLocations = map.GetLocations(HighestHeight);

    // for each potential trailhead, check each end of the trail to see if a path exists
    var scores = new Dictionary<Location, int>();
    foreach (var candidateStartLocation in candidateStartLocations)
    {
      foreach (var candidateEndLocation in candidateEndLocations)
      {
        // if a path exists, add 1 to the count for that trailhead
        if (!map.DoesTrailExist(candidateStartLocation, candidateEndLocation, TrailLength))
        {
          continue;
        }

        scores.TryAdd(candidateStartLocation, 0);
        scores[candidateStartLocation]++;
      }
    }

    // sum the counts
    var sum = scores.Values.Sum();
    Console.WriteLine($"Part 1: The total score of all trailheads is {sum}");
  }

  private static void SolvePart2(Matrix<int> matrix)
  {
    // make a map
    var map = new Map(matrix);

    // find all the potential trailheads (cells with a 0)
    var candidateStartLocations = map.GetLocations(LowestHeight);

    // find all the potential ends of a trail (cells with a 9)
    var candidateEndLocations = map.GetLocations(HighestHeight);

    // for each potential trailhead, check each end of the trail to see if a path exists
    var ratings = new Dictionary<Location, int>();
    foreach (var candidateStartLocation in candidateStartLocations)
    {
      foreach (var candidateEndLocation in candidateEndLocations)
      {
        // get the number of distinct trails
        var trails = new List<Trail>();
        var trail = new Trail(candidateStartLocation);
        map.FindTrails(candidateStartLocation, candidateEndLocation, TrailLength, trail, trails);

        ratings.TryAdd(candidateStartLocation, 0);
        ratings[candidateStartLocation] += trails.Count;
      }
    }

    // sum the counts
    var sum = ratings.Values.Sum();
    Console.WriteLine($"Part 2: The total rating of all trailheads is {sum}");
  }

  private static Matrix<int> ExtractInput(string[] lines)
  {
    var source1 = new LinesSource(lines);
    var source2 = new DigitSource(source1);
    return new Matrix<int>(source2);
  }
}
