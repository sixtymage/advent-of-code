namespace Advent2024.Problem10;

public class Map(Matrix<char> matrix)
{
  private const char EmptyLocation = '.';

  public bool IsOnMap(Location location)
  {
    return location.Row >= 0 && location.Row < matrix.Rows && location.Col >= 0 && location.Col < matrix.Cols;
  }
}
