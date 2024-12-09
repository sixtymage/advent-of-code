namespace Advent2024.Problem6;

public interface IMatrixSource<out T> where T: struct
{
  public int StartRow { get; }

  public int StartCol { get; }

  public int Rows { get; }

  public int Cols { get; }

  public T ElementAt(int row, int col);
}
