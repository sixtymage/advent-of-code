namespace Advent2024.Problem4;

public class Matrix<T>(IMatrixDataSource<T> source)
  where T : struct
{
  private readonly T[,] _data = FromSource(source);

  private static T[,] FromSource(IMatrixDataSource<T> source)
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
