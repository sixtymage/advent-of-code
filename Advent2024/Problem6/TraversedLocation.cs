namespace Advent2024.Problem6;

public record TraversedLocation
{
  public Location Location { get; }

  public Direction Direction { get; }

  public TraversedLocation(Location location, Direction direction)
  {
    Location = new Location(location);
    Direction = direction;
  }
}
