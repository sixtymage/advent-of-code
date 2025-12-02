namespace Advent2025.Problem2;

public class Problem(string filename = @"data\problem2-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var ranges = LoadRanges(lines[0]);

    var sum = ExamineRanges(ranges, true);

    Console.WriteLine($"Answer is: {sum}");
  }

  private static long ExamineRanges(List<Range> ranges, bool allowMultiple)
  {
    long sum = 0;
    foreach (var range in ranges)
    {
      sum += ExamineRange(range, allowMultiple);
    }

    return sum;
  }

  private static long ExamineRange(Range range, bool allowMultiple)
  {
    long sum = 0;
    for (long i = range.Start; i <= range.End; i++)
    {
      if (IsRepeatedSequence(i, allowMultiple))
      {
        sum += i;
      }
    }
    return sum;
  }

  private static bool IsRepeatedSequence(long number, bool allowMultiple)
  {
    if (allowMultiple)
    {
      return IsRepeatedSequenceAllowMultiple(number);
    }
    return IsDoubleSequence(number);
  }

  private static bool IsRepeatedSequenceAllowMultiple(long number)
  {
    var numberAsString = number.ToString();

    // if only one digit, there can't be any repeats
    if (numberAsString.Length <= 1)
    {
      return false;
    }

    // otherwise examine all sequence lengths that cleanly divide the length where all sequences are the same
    for (var sequenceLength = 1; sequenceLength <= numberAsString.Length / 2; sequenceLength++)
    {
      if (numberAsString.Length % sequenceLength == 0)
      {
        var numSegments = numberAsString.Length / sequenceLength;
        var segments = new List<string>();
        for (int i=0; i<numberAsString.Length; i += sequenceLength)
        {
          var segment = numberAsString.Substring(i, sequenceLength);
          segments.Add(segment);
        }

        if (segments.All(s => s == segments[0]))
        {
          return true;
        }
      }
    }
    return false;
  }

  private static bool IsDoubleSequence(long number)
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
