namespace Advent2024.Problem6;

public class Guard(Location location, Direction direction)
{
  public Location Location { get; } = new(location);

  public Direction Direction { get; private set; } = direction;

  public Guard(Guard other) : this(other.Location, other.Direction)
  {
  }

  public void StepForward()
  {
    switch (Direction)
    {
      case Direction.North:
        Location.Row--;
        break;

      case Direction.East:
        Location.Col++;
        break;

      case Direction.South:
        Location.Row++;
        break;

      case Direction.West:
        Location.Col--;
        break;

      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void TurnRight()
  {
    Direction = Direction switch
    {
      Direction.North => Direction.East,
      Direction.East => Direction.South,
      Direction.South => Direction.West,
      Direction.West => Direction.North,
      _ => throw new ArgumentOutOfRangeException()
    };
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
