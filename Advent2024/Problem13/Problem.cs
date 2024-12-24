using System.Diagnostics;

namespace Advent2024.Problem13;

public class Problem(string filename = @"data\problem13-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var clawMachines = ExtractClawMachines(lines);
    SolvePart1(clawMachines);
  }

  private static void SolvePart1(ClawMachine[] clawMachines)
  {
    long sum = 0;
    foreach (var clawMachine in clawMachines)
    {
      sum += SolveMachine(clawMachine);
    }

    Console.WriteLine($"Part 1: The fewest tokens is {sum}");
  }

  private static long SolveMachine(ClawMachine clawMachine)
  {
    var d = clawMachine.A.X * clawMachine.B.Y - clawMachine.B.X * clawMachine.A.Y;

    if (d == 0)
    {
      return SolveMachineSameVectorDirection(clawMachine);
    }

    var an = clawMachine.B.Y * clawMachine.Prize.X - clawMachine.Prize.Y * clawMachine.B.X;
    var bn = clawMachine.A.X * clawMachine.Prize.Y - clawMachine.Prize.X * clawMachine.A.Y;

    if (an % d != 0 || bn % d != 0)
    {
      return 0;
    }

    return an / d * 3 + bn / d;
  }

  private static long SolveMachineSameVectorDirection(ClawMachine clawMachine)
  {
    Debugger.Break();
    return 0;
  }

  private static ClawMachine[] ExtractClawMachines(string[] lines)
  {
    var clawMachines = new List<ClawMachine>();

    var step = 0;
    Vector? buttonA = null;
    Vector? buttonB = null;
    Vector? prize = null;

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
          prize = ExtractPrize(line);
          clawMachines.Add(new ClawMachine(buttonA!, buttonB!, prize!));
          break;
        default:
          throw new InvalidOperationException($"Unknown line: {line}");
      }

      step++;
    }

    return clawMachines.ToArray();
  }

  private static Vector? ExtractPrize(string line)
  {
    line = line.Replace($"Prize: X=", "");
    line = line.Replace(" Y=", "");
    var split = line.Split(',');
    return new Vector(int.Parse(split[0]), int.Parse(split[1]));
  }

  private static Vector ExtractButton(char button, string line)
  {
    line = line.Replace($"Button {button}: X+", "");
    line = line.Replace(" Y+", "");
    var split = line.Split(',');
    return new Vector(int.Parse(split[0]), int.Parse(split[1]));
  }
}
