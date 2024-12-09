namespace Advent2024.Problem6;

public class Problem(string filename = @"data\problem6-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var matrix = ExtractInput(lines);

    var guard = CreateGuard(matrix);
    var map = CreateMap(matrix);
    var traversedPath = map.Traverse(guard);

    var numDistinctPositions = CalcNumDistinctPositions(traversedPath);
    Console.WriteLine($"Number of distinct positions visited by the guard: {numDistinctPositions}");
  }

  private static Map CreateMap(Matrix<char> matrix)
  {
    return new Map(matrix);
  }

  private static Guard CreateGuard(Matrix<char> matrix)
  {
    for (var row = 0; row < matrix.Rows; row++)
    {
      for (var col = 0; col < matrix.Cols; col++)
      {
        switch (matrix.ElementAt(row, col))
        {
          case '^':
            return new Guard(new Location { Row = row, Col = col }, Direction.North);
          case '>':
            return new Guard(new Location { Row = row, Col = col }, Direction.East);
          case '<':
            return new Guard(new Location { Row = row, Col = col }, Direction.West);
          case 'v':
            return new Guard(new Location { Row = row, Col = col }, Direction.South);
          default:
            continue;
        }
      }
    }

    throw new InvalidDataException("Guard not found in input");
  }

  private static Matrix<char> ExtractInput(string[] lines)
  {
    var source = new LinesSource(lines);
    return new Matrix<char>(source);
  }

  private static int CalcNumDistinctPositions(TraversedPath traversedPath)
  {
    return traversedPath.Locations.Distinct().Count();
  }
}
