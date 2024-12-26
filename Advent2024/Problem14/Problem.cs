using System.Diagnostics;

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
    SolvePart2(lines, rows, cols, showSteps);
  }

  private static void SolvePart2(string[] lines, int rows, int cols, bool showSteps)
  {
    var robots = ExtractRobots(lines, rows, cols);

    long numSeconds = 0;
    long bestSecond = 0;
    var bestScore = 0;
    const long target = 1_000_000;
    const int increment = 1;

    var sw = Stopwatch.StartNew();

    while (true)
    {
      MoveRobots(robots, increment, rows, cols, showSteps);
      numSeconds += increment;

      var score = GetScore(robots);
      if (score > bestScore)
      {
        bestScore = score;
        bestSecond = numSeconds;
      }

      if (numSeconds == target)
      {
        break;
      }

      if (numSeconds / increment % 100_000 == 0)
      {
        var rate = numSeconds / sw.Elapsed.TotalSeconds;
        var remainingTimeSeconds = (target - numSeconds) / rate;
        var suffix = $" ({numSeconds}/{target}, {TimeSpan.FromSeconds(remainingTimeSeconds)} remaining)";
        Console.WriteLine(
          $"{sw.Elapsed}: Current Second: {numSeconds} Current Score: {score} Best Second: {bestSecond} Best Score: {bestScore} {suffix}");
      }
    }

    Console.WriteLine($"Part 2: The answer is probably {bestSecond} with score {bestScore}");

    var solutionRobots = ExtractRobots(lines, rows, cols);
    MoveRobots(solutionRobots, bestSecond, rows, cols, true);
  }

  private static int GetScore(Robot[] robots)
  {
    var groups = robots.GroupBy(r => r.Y);

    var score = 0;
    foreach (var group in groups)
    {
      var orderedRobots = group
        .OrderBy(r => r.X)
        .ToArray();

      for (var r = 1; r < orderedRobots.Length; r++)
      {
        var distance = Math.Abs(orderedRobots[r - 1].X - orderedRobots[r].X);
        score = distance == 1
          ? score + 1
          : score;
      }
    }

    return score;
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

  private static void MoveRobots(Robot[] robots, long seconds, int rows, int cols, bool showSteps)
  {
    foreach (var robot in robots)
    {
      robot.Move(seconds);
    }

    WriteRobots(robots, rows, cols, showSteps);
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
      int.Parse(position[0]),
      int.Parse(position[1]),
      int.Parse(velocity[0]),
      int.Parse(velocity[1]),
      rows,
      cols);
  }
}
