namespace Advent2024.Problem14;

public class Robot(int x, int y,  int vx, int vy, int rows, int cols)
{
  private int _x = x;
  private int _y = y;

  public int X => _x;
  public int Y => _y;

  public bool IsInQuadrant(int quadrant)
  {
    var middleRow = rows / 2;
    var middleCol = cols / 2;

    return quadrant switch
    {
      0 => _x < middleCol && _y < middleRow,
      1 => _x > middleCol && _y < middleRow,
      2 => _x < middleCol && _y > middleRow,
      3 => _x > middleCol && _y > middleRow,
      _ => throw new ArgumentException($"Invalid robot position")
    };
  }

  public void Move(long numSeconds)
  {
    _x += (int)(vx * numSeconds);
    _x %= cols;
    _x = _x < 0 ? _x + cols : _x;
    _y += (int)(vy * numSeconds);
    _y %= rows;
    _y = _y < 0 ? _y + rows : _y;
  }

  public bool IsPosition(int row, int col)
  {
    return _x == col && _y == row;
  }
}
