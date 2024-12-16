namespace Advent2024.Problem8;

public class Problem(string filename = @"data\problem8-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var matrix = ExtractInput(lines);

    Solve(matrix, false);
    Solve(matrix, true);
  }

  private static void Solve(Matrix<char> matrix, bool includeHarmonics)
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
      FindAntiNodes(map, antennae, frequency, antiNodes, includeHarmonics);
    }

    Console.WriteLine($"The number of anti-nodes is {antiNodes.Count} (harmonics={includeHarmonics})");
  }

  private static void FindAntiNodes(Map map,
    Antenna[] antennae,
    char frequency,
    List<Location> antiNodes,
    bool includeHarmonics)
  {
    // get the antennae for this frequency
    var someAntennae = antennae.Where(a => a.Frequency == frequency).ToArray();

    // get the pairs where order doesn't matter (so a1 with a2 also covers a2 with a1)
    var pairs = GetPairs(someAntennae);

    // iterate the pairs
    foreach (var pair in pairs)
    {
      // calculate the anti-node positions
      var candidateLocations = includeHarmonics
        ? FindAntiNodeLocationsWithHarmonics(map, pair.Item1, pair.Item2)
        : FindPrimaryAntiNodeLocationsOnly(pair.Item1, pair.Item2);

      // capture on-map locations we haven't yet found
      TryAdd(candidateLocations, map, antiNodes);
    }
  }

  private static Location[] FindAntiNodeLocationsWithHarmonics(Map map, Location location1, Location location2)
  {
    var rowDisplacement = location2.Row - location1.Row;
    var colDisplacement = location2.Col - location1.Col;

    var harmonics1 = FindHarmonics(map, location1, -1 * rowDisplacement, -1 * colDisplacement);
    var harmonics2 = FindHarmonics(map, location2, +1 * rowDisplacement, +1 * colDisplacement);

    return [..harmonics1, ..harmonics2];
  }

  private static Location[] FindHarmonics(Map map, Location startLocation, int rowDisplacement, int colDisplacement)
  {
    var locations = new List<Location>();
    var index = 0;
    while (true)
    {
      var location = new Location
      {
        Row = startLocation.Row + rowDisplacement * index,
        Col = startLocation.Col + colDisplacement * index
      };

      if (!map.IsOnMap(location))
      {
        break;
      }

      locations.Add(location);
      index++;
    }

    return locations.ToArray();
  }

  private static Location[] FindPrimaryAntiNodeLocationsOnly(Location location1, Location location2)
  {
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

    return [antiNode1, antiNode2];
  }

  private static void TryAdd(Location[] candidateLocations, Map map, List<Location> antiNodes)
  {
    foreach (var candidateLocation in candidateLocations)
    {
      if (map.IsOnMap(candidateLocation) && !antiNodes.Contains(candidateLocation))
      {
        antiNodes.Add(candidateLocation);
      }
    }
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