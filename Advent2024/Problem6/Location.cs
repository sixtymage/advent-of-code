namespace Advent2024.Problem6;

public record Location
{
  public Location(Location from)
  {
    Row = from.Row;
    Col = from.Col;
  }

  public int Row { get; set; }

  public int Col { get; set; }
}
