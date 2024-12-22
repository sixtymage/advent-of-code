namespace Advent2024.Problem12;

public class Problem(string filename = @"data\problem12-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    SolvePart1(lines);
  }

  private static void SolvePart1(string[] lines)
  {
    var matrix = ExtractInput(lines);
    var map = new Map(matrix);
    var zones = map.GetZones();

    var cost = zones.Sum(CalcCost);
    Console.WriteLine($"Cost to fence all zones is: {cost}");
  }

  private static int CalcCost(Zone zone)
  {
    var perimeter = FindPerimeter(zone);
    return perimeter * zone.Area;
  }

  private static int FindPerimeter(Zone zone)
  {
    var perimeter = 0;
    foreach (var location in zone.Locations)
    {
      var candidateLocations = zone.Locations
        .Where(l => l != location);

      var numSharedEdges = CalcNumSharedEdges(location, candidateLocations);
      perimeter += (4 - numSharedEdges);
    }

    return perimeter;
  }

  private static int CalcNumSharedEdges(Location location, IEnumerable<Location> otherLocations)
  {
    var numSharedEdges = 0;
    foreach (var otherLocation in otherLocations)
    {
      if (location.DoesShareEdge(otherLocation))
      {
        numSharedEdges++;
      }
    }

    return numSharedEdges;
  }

  private static Matrix<char> ExtractInput(string[] lines)
  {
    var source = new LinesSource(lines);
    return new Matrix<char>(source);
  }
}
