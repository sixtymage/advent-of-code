using System.Diagnostics;

namespace Advent2024.Problem13;

public class Problem(string filename = @"data\problem13-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    SolvePart1(lines);
    SolvePart2(lines);
  }

  private void SolvePart2(string[] lines)
  {
    var clawMachines = ExtractClawMachines(lines, 10_000_000_000_000);
    var sum = CalcFewestTokens(clawMachines);

    Console.WriteLine($"Part 2: The fewest tokens is {sum}");
  }

  private static void SolvePart1(string[] lines)
  {
    var clawMachines = ExtractClawMachines(lines, 0);
    var sum = CalcFewestTokens(clawMachines);

    Console.WriteLine($"Part 1: The fewest tokens is {sum}");
  }

  private static long CalcFewestTokens(ClawMachine[] clawMachines)
  {
    long sum = 0;
    foreach (var clawMachine in clawMachines)
    {
      sum += SolveMachine(clawMachine);
    }

    return sum;
  }

  private static long SolveMachine(ClawMachine clawMachine)
  {
    var d = clawMachine.A.X * clawMachine.B.Y - clawMachine.B.X * clawMachine.A.Y;

    if (d == 0)
    {
      return HandleSameVectorDirection();
    }

    var an = clawMachine.B.Y * clawMachine.Prize.X - clawMachine.Prize.Y * clawMachine.B.X;
    var bn = clawMachine.A.X * clawMachine.Prize.Y - clawMachine.Prize.X * clawMachine.A.Y;

    if (an % d != 0 || bn % d != 0)
    {
      return 0;
    }

    return an / d * 3 + bn / d;
  }

  private static long HandleSameVectorDirection()
  {
    throw new InvalidDataException("The claw machine solver doesn't support vectors with the same direction");
  }

  private static ClawMachine[] ExtractClawMachines(string[] lines, long prizeOffset)
  {
    var clawMachines = new List<ClawMachine>();

    var step = 0;
    Vector? buttonA = null;
    Vector? buttonB = null;

    foreach (var line in lines)
    {
      if (line == string.Empty)
      {
        step = 0;
        continue;
      }

      switch (step)
      {
        case 0:
          buttonA = ExtractButton('A', line);
          break;
        case 1:
          buttonB = ExtractButton('B', line);
          break;
        case 2:
          var prize = ExtractPrize(line, prizeOffset);
          clawMachines.Add(new ClawMachine(buttonA!, buttonB!, prize));
          break;
        default:
          throw new InvalidOperationException($"Unknown line: {line}");
      }

      step++;
    }

    return clawMachines.ToArray();
  }

  private static Vector ExtractPrize(string line, long offset)
  {
    line = line.Replace($"Prize: X=", "");
    line = line.Replace(" Y=", "");
    var split = line.Split(',');
    return new Vector(long.Parse(split[0]) + offset, long.Parse(split[1]) + offset);
  }

  private static Vector ExtractButton(char button, string line)
  {
    line = line.Replace($"Button {button}: X+", "");
    line = line.Replace(" Y+", "");
    var split = line.Split(',');
    return new Vector(long.Parse(split[0]), long.Parse(split[1]));
  }
}
