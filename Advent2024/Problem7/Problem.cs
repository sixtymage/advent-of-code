namespace Advent2024.Problem7;

public class Problem(string filename = @"data\problem7-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    Console.WriteLine($"Sum of qualifying numbers is {100}");
  }
}
