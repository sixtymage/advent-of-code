namespace Advent2025.Problem2;

public class Problem(string filename = @"data\problem2-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var ranges = LoadRanges(lines[0]);

    var sum = ExamineRanges(ranges);

    Console.WriteLine($"Answer is: {sum}");
  }

  private long ExamineRanges(List<Range> ranges)
  {
    long sum = 0;
    foreach (var range in ranges)
    {
      sum += ExamineRange(range);
    }

    return sum;
  }

  private long ExamineRange(Range range)
  {
    long sum = 0;
    for (long i = range.Start; i <= range.End; i++)
    {
      if (IsRepeatedSequence(i))
      {
        sum += i;
      }
    }
    return sum;
  }

  private bool IsRepeatedSequence(long number)
  {
    var numberAsString = number.ToString();

    if (numberAsString.Length % 2 != 0)
    {
      return false;
    }

    var left = numberAsString[..(numberAsString.Length / 2)];
    var right = numberAsString[(numberAsString.Length / 2)..];

    return left == right;
  }

  private static List<Range> LoadRanges(string line)
  {
    var ranges = new List<Range>();

    var pairs = line.Split(',');
    foreach (var pair in pairs)
    {
      var range = GetRange(pair);
      ranges.Add(range);
    }

    return ranges;
  }

  private static Range GetRange(string pair)
  {
    var data = pair.Split('-');
    var start = Int64.Parse(data[0]);
    var end= Int64.Parse(data[1]);

    return new Range
    { 
      Start = start,
      End = end
    };
  }
}
