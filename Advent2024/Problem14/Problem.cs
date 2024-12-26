namespace Advent2024.Problem14;

public class Problem(string filename = @"data\problem14-input.txt") : IProblem
{
  private readonly Dictionary<string, (int, int, bool)> _sizes = new()
  {
    { "problem14-input-example.txt", (7, 11, false) },
    { "problem14-input.txt", (103, 101, false) }
  };

  public async Task SolveAsync()
  {
    var rows = _sizes[Path.GetFileName(filename)].Item1;
    var cols = _sizes[Path.GetFileName(filename)].Item2;
    var showSteps = _sizes[Path.GetFileName(filename)].Item3;

    var lines = await File.ReadAllLinesAsync(filename);

    SolvePart1(lines, rows, cols, showSteps);
  }

  private static void SolvePart1(string[] lines, int rows, int cols, bool showSteps)
  {
    var robots = ExtractRobots(lines, rows, cols);
    MoveRobots(robots, 100, rows, cols, showSteps);
    var answer = CalcQuadrants(robots);

    Console.WriteLine($"Part 1: The answer is {answer}");
  }

  private static int CalcQuadrants(Robot[] robots)
  {
    var safetyFactor = 1;
    for (var i = 0; i < 4; i++)
    {
      var qCount = robots.Count(r => r.IsInQuadrant(i));
      safetyFactor *= qCount;
    }

    return safetyFactor;
  }

  private static void MoveRobots(Robot[] robots, int seconds, int rows, int cols, bool showSteps)
  {
    WriteRobots(robots, rows, cols, showSteps);

    for (var i = 0; i < seconds; i++)
    {
      foreach (var robot in robots)
      {
        robot.Move();
        WriteRobots(robots, rows, cols, showSteps);
      }
    }
  }

  private static void WriteRobots(Robot[] robots, int rows, int cols, bool showSteps)
  {
    if (!showSteps)
    {
      return;
    }

    for (var row = 0; row < rows; row++)
    {
      for (var col = 0; col < cols; col++)
      {
        var sum = robots.Count(r => r.IsPosition(row, col));
        switch (sum)
        {
          case 0:
            WriteString(".", ConsoleColor.Gray);
            break;
          case > 0 and < 10:
            WriteString($"{sum}", ConsoleColor.Cyan);
            break;
          case > 10:
            WriteString("+", ConsoleColor.Yellow);
            break;
        }
      }

      Console.WriteLine();
    }

    Console.WriteLine();
  }

  private static void WriteString(string value, ConsoleColor colour)
  {
    var oldColor = Console.ForegroundColor;
    try
    {
      Console.ForegroundColor = colour;
      Console.Write(value);
    }
    finally
    {
      Console.ForegroundColor = oldColor;
    }
  }

  private static Robot[] ExtractRobots(string[] lines, int rows, int cols)
  {
    var robots = new List<Robot>();

    foreach (var line in lines)
    {
      var robot = ExtractRobot(line, rows, cols);
      robots.Add(robot);
    }

    return robots.ToArray();
  }

  private static Robot ExtractRobot(string line, int rows, int cols)
  {
    var split = line.Split(' ');
    var position = split[0].Replace("p=", "").Split(',');
    var velocity = split[1].Replace("v=", "").Split(',');
    return new Robot(
      new Vector(int.Parse(position[0]), int.Parse(position[1])),
      new Vector(int.Parse(velocity[0]), int.Parse(velocity[1])),
      rows,
      cols);
  }
}
