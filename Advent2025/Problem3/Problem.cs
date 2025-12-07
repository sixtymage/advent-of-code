


namespace Advent2025.Problem3;

public class Problem(string filename = @"data\problem3-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var joltage = CalculateJoltage(lines, 2);
    Console.WriteLine($"Answer is: {joltage}");
  }

  private static long CalculateJoltage(string[] lines, int numSelections)
  {
    long joltage = 0;
    foreach (var line in lines )
    {
      joltage += CalculateJoltage(line, numSelections);
    }

    return joltage;
  }

  private static long CalculateJoltage(string line, int numSelections)
  {
    // 818181911112111

    long joltage = 0;

    // we start from the left
    int startIndex = 0;

    for (int i = numSelections-1; i >=0 ; i--)
    {
      // find the last usable index: 15 - 1 - 1 = 13
      var lastUsableIndex = line.Length - 1 - i;

      // select a range to search from startIndex to lastUsableIndex
      var usableRange = line.Skip(startIndex).Take(lastUsableIndex - startIndex + 1);

      // find the largest value in this range
      (int index, long digit) = FindLargest([.. usableRange]);

      // update joltage
      joltage = joltage * 10 + digit;

      // update start index (must be to the right of chosen index)
      startIndex = index + 1;
    }

    return joltage;
  }

  private static (int index, long digit) FindLargest(char[] chars)
  {
    long largestDigit = 0;
    int largestIndex = -1;
    for (int i = 0; i < chars.Length; i++)
    {
      long digit = Int64.Parse($"{chars[i]}");

      if (digit > largestDigit)
      {
        largestDigit = digit;
        largestIndex = i;
      }
    }

    return (largestIndex, largestDigit);
  }
}
