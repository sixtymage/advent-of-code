namespace Advent2023.Problem1;

public class Problem : IProblem
{
  private string _filename;
  private bool _considerWords;
  private static readonly string[] Words = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

  public Problem(string filename = @"data\problem1-input.txt", bool considerWords = false)
  {
    _filename = filename;
    _considerWords = considerWords;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    int sum = 0;
    foreach (var line in lines)
    {
      var calibrationValue = _considerWords
        ? RecoverCalibrationValueWithWords(line)
        : RecoverCalibrationValueSimple(line);

      sum += calibrationValue;
    }
    Console.WriteLine($"Sum of Calibration Values: {sum}");
  }

  private static int RecoverCalibrationValueWithWords(string line)
  {
    var digits = FindDigits(line);
    return CalcCalibrationValue(digits.First(), digits.Last());
  }

  private static List<int> FindDigits(ReadOnlySpan<char> line)
  {
    var digits = new List<int>();

    for (int i=0; i<line.Length; i++)
    {
      var digit = HarvestFromDigit(line, i);
      if (digit.HasValue)
      {
        digits.Add(digit.Value);
        continue;
      }

      digit = HarvestFromWord(line, i);
      if (digit.HasValue)
      {
        digits.Add(digit.Value);
        continue;
      }
    }

    return digits;
  }

  private static int? HarvestFromDigit(ReadOnlySpan<char> line, int i)
  {
    if (int.TryParse(line.Slice(i, 1), out int digit))
    {
      return digit;
    }
    return null;
  }

  private static int? HarvestFromWord(ReadOnlySpan<char> line, int i)
  {
    for (int j=0; j<Words.Length; j++)
    {
      var word = Words[j];
      var candidate = line.Slice(i, line.Length - i < word.Length ? line.Length - i : word.Length);

      if (candidate.CompareTo(word, StringComparison.Ordinal) == 0)
      {
        return j + 1;
      }
    }
    return null;
  }

  private static int RecoverCalibrationValueSimple(string line)
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

    return CalcCalibrationValue(first, last);
  }

  private static int CalcCalibrationValue(int? first, int? last)
  {
    if (first == null || last == null)
    {
      return 0;
    }

    return first.Value * 10 + last.Value;
  }
}
