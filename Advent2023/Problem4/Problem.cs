using Advent2023;

namespace Problem4;

public class Problem : IProblem
{
  private string _filename;

  public Problem(string filename = @"data\problem4-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    var sum = 0;
    foreach (var line in lines)
    {
      var score = CalculateCardScore(line);
      sum += score;
    }

    Console.WriteLine($"Sum of scores: {sum}");
  }

  private static int CalculateCardScore(string cardDescription)
  {
    (var winningNumbersDescription, var chosenNumbersDescription) = GetNumbersDescriptions(cardDescription);

    var winningNumbers = GetNumbers(winningNumbersDescription);
    var chosenNumbers = GetNumbers(chosenNumbersDescription);

    var numMatches = CalcNumMatches(winningNumbers, chosenNumbers);

    return CalculateScore(numMatches);
  }

  private static (string, string) GetNumbersDescriptions(string cardDescription)
  {
    var splitColon = cardDescription.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitColon.Length != 2)
    {
      throw new InvalidDataException("Unexpected format, ':' not found.");
    }

    var splitPipe = splitColon[1].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    return splitPipe.Length != 2
      ? throw new InvalidDataException("Unexpected format, ':' not found.")
      : (splitPipe[0], splitPipe[1]);
  }

  private static List<int> GetNumbers(string numbersDescription)
  {
    var split = numbersDescription.Split(' ');
    var numbers = split
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Select(x => int.Parse(x))
      .ToList();

    return CheckNumbers(numbersDescription, numbers);
  }

  private static List<int> CheckNumbers(string numbersDescription, List<int> numbers)
  {
    var distinct = numbers.Distinct();
    if (distinct.Count() != numbers.Count)
    {
      throw new InvalidDataException($"Invalid numbers, repeats found: {numbersDescription}");
    }
    return numbers;
  }

  private static int CalcNumMatches(List<int> winningNumbers, List<int> chosenNumbers)
  {
    return chosenNumbers.Intersect(winningNumbers).Count();
  }

  private static int CalculateScore(int numMatches)
  {
    if (numMatches <= 0)
    {
      return 0;
    }
    if (numMatches == 1)
    {
      return 1;
    }
    return 2 << (numMatches - 2);
  }
}
