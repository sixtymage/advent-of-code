namespace Advent2023.Problem7
{
  internal class Hand(List<Rank> cards, HandType handType, long bid)
  {
    public List<Rank> Cards { get; } = cards;

    public HandType HandType { get; } = handType;

    public long Bid { get; } = bid;
  }
}
