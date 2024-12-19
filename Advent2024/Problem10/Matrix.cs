namespace Advent2024.Problem10;

public class Matrix<T>(IMatrixSource<T> source) : IMatrixSource<T>
  where T : struct
{
  private readonly T[,] _data = FromSource(source);

  public int StartRow => source.StartRow;

  public int StartCol => source.StartCol;

  public int Rows => _data.GetLength(0);

  public int Cols => _data.GetLength(1);

  public T ElementAt(int row, int col)
  {
    return _data[row, col];
  }
  
   public T this[int row, int col]
   {
     get => _data[row, col];
     set => _data[row, col] = value;
   }

  private static T[,] FromSource(IMatrixSource<T> source)
  {
    var rows = source.Rows;
    var cols = source.Cols;
    var data = new T[rows, cols];
    for (var row = 0; row < rows; row++)
    {
      for (var col = 0; col < cols; col++)
      {
        data[row, col] = source.ElementAt(row, col);
      }
    }

    return data;
  }
}
