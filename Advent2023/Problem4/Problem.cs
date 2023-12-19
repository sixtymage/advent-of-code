using Advent2023;
using Advent2023.Problem4;
using static System.Formats.Asn1.AsnWriter;

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

    var cards = new List<Card>();
    foreach (var line in lines)
    {
      var card = CalculateCard(line);
      cards.Add(card);
    }

    UpdateCopies(cards);

    var sum = cards.Sum(x => x.Score);
    Console.WriteLine($"Sum of scores: {sum}");

    var countCards = cards.Sum(x => x.Count);
    Console.WriteLine($"Count of cards: {countCards}");
  }

  private static void UpdateCopies(List<Card> cards)
  {
    foreach (var card in cards)
    {
      var numNextCards = card.NumMatches;
      var startIndex = card.Id;

      for (var i = 0; i < numNextCards; i++)
      {
        cards[startIndex + i].Count += card.Count;
      }
    }
  }

  private static Card CalculateCard(string cardDescription)
  {
    (var cardId, var winningNumbersDescription, var chosenNumbersDescription) = GetCardDetails(cardDescription);

    var winningNumbers = GetNumbers(winningNumbersDescription);
    var chosenNumbers = GetNumbers(chosenNumbersDescription);

    var numMatches = CalcNumMatches(winningNumbers, chosenNumbers);
    var score = CalcScore(numMatches);

    return new Card(cardId, numMatches, score);
  }

  private static (int, string, string) GetCardDetails(string cardDescription)
  {
    var splitColon = cardDescription.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitColon.Length != 2)
    {
      throw new InvalidDataException("Unexpected format, ':' not found.");
    }

    var splitSpace = splitColon[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitSpace.Length != 2)
    {
      throw new InvalidDataException("Unexpected format, ' ' not found.");
    }
    var cardId = int.Parse(splitSpace[1]);

    var splitPipe = splitColon[1].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    return splitPipe.Length != 2
      ? throw new InvalidDataException("Unexpected format, ':' not found.")
      : (cardId, splitPipe[0], splitPipe[1]);
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

  private static int CalcScore(int numMatches)
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
