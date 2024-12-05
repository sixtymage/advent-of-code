using System.Collections.Immutable;

namespace Advent2024.Problem1;

public class Problem(string filename = @"data\problem1-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var numbers = lines
      .Select(x =>
      {
        var s = x.Split("   ");
        return (int.Parse(s[0]), int.Parse(s[1]));
      })
      .ToArray();

    var first = numbers.Select(x => x.Item1).ToArray();
    var second = numbers.Select(x => x.Item2).ToArray();

    if (first.Length != second.Length)
    {
      throw new InvalidDataException("Input arrays do not have the same length");
    }
   
    Array.Sort(first);
    Array.Sort(second);

    var total = 0L;
    for (var i=0; i<first.Length; i++)
    {
      var delta = Math.Abs(first[i] - second[i]);
      total += delta;
    }

    Console.WriteLine($"Total distance is: {total}");
  }
}
