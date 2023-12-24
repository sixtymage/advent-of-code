
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

    var cycleLengths = new int[nodes.Count];
    for (int i = 0; i< nodes.Count; i++)
    {
      cycleLengths[i] = FindCycleLength(instructions, map, nodes[i]);
    }

    return FindLeastCommonDivisor(cycleLengths);
  }

  private static int FindCycleLength(Instructions instructions, Map map, MapNode node)
  {
    var pathLengths = new List<int>();

    int numSteps = 0;
    while (true)
    {
      if (node.IsEndNode())
      {
        pathLengths.Add(numSteps);

        if (ContainsRepeatSequence(pathLengths))
        {
          var sequenceRange = pathLengths[0..(pathLengths.Count/2)];
          return sequenceRange.Sum();
        }

        numSteps = 0;
      }

      var direction = instructions.GetNextMove();
      var nextId = node.GetNextNode(direction);
      node = map.FindMapNode(nextId);
      numSteps++;
    }
  }

  private static bool ContainsRepeatSequence(List<int> list)
  {
    if (list.Count % 2 == 1)
    {
      return false;
    }

    var left = list[0..(list.Count/2)];
    var right = list[(list.Count/2)..];

    for(var i=0; i<left.Count; i++)
    {
      if (left[i] != right[i])
      {
        return false;
      }
    }
    return true;
  }

  private static long FindLeastCommonDivisor(ReadOnlySpan<int> values)
  {
    ArgumentOutOfRangeException.ThrowIfLessThan(values.Length, 2);

    return values.Length == 2
      ? FindLeastCommonDivisor(values[0], values[1])
      : FindLeastCommonDivisor(values[0], FindLeastCommonDivisor(values[1..]));
  }

  private static long FindLeastCommonDivisor(long n, long m)
  {
    var gcd = FindGreatestCommonDivisor(n, m);
    return n / gcd * m;
  }

  private static long FindGreatestCommonDivisor(long n, long m)
  {
    ArgumentOutOfRangeException.ThrowIfZero(n);
    ArgumentOutOfRangeException.ThrowIfZero(m);

    var smaller = n < m ? n : m;
    var larger = n > m ? n : m;
    
    if (larger % smaller == 0)
    {
      return larger;
    }

    // check for factors up to the square root of the smaller number
    var searchMax = (long)Math.Sqrt(smaller);

    for (var p = searchMax; p > 1; p--)
    {
      if (smaller % p == 0)
      {
        // if this factor also divides the large number, we're done
        if (larger % p == 0)
        {
          return p;
        }

        // check if the pair of this factor divides the larger number
        var q = smaller / p;
        if (larger % q == 0)
        {
          return q;
        }
      }
    }

    return 1;
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
