namespace Advent2024.Problem3;

public record Instruction
{
  public int Left { get; init; } = 0;
  
  public int Right { get; init; } = 0;
}