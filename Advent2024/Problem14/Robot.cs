namespace Advent2024.Problem14;

public class Robot(Vector position, Vector velocity, int rows, int cols)
{
  private Vector _position = position;

  public bool IsInQuadrant(int quadrant)
  {
    var middleRow = rows / 2;
    var middleCol = cols / 2;

    return quadrant switch
    {
      0 => _position.X < middleCol && _position.Y < middleRow,
      1 => _position.X > middleCol && _position.Y < middleRow,
      2 => _position.X < middleCol && _position.Y > middleRow,
      3 => _position.X > middleCol && _position.Y > middleRow,
      _ => throw new ArgumentException($"Invalid robot position: {_position}"),
    };
  }

  public void Move()
  {
    var newX = (_position.X + velocity.X + cols) % cols;
    var newY = (_position.Y + velocity.Y + rows) % rows;
    _position = new Vector(newX, newY);
  }

  public bool IsPosition(int row, int col)
  {
    return _position.X == col && _position.Y == row;
  }
}
