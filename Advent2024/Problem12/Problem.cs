namespace Advent2024.Problem12;

public class Problem(string filename = @"data\problem12-input-example.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
  }
}
