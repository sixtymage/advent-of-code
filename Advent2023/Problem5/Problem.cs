namespace Advent2023.Problem5;

public class Problem : IProblem
{
  private string _filename;

  public Problem(string filename = @"data\problem4-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);
  }
}