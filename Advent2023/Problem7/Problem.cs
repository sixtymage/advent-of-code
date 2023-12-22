namespace Advent2023.Problem7;

public class Problem : IProblem
{
  private readonly string _filename;
  
  public Problem(string filename = @"data\problem6-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);
  }
}
