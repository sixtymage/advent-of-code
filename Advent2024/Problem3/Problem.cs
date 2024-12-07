namespace Advent2024.Problem3;

public class Problem(string filename = @"data\problem3-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var input = await File.ReadAllTextAsync(filename);

    var extractor = new StateMachineInstructionExtractor();
    var instructions = extractor.ExtractInstructions(input);
    var total = CalculateTotal(instructions);

    Console.WriteLine($"Sum of mul instructions is: {total}");
  }

  private static long CalculateTotal(List<Instruction> parsedInstructions)
  {
    return parsedInstructions
      .Aggregate<Instruction?, long>(0, (total, pi) => pi == null ? total : total + pi.Left * pi.Right);
  }
}
