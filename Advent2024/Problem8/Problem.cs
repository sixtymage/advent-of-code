namespace Advent2024.Problem8;

public class Problem(string filename = @"data\problem8-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var matrix = ExtractInput(lines);

    Solve(matrix);
  }

  private static void Solve(Matrix<char> matrix)
  {
    // create the map
    var map = new Map(matrix);

    // get all antennae
    var antennae = map.GetAntennae();

    // get all distinct frequencies
    var frequencies = antennae
      .Select(a => a.Frequency)
      .Distinct();

    // iterate each frequency
    var antiNodes = new List<Location>();
    foreach (var frequency in frequencies)
    {
      FindAntiNodes(map, antennae, frequency, antiNodes);
    }

    Console.WriteLine($"The number of anti-nodes is {antiNodes.Count}");
  }

  private static void FindAntiNodes(
    Map map,
    Antenna[] antennae,
    char frequency,
    List<Location> antiNodes)
  {
    // get the antennae for this frequency
    var someAntennae = antennae.Where(a => a.Frequency == frequency).ToArray();

    // get the pairs where order doesn't matter (so a1 with a2 also covers a2 with a1)
    var pairs = GetPairs(someAntennae);

    // iterate the pairs
    foreach (var pair in pairs)
    {
      // calculating the anti-node positions
      var (antiNode1, antiNode2) = FindAntiNodeLocation(pair.Item1, pair.Item2);

      // if an anti-node location is on the map and not one already found, add it to the list
      TryAdd(antiNode1, map, antiNodes);
      TryAdd(antiNode2, map, antiNodes);
    }
  }

  private static void TryAdd(Location candidateLocation, Map map, List<Location> antiNodes)
  {
    if (map.IsOnMap(candidateLocation) && !antiNodes.Contains(candidateLocation))
    {
      antiNodes.Add(candidateLocation);
    }
  }

  private static (Location, Location) FindAntiNodeLocation(Location location1, Location location2)
  {
    // find the row and column displacement of the two locations
    var rowDisplacement = location2.Row - location1.Row;
    var colDisplacement = location2.Col - location1.Col;

    var antiNode1 = new Location
    {
      Row = location1.Row - rowDisplacement,
      Col = location1.Col - colDisplacement
    };
    var antiNode2 = new Location
    {
      Row = location2.Row + rowDisplacement,
      Col = location2.Col + colDisplacement
    };

    return (antiNode1, antiNode2);
  }

  private static (Location, Location)[] GetPairs(Antenna[] antennae)
  {
    switch (antennae.Length)
    {
      case < 2:
        throw new InvalidOperationException($"Cannot find pairs for an array of length {antennae.Length}");
      case 2:
        return [(antennae[0].Location, antennae[1].Location)];
    }

    var pairs = new List<(Location, Location)>();
    for (var i = 1; i < antennae.Length; i++)
    {
      pairs.Add((antennae[0].Location, antennae[i].Location));
    }

    var subPairs = GetPairs(antennae.Skip(1).ToArray());

    pairs.AddRange(subPairs);
    return pairs.ToArray();
  }

  private static Matrix<char> ExtractInput(string[] lines)
  {
    var source = new LinesSource(lines);
    return new Matrix<char>(source);
  }
}