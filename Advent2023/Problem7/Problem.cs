using System.Diagnostics;

namespace Advent2023.Problem7;

public class Problem : IProblem
{
  private readonly string _filename;
  private readonly bool _useJoker;

  public Problem(string filename = @"data\problem7-input.txt", bool useJoker = false)
  {
    _filename = filename;
    _useJoker = useJoker;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    var hands = new Hand[lines.Length];
    for (var i=0; i<hands.Length; i++)
    {
      hands[i] = RecoverHand(lines[i], _useJoker);
    }

    Array.Sort(hands, new HandComparer());

    var winnings = CalculateWinnings(hands);
    Console.WriteLine($"Total winnings is {winnings}");
  }

  private static Hand RecoverHand(string line, bool useJoker)
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

    var cards = RecoverCards(splitSpace[0], useJoker);
    long bid = RecoverBid(splitSpace[1]);
    var handType = CalculateHandType(cards);

    return new Hand(cards, handType, bid);
  }

  private static List<Rank> RecoverCards(string cardsDescription, bool useJoker)
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
        'J' => useJoker ? Rank.Joker : Rank.Jack,
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
    var hand = ExpandJokers(cards);
    return CalculateHandTypeNoJoker(hand);
  }

  private static List<Rank> ExpandJokers(List<Rank> cards)
  {
    var jokerCount = cards.Count(c => c == Rank.Joker);

    return jokerCount switch
    {
      0 => cards,
      1 => ExpandOneJoker(cards),
      2 => ExpandTwoJokers(cards),
      3 => ExpandThreeJokers(cards),
      4 => ExpandFourJokers(cards),
      5 => ExpandFiveJokers(cards),
      _ => throw new Exception("More than 5 Jokers is not supported"),
    };
  }

  private static List<Rank> ExpandOneJoker(List<Rank> cards)
  {
    var nonJokerGroups = cards
      .Where(c => c != Rank.Joker)
      .GroupBy(c => c)
      .ToList();

    // if all 4 non-joker cards are the same rank, the best hand is 5 of a kind
    if (nonJokerGroups.Count == 1)
    {
      var card = nonJokerGroups[0].Key;
      return [card, card, card, card, card];
    }

    // if we have two groups of 4 non-joker cards they could be split 1/3 or 2/2
    if (nonJokerGroups.Count == 2)
    {
      var card1 = nonJokerGroups[0].Key;
      var card2 = nonJokerGroups[1].Key;

      // if they are split 1/3, allocate to make a 4 of a kind
      if (nonJokerGroups[0].Count() == 1)
      {
        return [card2, card2, card2, card2, card1];
      }
      if (nonJokerGroups[0].Count() == 3)
      {
        return [card1, card1, card1, card1, card2];
      }


      // otherwise they are split 2/2 - allocate to make the best full house
      if (card1 > card2)
      {
        return [card1, card1, card1, card2, card2];
      }
      return [card2, card2, card2, card1, card1];
    }

    // if we have 3 groups of 4 non-joker cards, they must be split 1/1/2
    if (nonJokerGroups.Count == 3)
    {
      var card1 = nonJokerGroups[0].Key;
      var card2 = nonJokerGroups[1].Key;
      var card3 = nonJokerGroups[2].Key;

      // it's best to allocate the joker to the 2, making 3 of a kind (allocating to either 1 would make only two pair)
      if (nonJokerGroups[0].Count() == 2)
      {
        return [card1, card1, card1, card2, card3];
      }
      if (nonJokerGroups[1].Count() == 2)
      {
        return [card2, card2, card2, card1, card3];
      }
      Debug.Assert(nonJokerGroups[2].Count() == 2);
      return [card3, card3, card3, card1, card2];
    }

    // if we have 4 groups of 4 non-joker cards, they must be split 1/1/1/1, and we make a one pair with the best card
    var nonJokerCards = nonJokerGroups
      .Select(c => c.Key)
      .ToList();
    nonJokerCards.Sort();
    return [nonJokerCards[3], nonJokerCards[3], nonJokerCards[2], nonJokerCards[1], nonJokerCards[0]];
  }

  private static List<Rank> ExpandTwoJokers(List<Rank> cards)
  {
    var nonJokerGroups = cards
      .Where(c => c != Rank.Joker)
      .GroupBy(c => c)
      .ToList();

    // if all 3 non-joker cards are the same rank, the best hand is 5 of a kind
    if (nonJokerGroups.Count == 1)
    {
      var card = nonJokerGroups[0].Key;
      return [card, card, card, card, card];
    }

    // if we have a 2/1 split, the best would be 4 of a kind of the rank with 2 cards
    if (nonJokerGroups.Count == 2)
    {
      var card1 = nonJokerGroups[0].Key;
      var card2 = nonJokerGroups[1].Key;

      if (nonJokerGroups[0].Count() == 2)
      {
        return [card1, card1, card1, card1, card2];
      }
      return [card2, card2, card2, card2, card1];
    }

    // finally if we have all 3 cards different, we make a three of a kind with the best card
    var nonJokerCards = nonJokerGroups
      .Select(c => c.Key)
      .ToList();
    nonJokerCards.Sort();
    return [nonJokerCards[2], nonJokerCards[2], nonJokerCards[2], nonJokerCards[1], nonJokerCards[0]];
  }

  private static List<Rank> ExpandThreeJokers(List<Rank> cards)
  {
    var nonJokers = cards.Where(c => c != Rank.Joker);
    Debug.Assert(nonJokers.Count() == 2);

    var card1 = nonJokers.First();
    var card2 = nonJokers.Last();

    // if the two non-joker cards are the same rank, the best hand is 5 of a kind of this rank
    if (card1 == card2)
    {
      return [card1, card1, card1, card1, card1];
    }

    // if the two non-joker cards are different ranks, best hand is 4 of a kind of the card of better rank
    if (card1 > card2)
    {
      return [card1, card1, card1, card1, card2];
    }
    return [card2, card2, card2, card2, card1];
  }

  private static List<Rank> ExpandFourJokers(List<Rank> cards)
  {
    var nonJoker = cards.Where(c => c != Rank.Joker).First();
    return [nonJoker, nonJoker, nonJoker, nonJoker, nonJoker];
  }

  private static List<Rank> ExpandFiveJokers(List<Rank> cards)
  {
    return [Rank.Ace, Rank.Ace, Rank.Ace, Rank.Ace, Rank.Ace];
  }

  private static HandType CalculateHandTypeNoJoker(List<Rank> cards)
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
