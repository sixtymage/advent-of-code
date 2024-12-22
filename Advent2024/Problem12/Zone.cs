namespace Advent2024.Problem12;

public record Zone(char Crop, Location[] Locations)
{
  public int Area => Locations.Length;
}
