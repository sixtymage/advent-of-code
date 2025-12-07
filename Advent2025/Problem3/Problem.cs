


namespace Advent2025.Problem3;

public class Problem(string filename = @"data\problem3-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var joltage = CalculateJoltage(lines, 12);
    Console.WriteLine($"Answer is: {joltage}");
  }

  private static long CalculateJoltage(string[] lines, int numSelections)
  {
    long joltage = 0;
    foreach (var line in lines )
    {
      var lineJoltage = CalculateJoltage(line, numSelections);
      Console.WriteLine($"{line}: {lineJoltage}");

      joltage += lineJoltage;
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

      // find the largest value in this range
      (int index, long digit) = FindLargest(line, startIndex, lastUsableIndex);

      // update joltage
      joltage = joltage * 10 + digit;

      // update start index (must be to the right of chosen index)
      startIndex = index + 1;
    }

    return joltage;
  }

  private static (int index, long digit) FindLargest(string line, int startIndex, int endIndex)
  {
    long largestDigit = 0;
    int largestIndex = -1;
    for (int i = startIndex; i <= endIndex; i++)
    {
      long digit = Int64.Parse($"{line[i]}");

      if (digit > largestDigit)
      {
        largestDigit = digit;
        largestIndex = i;
      }
    }

    return (largestIndex, largestDigit);
  }
}
