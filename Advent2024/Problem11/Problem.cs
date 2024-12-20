using System.Diagnostics;

namespace Advent2024.Problem11;

public class Problem(string filename = @"data\problem11-input.txt") : IProblem
{
  private static readonly Dictionary<BlinkStone, long> PrecomputedSums = new Dictionary<BlinkStone, long>();

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var stones = lines[0].Split(" ").Select(long.Parse).ToList();

    SolvePart1(stones);
    SolvePart2(stones);
  }

  private static void SolvePart1(List<long> stones)
  {
    const int maxNumBlinks = 25;
    for (var i = 0; i < maxNumBlinks; i++)
    {
      stones = Blink(stones);
    }

    Console.WriteLine($"Part 1: After {maxNumBlinks} blinks there are {stones.Count} stones");
  }

  private static void SolvePart2(List<long> initialStones)
  {
    const int minNumBlinks = 75;
    const int maxNumBlinks = 75;
    for (var i = minNumBlinks; i <= maxNumBlinks; i++)
    {
      var sw = Stopwatch.StartNew();
      var sum = initialStones.Sum(s => FindCount(s, i));
      Console.WriteLine($"Part 2: After {i} blinks there are {sum} stones ({sw.Elapsed})");
    }
  }

  private static void WriteStones(List<long> stones)
  {
    Console.WriteLine(string.Join(' ', stones));
  }

  private static long FindCount(long stone, int numBlinks)
  {
    return stone switch
    {
      0 => FindCount0(numBlinks),
      1 => FindCount1(numBlinks),
      2 => FindCount2(numBlinks),
      3 => FindCount3(numBlinks),
      4 => FindCount4(numBlinks),
      5 => FindCount5(numBlinks),
      6 => FindCount6(numBlinks),
      7 => FindCount7(numBlinks),
      8 => FindCount8(numBlinks),
      9 => FindCount9(numBlinks),
      _ => FindCount10OrGreater(stone, numBlinks)
    };
  }

  private static long FindCount0(int numBlinks)
  {
    // 0
    // 1
    // 2024
    // 20 24
    // 2 0 2 4
    if (numBlinks <= 4)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 1,
        3 => 2,
        4 => 4,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 0)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(2, numBlinks - 4);
    sum += FindCachedCount(0, numBlinks - 4);
    sum += FindCachedCount(2, numBlinks - 4);
    sum += FindCachedCount(4, numBlinks - 4);

    return sum;
  }

  private static long FindCount1(int numBlinks)
  {
    // 1
    // 2024
    // 20 24
    // 2 0 2 4
    if (numBlinks <= 3)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 2,
        3 => 4,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 1)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(2, numBlinks - 3);
    sum += FindCachedCount(0, numBlinks - 3);
    sum += FindCachedCount(2, numBlinks - 3);
    sum += FindCachedCount(4, numBlinks - 3);

    return sum;
  }

  private static long FindCount2(int numBlinks)
  {
    // 2
    // 4048
    // 40 48
    // 4 0 4 8
    if (numBlinks <= 3)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 2,
        3 => 4,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 2)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(4, numBlinks - 3);
    sum += FindCachedCount(0, numBlinks - 3);
    sum += FindCachedCount(4, numBlinks - 3);
    sum += FindCachedCount(8, numBlinks - 3);

    return sum;
  }

  private static long FindCount3(int numBlinks)
  {
    // 3
    // 6072
    // 60 72
    // 6 0 7 2
    if (numBlinks <= 3)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 2,
        3 => 4,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 3)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(6, numBlinks - 3);
    sum += FindCachedCount(0, numBlinks - 3);
    sum += FindCachedCount(7, numBlinks - 3);
    sum += FindCachedCount(2, numBlinks - 3);

    return sum;
  }

  private static long FindCount4(int numBlinks)
  {
    // 4
    // 8096
    // 80 96
    // 8 0 9 6
    if (numBlinks <= 3)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 2,
        3 => 4,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 4)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(8, numBlinks - 3);
    sum += FindCachedCount(0, numBlinks - 3);
    sum += FindCachedCount(9, numBlinks - 3);
    sum += FindCachedCount(6, numBlinks - 3);

    return sum;
  }

  private static long FindCount5(int numBlinks)
  {
    // 5
    // 10120
    // 20482880
    // 2048 2880
    // 20 48 28 80
    // 2 0 4 8 2 8 8 0
    if (numBlinks <= 5)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 1,
        3 => 2,
        4 => 4,
        5 => 8,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 5)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(2, numBlinks - 5);
    sum += FindCachedCount(0, numBlinks - 5);
    sum += FindCachedCount(4, numBlinks - 5);
    sum += FindCachedCount(8, numBlinks - 5);
    sum += FindCachedCount(2, numBlinks - 5);
    sum += FindCachedCount(8, numBlinks - 5);
    sum += FindCachedCount(8, numBlinks - 5);
    sum += FindCachedCount(0, numBlinks - 5);

    return sum;
  }

  private static long FindCount6(int numBlinks)
  {
    // 6
    // 12144
    // 24579456
    // 2457 9456
    // 24 57 94 56
    // 2 4 5 7 9 4 5 6
    if (numBlinks <= 5)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 1,
        3 => 2,
        4 => 4,
        5 => 8,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 5)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(2, numBlinks - 5);
    sum += FindCachedCount(4, numBlinks - 5);
    sum += FindCachedCount(5, numBlinks - 5);
    sum += FindCachedCount(7, numBlinks - 5);
    sum += FindCachedCount(9, numBlinks - 5);
    sum += FindCachedCount(4, numBlinks - 5);
    sum += FindCachedCount(5, numBlinks - 5);
    sum += FindCachedCount(6, numBlinks - 5);

    return sum;
  }

  private static long FindCount7(int numBlinks)
  {
    // 7
    // 14168
    // 28676032
    // 2867 6032
    // 28 67 60 32
    // 2 8 6 7 6 0 3 2
    if (numBlinks <= 5)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 1,
        3 => 2,
        4 => 4,
        5 => 8,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 5)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(2, numBlinks - 5);
    sum += FindCachedCount(8, numBlinks - 5);
    sum += FindCachedCount(6, numBlinks - 5);
    sum += FindCachedCount(7, numBlinks - 5);
    sum += FindCachedCount(6, numBlinks - 5);
    sum += FindCachedCount(0, numBlinks - 5);
    sum += FindCachedCount(3, numBlinks - 5);
    sum += FindCachedCount(2, numBlinks - 5);

    return sum;
  }

  private static long FindCount8(int numBlinks)
  {
    // 8
    // 16192
    // 32772608
    // 3277 2608
    // 32 77 26 8
    if (numBlinks <= 4)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 1,
        3 => 2,
        4 => 4,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 8)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(32, numBlinks - 4);
    sum += FindCachedCount(77, numBlinks - 4);
    sum += FindCachedCount(26, numBlinks - 4);
    sum += FindCachedCount(8, numBlinks - 4);

    return sum;
  }

  private static long FindCount9(int numBlinks)
  {
    // 9
    // 18216
    // 36869184
    // 3686 9184
    // 36 86 91 84
    // 3 6 8 6 9 1 8 4
    if (numBlinks <= 5)
    {
      return numBlinks switch
      {
        1 => 1,
        2 => 1,
        3 => 2,
        4 => 4,
        5 => 8,
        _ => throw new InvalidOperationException("Cannot process 0 blinks (stone 9)")
      };
    }

    long sum = 0;
    sum += FindCachedCount(3, numBlinks - 5);
    sum += FindCachedCount(6, numBlinks - 5);
    sum += FindCachedCount(8, numBlinks - 5);
    sum += FindCachedCount(6, numBlinks - 5);
    sum += FindCachedCount(9, numBlinks - 5);
    sum += FindCachedCount(1, numBlinks - 5);
    sum += FindCachedCount(8, numBlinks - 5);
    sum += FindCachedCount(4, numBlinks - 5);

    return sum;
  }

  private static long FindCount10OrGreater(long stone, int numBlinks)
  {
    var stones = ApplyRules(stone);
    return numBlinks == 1
      ? stones.Count()
      : stones.Sum(s => FindCachedCount(s, numBlinks - 1));
  }

  private static long FindCachedCount(long stone, int numBlinks)
  {
    var key = new BlinkStone(stone, numBlinks);
    if (PrecomputedSums.TryGetValue(key, out var sum))
    {
      return sum;
    }

    sum = FindCount(stone, numBlinks);
    PrecomputedSums.Add(key, sum);
    return sum;
  }

  private static List<long> Blink(List<long> stones)
  {
    var newStones = new List<long>();
    foreach (var stone in stones)
    {
      newStones.AddRange(ApplyRules(stone));
    }

    return newStones;
  }

  private static IEnumerable<long> ApplyRules(long stone)
  {
    if (stone == 0)
    {
      return [1];
    }

    var strStone = $"{stone}";
    if (strStone.Length % 2 != 0)
    {
      return [stone * 2024];
    }

    var left = strStone[..(strStone.Length / 2)];
    var right = strStone[(strStone.Length / 2)..];
    return [long.Parse(left), long.Parse(right)];
  }
}
