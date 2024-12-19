namespace Advent2024.Problem10;

public class DigitSource(IMatrixSource<char> source) : IMatrixSource<int>
{
  public int StartRow => source.StartRow;

  public int StartCol => source.StartCol;

  public int Rows => source.Rows;

  public int Cols => source.Cols;

  public int ElementAt(int row, int col)
  {
    return int.Parse($"{source.ElementAt(row, col)}");
  }
}
