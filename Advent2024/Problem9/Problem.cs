namespace Advent2024.Problem9;

public class Problem(string filename = @"data\problem9-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var fileSystem = new FileSystem(lines[0]);

    var before = fileSystem.Render();
    Console.WriteLine(before);
    Console.WriteLine();
    Compact(fileSystem);
    var after = fileSystem.Render();
    Console.WriteLine(after);
    Console.WriteLine();

    var checksum = fileSystem.CalculateChecksum();
    Console.WriteLine($"Checksum is {checksum}");
  }

  private static void Compact(FileSystem fileSystem)
  {
    while (true)
    {
      var sourceIndex = fileSystem.IndexOfLastFile();
      var targetIndex = fileSystem.IndexOfFirstEmptyBlock();

      fileSystem.Swap(sourceIndex, targetIndex);

      if (fileSystem.IsCompacted())
      {
        break;
      }
    }
  }
}