namespace Advent2024.Problem5;

public class PageOrderingRule(int before, int after)
{
  public bool IsForPages(int[] pages)
  {
    return pages.Contains(before) && pages.Contains(after);
  }

  public bool IsSatisfied(int[] pages)
  {
    var beforeFound = false;
    var afterFound = false;
    foreach (var page in pages)
    {
      beforeFound = !beforeFound && page == before;
      afterFound = !afterFound && page == after;

      if (beforeFound)
      {
        return true;
      }

      if (afterFound)
      {
        return beforeFound;
      }
    }

    return false;
  }
}
