namespace Advent2024.Problem4;

public interface IMatrixDataSource<out T> where T: struct
{
  public int Rows { get; }
  
  public int Cols { get; }

  public T ElementAt(int row, int col);
}
