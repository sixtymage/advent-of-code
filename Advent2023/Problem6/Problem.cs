namespace Advent2023.Problem6;

public class Problem : IProblem
{
  private readonly string _filename;
  private readonly bool _handleKerning;

  public Problem(string filename = @"data\problem6-input.txt", bool handleKerning = false)
  {
    _filename = filename;
    _handleKerning = handleKerning;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);
    if (lines.Length != 2)
    {
      throw new InvalidDataException("Unexpected input, should be 2 lines exactly");
    }

    var times = LoadValues(lines[0]);
    var distances = LoadValues(lines[1]);

    if (_handleKerning)
    {
      times = new List<long> { KernValues(times) };
      distances = new List<long> { KernValues(distances) };
    }

    if (times.Count != distances.Count)
    {
      throw new InvalidDataException($"Unexpected input, should be the same number of times ({times.Count}) as distances ({distances.Count}).");
    }

    long sum = 1;
    for (var i = 0; i < times.Count; i++)
    {
      var numWays = CalculateNumWays(times[i], distances[i]);
      sum *= numWays;
    }
    Console.WriteLine($"Total number of ways is {sum}");
  }

  private static long KernValues(List<long> values)
  {
    return long.Parse(string.Join("", values));
  }

  private static List<long> LoadValues(string line)
  {
    var splitColon = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitColon.Length != 2)
    {
      throw new InvalidDataException("Unexpected values, should be a single colon after the prefix");
    }

    var splitSpace = splitColon[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    return splitSpace
      .Select(long.Parse)
      .ToList();
  }

  private static long CalculateNumWays(long time, long distance)
  {
    var numWays = 0;
    for (var holdTime = 0; holdTime <= time; holdTime++)
    {
      var remainingTime = time - holdTime;
      var speed = holdTime;

      var travelledDistance = speed * remainingTime;

      if (travelledDistance > distance)
      {
        numWays++;
      }
    }
    return numWays;
  }
}
