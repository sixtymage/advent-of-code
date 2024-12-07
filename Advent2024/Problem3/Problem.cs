namespace Advent2024.Problem3;

public class Problem(string filename = @"data\problem3-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var input = await File.ReadAllTextAsync(filename);

    var validInstructions = ExtractValidInstructions(input);
    var parsedInstructions = ParseInstructions(validInstructions);

    var total = CalculateTotal(parsedInstructions);

    Console.WriteLine($"Sum of mul instructions is: {total}");
  }

  private static List<string> ExtractValidInstructions(string input)
  {
    throw new NotImplementedException();
  }

  private static List<Instruction> ParseInstructions(List<string> validInstructions)
  {
    throw new NotImplementedException();
  }

  private static long CalculateTotal(List<Instruction> parsedInstructions)
  {
    throw new NotImplementedException();
  }
}
