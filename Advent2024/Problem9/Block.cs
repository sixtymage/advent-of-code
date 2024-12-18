namespace Advent2024.Problem9;

public record Block(int? Id)
{
  public bool IsEmpty => Id == null;

  public override string ToString()
  {
    return IsEmpty ? "." : $"{Id}";
  }
}
