using System.Xml;

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
    
    SolvePart1(traversedPath);
    SolvePart2(matrix, traversedPath);
  }

  private static void SolvePart2(Matrix<char> matrix, TraversedPath traversedPath)
  {
    var initialGuardLocation = CreateGuard(matrix).Location;
    
    var numLocationsChecked = 0;
    var matchingObstacles = new HashSet<Location>();
    foreach (var traversedLocation in traversedPath.TraversedLocations)
    {
      var candidateObstacleLocation = traversedLocation.GetNextLocation();
      
      if (DoesObstacleCauseLoop(matrix, candidateObstacleLocation, traversedLocation, initialGuardLocation))
      {
        if (!matchingObstacles.Add(candidateObstacleLocation))
        {
          WriteLine($"Ignoring location {candidateObstacleLocation} as it has already been matched", ConsoleColor.Yellow);
        }
        else
        {
          WriteLine($"Found matching obstacle at {candidateObstacleLocation} (num matches = {matchingObstacles.Count})", ConsoleColor.Cyan);
        }
      }
      
      numLocationsChecked++;
      if (numLocationsChecked % 10 == 0)
      {
        WriteLine($"Checked {numLocationsChecked}/{traversedPath.TraversedLocations.Count} locations", ConsoleColor.Gray);
      }
    }
    
    WriteLine($"Number of obstacles that cause looped guard behaviour: {matchingObstacles.Count}", ConsoleColor.White); 
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

  private static bool DoesObstacleCauseLoop(
    Matrix<char> matrix,
    Location candidateObstacleLocation,
    TraversedLocation guardLocation,
    Location initialGuardLocation)
  {
    if (candidateObstacleLocation == initialGuardLocation)
    {
      WriteLine($"Ignoring location {candidateObstacleLocation} as it is the guard's initial location", ConsoleColor.Yellow);
      return false;
    }
    
    var copiedMatrix = CopyMatrix(matrix);
    var map = CreateMap(copiedMatrix);

    if (!map.IsOnMap(candidateObstacleLocation))
    {
      WriteLine($"Ignoring location {candidateObstacleLocation} as it is off the map", ConsoleColor.Yellow);
      return false;
    }

    map.AddObstacle(candidateObstacleLocation);

    var guard = new Guard(guardLocation.Location, guardLocation.Direction);
    return map.DoesGuardLoop(guard);
  }

  private static Matrix<char> CopyMatrix(Matrix<char> matrix)
  {
    var source = new SubMatrixSource<char>(0, 0, matrix.Rows, matrix);
    return new Matrix<char>(source);
  }

  private static void SolvePart1(TraversedPath traversedPath)
  {
    var numDistinctPositions = CalcNumDistinctPositions(traversedPath);
    WriteLine($"Number of distinct positions visited by the guard: {numDistinctPositions}", ConsoleColor.White);
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
        switch (matrix[row, col])
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
    return traversedPath.TraversedLocations
      .Select(x => x.Location)
      .Distinct()
      .Count();
  }
}
