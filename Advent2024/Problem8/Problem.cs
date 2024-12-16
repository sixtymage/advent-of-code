using System.Diagnostics;
using System.Xml;

namespace Advent2024.Problem8;

public class Problem(string filename = @"data\problem8-input-example.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var matrix = ExtractInput(lines);

    Solve(matrix);
  }

  private static void Solve(Matrix<char> matrix)
  {
  }

  private static Matrix<char> ExtractInput(string[] lines)
  {
    var source = new LinesSource(lines);
    return new Matrix<char>(source);
  }
}