

namespace Advent2023.Problem8;

public class Problem : IProblem
{
  private readonly string _filename;
  private readonly bool _solveManyPaths;

  public Problem(string filename = @"data\problem8-input.txt", bool solveManyPaths = false)
  {
    _filename = filename;
    _solveManyPaths = solveManyPaths;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);
    var instructions = RecoverInstructions(lines);
    var map = RecoverMap(lines);

    long numMoves = _solveManyPaths
      ? SolveManyPaths(instructions, map)
      : SolveSinglePath(instructions, map);

    Console.WriteLine($"Found the exit in {numMoves} moves.");
  }

  private static long SolveManyPaths(Instructions instructions, Map map)
  {
    var nodes = map.FindStartNodes();
    long numMoves = 0;

    while (true)
    {
      if (map.AreAllEndNodes(nodes))
      {
        break;
      }

      var direction = instructions.GetNextMove();
      nodes = MoveNextNodes(map, nodes, direction);
      numMoves++;

      if (numMoves % 10000000 == 0)
      {
        Console.WriteLine($"Made {numMoves:#,0} moves, still looking for the exit...");
      }
    }

    return numMoves;
  }

  private static List<MapNode> MoveNextNodes(Map map, List<MapNode> nodes, Direction direction)
  {
    var nextNodes = new List<MapNode>();
    foreach (var node in nodes)
    {
      var nextId = node.GetNextNode(direction);
      var nextNode = map.FindMapNode(nextId);
      nextNodes.Add(nextNode);
    }
    return nextNodes;
  }

  private static long SolveSinglePath(Instructions instructions, Map map)
  {
    var currentNode = map.FindMapNode("AAA");

    long numMoves = 0;
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

    return numMoves;
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
