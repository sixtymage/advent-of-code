
using System.Diagnostics;

namespace Advent2023.Problem7
{
  internal class HandComparer : IComparer<Hand>
  {
    public int Compare(Hand? x, Hand? y)
    {
      return (x != null && y != null)
        ? CompareNonNull(x, y)
        : CompareNull(x, y);
    }

    private static int CompareNonNull(Hand x, Hand y)
    {
      if (x.HandType < y.HandType)
      {
        return -1;
      }
      if (x.HandType > y.HandType)
      {
        return 1;
      }

      return CompareCards(x, y);
    }

    private static int CompareCards(Hand x, Hand y)
    {
      Debug.Assert(x.Cards.Count == y.Cards.Count);

      for (var i = 0; i < x.Cards.Count; i++)
      {
        if (x.Cards[i] < y.Cards[i])
        {
          return -1;
        }
        if (x.Cards[i] > y.Cards[i])
        {
          return 1;
        }
      }
      return 0;
    }

    private static int CompareNull(Hand? x, Hand? y)
    {
      if (x == null && y == null)
      {
        return 0;
      }

      if (x == null && y != null)
      {
        return -1;
      }

      if (x != null && y == null)
      {
        return 1;
      }

      throw new Exception("At least one hand should be null");
    }
  }
}
