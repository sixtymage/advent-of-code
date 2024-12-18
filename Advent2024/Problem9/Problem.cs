namespace Advent2024.Problem9;

public class Problem(string filename = @"data\problem9-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    SolvePart1(lines);
    SolvePart2(lines);
  }

  private static void SolvePart1(string[] lines)
  {
    var fileSystem = new FileSystem(lines[0]);
    fileSystem.Compact();
    var checksum = fileSystem.CalculateChecksum();
    Console.WriteLine($"Part 1: Checksum is {checksum}");
  }

  private static void SolvePart2(string[] lines)
  {
    var fileSystem = new FileSystem(lines[0]);
    fileSystem.CompactWholeFiles();
    var checksum = fileSystem.CalculateChecksum();
    Console.WriteLine($"Part 2: Checksum is {checksum}");
  }
}
