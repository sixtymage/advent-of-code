using Advent2023;
using System.Globalization;

namespace Problem1;

public class Problem : IProblem
{
  private string _filename;

  public Problem(string filename = @"data\problem1-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    int sum = 0;
    foreach (var line in lines) 
    {
      var calibrationValue = RecoverCalibrationValue(line);
      sum += calibrationValue;
    }
    Console.WriteLine($"Sum of Calibration Values: {sum}");
  }

  private int RecoverCalibrationValue(string line)
  {
    if (line == string.Empty)
    {
      return 0;
    }

    int? first = null;
    int? last = null;

    var span = (ReadOnlySpan<char>)line;
    for (int i=0; i<span.Length; i++)
    {
      var slice = span.Slice(i, 1);
      if (int.TryParse(slice, out int digit))
      {
        first ??= digit;
        last = digit;
      }
    }

    if (first == null || last == null)
    {
      return 0;
    }

    return first.Value * 10 + last.Value;
  }
}
