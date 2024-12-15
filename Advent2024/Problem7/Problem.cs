namespace Advent2024.Problem7;

public class Problem(string filename = @"data\problem7-input.txt") : IProblem
{
  private enum Operation
  {
    Add,
    Multiply,
    Concatenate
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var equations = ExtractEquations(lines);
    SolvePart1(equations);
    SolvePart2(equations);
  }

  private static void SolvePart1(Equation[] equations)
  {
    var sum = equations.Where(IsPossibleTwoOperators).Sum(e => e.Total);
    Console.WriteLine($"Part 1: Sum of qualifying numbers is {sum}");
  }

  private static void SolvePart2(Equation[] equations)
  {
    var sum = equations.Where(IsPossibleThreeOperators).Sum(e => e.Total);
    Console.WriteLine($"Part 2: Sum of qualifying numbers is {sum}");
  }

  private static Equation[] ExtractEquations(string[] lines)
  {
    var equations = new List<Equation>();
    foreach (var line in lines)
    {
      var inputs = line.Replace(": ",":").Split([':', ' ']);
      var total = long.Parse(inputs[0]);
      var arguments = inputs.Skip(1).Select(long.Parse);
      equations.Add(new Equation(total, arguments.ToArray()));
    }

    return equations.ToArray();
  }

  private static bool IsPossibleTwoOperators(Equation equation)
  {
    var numOperationPlaces = equation.Arguments.Length - 1;
    var numUniqueOptions = Math.Pow(2, numOperationPlaces);

    for (var option = 0; option < numUniqueOptions; option++)
    {
      var total = equation.Arguments[0];
      for (var i=1; i<equation.Arguments.Length; i++)
      {
        if ((option & (1 << (i-1))) == 0)
        {
          total += equation.Arguments[i];
        }
        else
        {
          total *= equation.Arguments[i];
        }
      }

      if (total == equation.Total)
      {
        return true;
      }
    }

    return false;
  }

  private static bool IsPossibleThreeOperators(Equation equation)
  {
    var numOperationPlaces = equation.Arguments.Length - 1;
    var numUniqueOptions = Math.Pow(3, numOperationPlaces);

    for (var option = 0; option < numUniqueOptions; option++)
    {
      var total = equation.Arguments[0];
      for (var i=1; i<equation.Arguments.Length; i++)
      {
        var op = GetOperationBaseThree(option, i - 1);

        total = op switch
        {
          Operation.Add => total + equation.Arguments[i],
          Operation.Multiply => total * equation.Arguments[i],
          Operation.Concatenate => Concatenate(total, equation.Arguments[i]),
          _ => throw new InvalidOperationException()
        };
      }

      if (total == equation.Total)
      {
        return true;
      }
    }

    return false;
  }

  private static long Concatenate(long left, long right)
  {
    var str = string.Join("", left, right);
    return long.Parse(str);
  }

  private static Operation GetOperationBaseThree(long option, int digit)
  {
    // 2012 in base 3 is the decimal number 2 * 3^0 + 1 * 3^1 + 0 * 3^2 + 2 * 3^3 = 2 + 3 + 0 + 54 = 59

    // so 59 in decimal is the base 3 number:
    // 0th digit = (option / 3^0) % 3 = 2
    // 1st digit = (option / 3^1) % 3 = 1
    // 2nd digit = (option / 3^2) % 3 = 0
    // 3rd digit = (option / 3^3) % 3 = 2

    // nth digit = (option / 3^(digit)) % 3
    var digitValue = option / (int)Math.Pow(3, digit) % 3;
    return digitValue switch
    {
      0 => Operation.Add,
      1 => Operation.Multiply,
      2 => Operation.Concatenate,
      _ => throw new InvalidOperationException()
    };
  }
}
