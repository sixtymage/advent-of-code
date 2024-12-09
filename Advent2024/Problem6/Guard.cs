namespace Advent2024.Problem6;

public class Guard(Location location, Direction direction)
{
  public Direction Direction { get; private set; } = direction;

  public Location Location { get; private set; } = location;

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
}
