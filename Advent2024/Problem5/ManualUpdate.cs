namespace Advent2024.Problem5;

public class ManualUpdate(int[] pages)
{
  public int[] Pages => pages;

  public int MiddlePage => pages[pages.Length / 2];
}
