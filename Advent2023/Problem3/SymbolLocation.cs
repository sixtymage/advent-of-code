namespace Advent2023.Problem3
{
  internal class SymbolLocation(int row, int col, char symbol)
  {
    public int Row { get; } = row;

    public int Col { get; } = col;

    public char Symbol { get; } = symbol;
  }
}
