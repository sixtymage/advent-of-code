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

  public int[] GetCandidateCorrection(int[] pages)
  {
    var indexedPages = pages.Index().ToArray();
    var beforeIndex = indexedPages.First(ip => ip.Item == before).Index;
    var afterIndex = indexedPages.First(ip => ip.Item == after).Index;
    (indexedPages[beforeIndex], indexedPages[afterIndex]) = (indexedPages[afterIndex], indexedPages[beforeIndex]);
    return indexedPages.Select(x => x.Item).ToArray();
  }
}
