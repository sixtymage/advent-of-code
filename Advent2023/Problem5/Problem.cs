
namespace Advent2023.Problem5;

public class Problem : IProblem
{
  private string _filename;

  public Problem(string filename = @"data\problem5-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    var input = ReadInput(lines);
    var locations = LocateSeeds(input);

    var lowest = locations.Min();
    Console.WriteLine($"Lowest seed location is {lowest}");
  }

  private List<long> LocateSeeds(Input input)
  {
    var locations = new List<long>();
    foreach (var seed in input.Seeds)
    {
      var destination = seed;
      foreach (var map in input.Maps)
      {
        destination = map.Lookup(destination);
      }
      locations.Add(destination);
    }
    return locations;
  }

  private static Input ReadInput(string[] lines)
  {
    var seeds = RecoverSeeds(lines);
    var maps = RecoverMaps(lines);
    return new Input(seeds, maps);
  }

  private static List<long> RecoverSeeds(string[] lines)
  {
    if (lines.Length == 0)
    {
      throw new InvalidDataException("Unexpected format, no lines listed.");
    }

    if (!lines[0].StartsWith("seeds:"))
    {
      throw new InvalidDataException("Unexpected format, seeds not listed on first line.");
    }

    var splitColon = lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitColon.Length != 2)
    {
      throw new InvalidDataException("Unexpected format, no seed numbers found.");
    }

    var splitSpace = splitColon[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    return splitSpace
      .Select(long.Parse)
      .ToList();
  }

  private static List<RangedMap> RecoverMaps(string[] lines)
  {
    int i = 0;
    var maps = new List<RangedMap>();
    while (true)
    {
      if (i >= lines.Length)
      {
        break;
      }

      if (lines[i].Contains("map:"))
      {
        var map = RecoverMap(lines, ref i);
        maps.Add(map);
      }

      i++;
    }

    return maps;
  }

  private static RangedMap RecoverMap(string[] lines, ref int i)
  {
    var splitSpace = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitSpace.Length != 2)
    {
      throw new InvalidDataException("Unexpected format, seed map description not found.");
    }

    var map = new RangedMap(splitSpace[0]);

    while (true)
    {
      i++;

      if (i >= lines.Length)
      {
        return map;
      }

      if (string.IsNullOrWhiteSpace(lines[i]))
      {
        return map;
      }

      (var destination, var source, var length) = RecoverRange(lines[i]);
      map.AddRange(destination, source, length);
    }
  }

  private static (long, long, long) RecoverRange(string rangeDescription)
  {
    var splitSpace = rangeDescription.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitSpace.Length != 3)
    {
      throw new InvalidDataException("Unexpected format, map range not loaded.");
    }

    var destination = long.Parse(splitSpace[0]);
    var source = long.Parse(splitSpace[1]);
    var length = long.Parse(splitSpace[2]);

    return (destination, source, length);
  }
}
