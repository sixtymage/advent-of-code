namespace Advent2024.Problem4;

public class SubMatrixSource<T>(int startRow, int startCol, int length, Matrix<T> matrix)
  : IMatrixSource<T>
  where T : struct
{
  private readonly Matrix<T> _matrix = VerifySubMatrix(startRow, startCol, length, matrix);

  public int StartRow => startRow;

  public int StartCol => startCol;

  public int Rows => length;

  public int Cols => length;

  public T ElementAt(int row, int col)
  {
    return _matrix.ElementAt(startRow + row, startCol + col);
  }

  private static Matrix<T> VerifySubMatrix(int startRow, int startCol, int length, Matrix<T> matrix)
  {
    if (startRow + length > matrix.Rows || startCol + length > matrix.Cols)
    {
      throw new ArgumentException($"Invalid sub matrix extent ({startRow}, {startCol}, {length}).");
    }
    return matrix ?? throw new ArgumentNullException(nameof(matrix));
  }
}
