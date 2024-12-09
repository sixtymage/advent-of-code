namespace Advent2024.Problem6;

public class Map(Matrix<char> matrix)
{
  private const char Obstacle = '#';

  public TraversedPath Traverse(Guard guard)
  {
    var traversedPath = new TraversedPath(guard.Location);

    while (true)
    {
      while (IsDirectionBlocked(guard.Location, guard.Direction))
      {
        guard.TurnRight();
      }

      guard.StepForward();

      if (!IsOnMap(guard.Location))
      {
        return traversedPath;
      }
      traversedPath.AddLocation(guard.Location);
    }
  }

  private bool IsOnMap(Location location)
  {
    return location.Row >= 0 && location.Row < matrix.Rows && location.Col >= 0 && location.Col < matrix.Cols;
  }

  private bool IsDirectionBlocked(Location location, Direction direction)
  {
    return direction switch
    {
      Direction.North => IsBlocked(-1, 0, location),
      Direction.East => IsBlocked(0, 1, location),
      Direction.South => IsBlocked(1, 0, location),
      Direction.West => IsBlocked(0, -1, location),
      _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
  }

  private bool IsBlocked(int rowOffset, int colOffset, Location location)
  {
    var offsetLocation = new Location { Row = location.Row + rowOffset, Col = location.Col + colOffset };
    return IsOnMap(offsetLocation) && matrix.ElementAt(offsetLocation.Row, offsetLocation.Col) == Obstacle;
  }
}
