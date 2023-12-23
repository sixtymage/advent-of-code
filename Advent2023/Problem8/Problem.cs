namespace Advent2023.Problem8;

public class Problem : IProblem
{
  private readonly string _filename;
  private readonly bool _useJoker;

  public Problem(string filename = @"data\problem8-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);
  }
}
