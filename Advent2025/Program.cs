namespace Advent2025;

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
      1 => new Advent2025.Problem1.Problem(),
      _ => new NullProblem()
    };
  }

  private class NullProblem : IProblem
  {
    public Task SolveAsync()
    {
      Console.WriteLine("Usage: Advent2025.exe [problemId]");
      return Task.CompletedTask;
    }
  }
}
