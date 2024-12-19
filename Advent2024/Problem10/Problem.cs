namespace Advent2024.Problem10;

public class Problem(string filename = @"data\problem8-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var matrix = ExtractInput(lines);
  }

  private static Matrix<char> ExtractInput(string[] lines)
  {
    var source = new LinesSource(lines);
    return new Matrix<char>(source);
  }
}