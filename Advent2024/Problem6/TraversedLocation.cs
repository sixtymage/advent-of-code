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

  public Location GetNextLocation()
  {
    return Direction switch
    {
      Direction.North => Location with { Row = Location.Row - 1 },
      Direction.East => Location with { Col  = Location.Col + 1 },
      Direction.South => Location with { Row = Location.Row + 1 },
      Direction.West => Location with { Col  = Location.Col - 1 },
      _ => throw new ArgumentOutOfRangeException()
    };
  }
}
