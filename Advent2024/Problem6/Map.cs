namespace Advent2024.Problem6;

public class Map(Matrix<char> matrix)
{
  public const char Obstacle = '#';
  public const char EmptyLocation = '.';
  public const char NewObstacle = 'O';
  public const char Up = '^';
  public const char Right = '>';
  public const char Down = 'v';
  public const char Left = '<';

  public bool IsOnMap(Location location)
  {
    return location.Row >= 0 && location.Row < matrix.Rows && location.Col >= 0 && location.Col < matrix.Cols;
  }

  public bool IsObstacle(Location location)
  {
    if (!IsOnMap(location))
    {
      return false;
    }

    return matrix[location.Row, location.Col] == Obstacle;
  }

  public void SetObstacle(Location location)
  {
    matrix[location.Row, location.Col] = Obstacle;
  }
  
  public void SetEmptyLocation(Location location)
  {
    matrix[location.Row, location.Col] = EmptyLocation;
  }
  public void SetNewObstacle(Location location)
  {
    matrix[location.Row, location.Col] = NewObstacle;
  }

  public void SetGuardPath(TraversedLocation traversedLocation)
  {
    matrix[traversedLocation.Location.Row, traversedLocation.Location.Col] = traversedLocation.Direction switch
    {
      Direction.North => Up,
      Direction.East => Right,
      Direction.South => Down,
      Direction.West => Left,
      _ => throw new InvalidOperationException()
    };
  }

  public (bool, Path) DoesGuardLoop(Guard guard)
  {
    var path = new Path(guard.Location, guard.Direction);
    
    while (true)
    {
      while (IsDirectionBlocked(guard.Location, guard.Direction))
      {
        guard.TurnRight();
      }

      guard.StepForward();

      if (!IsOnMap(guard.Location))
      {
        return (false, path);
      }

      if (IsLoop(guard, path))
      {
        return (true, path);
      }

      path.AddLocation(guard.Location, guard.Direction);
    }
  }

  private static bool IsLoop(Guard guard, Path path)
  {
    return path.TraversedLocations.Any(tl => tl == new TraversedLocation(guard.Location, guard.Direction));
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
    return IsOnMap(offsetLocation) && matrix[offsetLocation.Row, offsetLocation.Col] == Obstacle;
  }
}
