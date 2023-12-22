using System.Diagnostics;

namespace Advent2023.Problem7;

public class Problem : IProblem
{
  private readonly string _filename;
  
  public Problem(string filename = @"data\problem7-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    var hands = new Hand[lines.Length];
    for (var i=0; i<hands.Length; i++)
    {
      hands[i] = RecoverHand(lines[i]);
    }

    Array.Sort(hands, new HandComparer());

    var winnings = CalculateWinnings(hands);
    Console.WriteLine($"Total winnings is {winnings}");
  }

  private static Hand RecoverHand(string line)
  {
    if (string.IsNullOrWhiteSpace(line))
    {
      throw new InvalidDataException("Unexpected input, line is empty");
    }

    var splitSpace = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (splitSpace.Length != 2 )
    {
      throw new InvalidDataException("Unexpected input, line should have exactly two components");
    }

    var cards = RecoverCards(splitSpace[0]);
    long bid = RecoverBid(splitSpace[1]);
    var handType = CalculateHandType(cards);

    return new Hand(cards, handType, bid);
  }

  private static List<Rank> RecoverCards(string cardsDescription)
  {
    var cards = new List<Rank>();
    foreach (var cardDescription in cardsDescription)
    {
      var rank = cardDescription switch
      {
        '2' => Rank.Two,
        '3' => Rank.Three,
        '4' => Rank.Four,
        '5' => Rank.Five,
        '6' => Rank.Six,
        '7' => Rank.Seven,
        '8' => Rank.Eight,
        '9' => Rank.Nine,
        'T' => Rank.Ten,
        'J' => Rank.Jack,
        'Q' => Rank.Queen,
        'K' => Rank.King,
        'A' => Rank.Ace,
        _ => throw new InvalidDataException($"Invalid card '{cardDescription}'"),
      };
      cards.Add(rank);
    }
    return cards;
  }

  private static long RecoverBid(string bidDescription)
  {
    return long.Parse(bidDescription);
  }

  private static HandType CalculateHandType(List<Rank> cards)
  {
    var groupedRanks = cards.GroupBy(c => c);

    if (groupedRanks.Count() == 1)
    {
      return HandType.FiveOfAKind;
    }

    if (groupedRanks.Count() == 2)
    {
      if (groupedRanks.Any(g => g.Count() == 4))
      {
        return HandType.FourOfAKind;
      }
      return HandType.FullHouse;
    }

    if (groupedRanks.Count() == 3)
    {
      if (groupedRanks.Any(g => g.Count() == 3))
      {
        return HandType.ThreeOfAKind;
      }
      return HandType.TwoPair;
    }

    if (groupedRanks.Count() == 4)
    {
      return HandType.OnePair;
    }

    Debug.Assert(groupedRanks.Count() == 5);
    return HandType.HighCard;
  }

  private static long CalculateWinnings(Hand[] hands)
  {
    long winnings = 0;

    for (var i=0; i<hands.Length; i++)
    {
      var handWinnings = (i+1) * hands[i].Bid;
      winnings += handWinnings;
    }

    return winnings;
  }
}
