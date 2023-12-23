
namespace Advent2023.Problem8;

public class Problem : IProblem
{
  private readonly string _filename;

  public Problem(string filename = @"data\problem8-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);
    var instructions = RecoverInstructions(lines);
    var map = RecoverMap(lines);
    var currentNode = map.FindMapNode("AAA");

    int numMoves = 0;
    while (true)
    {
      if (currentNode.Id == "ZZZ")
      {
        break;
      }

      var direction = instructions.GetNextMove();
      var nextId = currentNode.GetNextNode(direction);
      currentNode = map.FindMapNode(nextId);
      numMoves++;
    }

    Console.WriteLine($"Found the exit in {numMoves} moves.");
  }

  private static Instructions RecoverInstructions(string[] lines)
  {
    if (lines.Length < 1 || string.IsNullOrWhiteSpace(lines[0]))
    {
      throw new InvalidDataException("Unexepected input, first line should contain instructions");
    }

    return Instructions.FromDescription(lines[0]);
  }

  private static Map RecoverMap(string[] lines)
  {
    if (lines.Length < 3)
    {
      throw new InvalidDataException("Unexpected input, map coordinates should be from line 3 onwards but there are fewer than 3 lines");
    }

    return Map.FromDescription(lines.Skip(2));
  }
}
