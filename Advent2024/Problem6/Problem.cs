using System.Diagnostics;
using System.Xml;

namespace Advent2024.Problem6;

public class Problem(string filename = @"data\problem6-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var matrix = ExtractInput(lines);

    Solve(matrix);
  }

  private static void Solve(Matrix<char> matrix)
  {
    var guard = CreateGuard(matrix);
    var initialGuardLocation = new Location(guard.Location);
    var map = CreateMap(matrix, initialGuardLocation);

    var path = new Path(guard.Location, guard.Direction);
    var obstacles = new HashSet<Location>();
    
    while (true)
    {
      var nextLocation = guard.GetNextLocation();

      if (!map.IsOnMap(nextLocation))
      {
        break;
      }

      if (map.IsObstacle(nextLocation))
      {
        guard.TurnRight();
        continue;
      }

      TryLocationAsObstacle(matrix, obstacles, nextLocation, path, map, guard, initialGuardLocation);

      guard.StepForward();
      path.AddLocation(guard.Location, guard.Direction);

      if (path.TraversedLocations.Count % 10 == 0)
      {
        WriteLine($"Traversed {path.TraversedLocations.Count} locations, obstacle count = {obstacles.Count}", ConsoleColor.Gray);
      }
    }

    var numDistinctPositions = CalcNumDistinctPositions(path);
    WriteLine($"Number of distinct locations visited by the guard: {numDistinctPositions}", ConsoleColor.White);
    WriteLine($"Number of obstacles that cause looped guard behaviour: {obstacles.Count}", ConsoleColor.White); 
  }

  private static void TryLocationAsObstacle(
    Matrix<char> matrix,
    HashSet<Location> obstacles,
    Location nextLocation,
    Path path,
    Map map,
    Guard guard,
    Location initialGuardLocation)
  {
    // we can't consider an obstacle here if it overlaps our path, or if we already have it as an obstacle
    if (obstacles.Contains(nextLocation) || path.ContainsLocation(nextLocation))
    {
      return;
    }

    var pathWithLoop = GetPathWithLoop(map, nextLocation, guard, initialGuardLocation);
    if (pathWithLoop == null)
    {
      return;
    }

    obstacles.Add(nextLocation);
    if (Debugger.IsAttached)
    {
      WritePathWithLoop(nextLocation, matrix, pathWithLoop);
    }
  }

  private static void WritePathWithLoop(Location obstacleLocation, Matrix<char> matrix, Path pathWithLoop)
  {
    var matrixCopy = new Matrix<char>(matrix);
    var map = new Map(matrixCopy);
    map.SetNewObstacle(obstacleLocation);
    foreach (var traversedLocation in pathWithLoop.TraversedLocations)
    {
      map.SetGuardPath(traversedLocation);
    }
    
    WriteMap(matrixCopy);
  }

  private static void WriteMap(Matrix<char> matrixCopy)
  {
    for (var row = 0; row < matrixCopy.Rows; row++)
    {
      for (var col = 0; col < matrixCopy.Cols; col++)
      {
        var colour = matrixCopy[row, col] switch
        {
          Map.EmptyLocation => ConsoleColor.Gray,
          Map.Obstacle => ConsoleColor.Red,
          Map.NewObstacle => ConsoleColor.White,
          Map.Up or Map.Right or Map.Down or Map.Left => ConsoleColor.Green,
          _ => throw new InvalidOperationException()
        };
        Write(matrixCopy[row, col], colour);
      }
      Console.WriteLine();
    }
  }

  private static void Write(char message, ConsoleColor colour)
  {
    var previousColour = Console.ForegroundColor;
    try
    {
      Console.ForegroundColor = colour;
      Console.Write(message);
    }
    finally
    {
      Console.ForegroundColor = previousColour;
    }
  }

  private static void WriteLine(string message, ConsoleColor colour)
  {
    var previousColour = Console.ForegroundColor;
    try
    {
      Console.ForegroundColor = colour;
      Console.WriteLine(message);
    }
    finally
    {
      Console.ForegroundColor = previousColour;
    }
  }

  private static Path? GetPathWithLoop(
    Map map,
    Location candidateObstacleLocation,
    Guard guard,
    Location initialGuardLocation)
  {
    if (candidateObstacleLocation == initialGuardLocation)
    {
      WriteLine($"Ignoring location {candidateObstacleLocation} as it is the guard's initial location", ConsoleColor.Yellow);
      return null;
    }
    
    if (!map.IsOnMap(candidateObstacleLocation))
    {
      WriteLine($"Ignoring location {candidateObstacleLocation} as it is off the map", ConsoleColor.Yellow);
      return null;
    }

    map.SetObstacle(candidateObstacleLocation);

    var phantomGuard = new Guard(guard);
    try
    {
      var (doesLoop, path) = map.DoesGuardLoop(phantomGuard);
      return doesLoop ? path : null;
    }
    finally
    {
      map.SetEmptyLocation(candidateObstacleLocation);
    }
  }

  private static Map CreateMap(Matrix<char> matrix, Location initialGuardLocation)
  {
    var map = new Map(matrix);
    map.SetEmptyLocation(initialGuardLocation);
    return map;
  }

  private static Guard CreateGuard(Matrix<char> matrix)
  {
    for (var row = 0; row < matrix.Rows; row++)
    {
      for (var col = 0; col < matrix.Cols; col++)
      {
        switch (matrix[row, col])
        {
          case Map.Up:
            return new Guard(new Location { Row = row, Col = col }, Direction.North);
          case Map.Right:
            return new Guard(new Location { Row = row, Col = col }, Direction.East);
          case Map.Down:
            return new Guard(new Location { Row = row, Col = col }, Direction.South);
          case Map.Left:
            return new Guard(new Location { Row = row, Col = col }, Direction.West);
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

  private static int CalcNumDistinctPositions(Path path)
  {
    return path.TraversedLocations
      .Select(x => x.Location)
      .Distinct()
      .Count();
  }
}
