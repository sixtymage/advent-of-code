namespace Advent2024.Problem8;

public record Location
{
  public Location(Location from)
  {
    Row = from.Row;
    Col = from.Col;
  }

  public int Row { get; init; }

  public int Col { get; init; }
}
