using Advent2023;
using Advent2023.Problem3;

namespace Problem3;

public class Problem : IProblem
{
  private string _filename;

  public Problem(string filename = @"data\problem3-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    var matrix = PartMatrix.FromLines(lines);

    var locations = matrix.GetPartNumberLocations();

    int sum = 0;
    var gearTracking = new Dictionary<long, List<int>>();
    foreach (var location in locations)
    {
      var symbols = matrix.GetSurroundingSymbols(location);

      if (PartMatrix.IsPartNumber(symbols))
      {
        int number = matrix.GetPartNumber(location);
        sum += number;

        var gearLocations = symbols.Where(x => x.Symbol == '*');
        UpdateGearTracking(gearTracking, number, gearLocations);
      }
    }
    Console.WriteLine($"Sum of part numbers: {sum}");

    int sumGearRatios = CalcSumOfGearRatios(gearTracking);
    Console.WriteLine($"Sum of gear ratios: {sumGearRatios}");
  }

  private static int CalcSumOfGearRatios(Dictionary<long, List<int>> gearTracking)
  {
    int sum = 0;
    foreach (var gear in gearTracking.Values)
    {
      if (gear.Count == 2)
      {
        var ratio = gear[0] * gear[1];
        sum += ratio;
      }
    }
    return sum;
  }

  private static void UpdateGearTracking(
    Dictionary<long, List<int>> gearTracking,
    int number,
    IEnumerable<SymbolLocation> gearLocations)
  {
    foreach (var location in gearLocations)
    {
      var key = ((long)location.Row << 32) | (long)location.Col;

      if (!gearTracking.ContainsKey(key))
      {
        gearTracking[key] = [];
      }
      gearTracking[key].Add(number);
    }
  }
}
