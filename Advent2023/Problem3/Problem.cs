using Advent2023;
using Advent2023.Problem3;

namespace Problem3;

public class Problem : IProblem
{
  private string _filename;

  public Problem(string filename = @"data\problem3-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    var matrix = PartMatrix.FromLines(lines);

    var locations = matrix.GetPartNumberLocations();

    int sum = 0;
    foreach (var location in locations)
    {
      if (matrix.IsPartNumber(location))
      {
        int number = matrix.GetPartNumber(location);
        sum += number;
      }
    }
    Console.WriteLine($"Sum of part numbers: {sum}");
  }
}
