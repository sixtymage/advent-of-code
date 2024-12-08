namespace Advent2024.Problem4;

public class LinesSource(string[] lines) : IMatrixSource<char>
{
  private readonly string[] _lines = VerifyLines(lines);

  public int StartRow => 0;

  public int StartCol => 0;

  public int Rows => _lines.Length;

  public int Cols => _lines[0].Length;

  public char ElementAt(int row, int col)
  {
    return _lines[row][col];
  }
  
  private static string[] VerifyLines(string[] lines)
  {
    lines = lines ?? throw new ArgumentNullException(nameof(lines));

    if (lines.Length == 0)
    {
      throw new InvalidDataException("Input data should must have at least one row.");
    }

    if (lines[0].Length == 0)
    {
      throw new InvalidDataException("No line of input data can have 0 characters.");
    }

    if (lines.GroupBy<string, object>(x => x.Length).Count() != 1)
    {
      throw new InvalidDataException("Each line of input data must be the same length.");
    }

    return lines;
  }
}
