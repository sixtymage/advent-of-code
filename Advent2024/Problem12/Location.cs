namespace Advent2024.Problem12;

public record Location(int Row, int Col)
{
  public bool DoesShareEdge(Location other)
  {
    var colDiff = Math.Abs(other.Col - Col);
    var rowDiff = Math.Abs(other.Row - Row);

    return (colDiff == 0 && rowDiff == 1) || (colDiff == 1 && rowDiff == 0);
  }
}
