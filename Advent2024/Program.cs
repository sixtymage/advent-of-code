namespace Advent2024;

internal static class Program
{
  public static async Task Main(string[] args)
  {
    try
    {
      var problemId = GetProblemId(args);
      var problem = CreateProblem(problemId);
      await problem.SolveAsync();
    }
    catch (Exception e)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"Error: {e}");
    }
  }

  private static int GetProblemId(string[] args)
  {
    return args.Length switch
    {
      > 0 when int.TryParse(args[0], out var problemId) => problemId,
      _ => 0
    };
  }

  private static IProblem CreateProblem(object problemId)
  {
    return problemId switch
    {
      1 => new Problem1.Problem(),
      2 => new Problem2.Problem(),
      3 => new Problem3.Problem(),
      4 => new Problem4.Problem(),
      5 => new Problem5.Problem(),
      6 => new Problem6.Problem(),
      7 => new Problem7.Problem(),
      8 => new Problem8.Problem(),
      _ => new NullProblem()
    };
  }

  private class NullProblem : IProblem
  {
    public Task SolveAsync()
    {
      Console.WriteLine("Usage: Advent2024.exe [problemId]");
      return Task.CompletedTask;
    }
  }
}