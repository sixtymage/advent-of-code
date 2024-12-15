namespace Advent2024.Problem7;

public class Problem(string filename = @"data\problem7-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);
    var equations = ExtractEquations(lines);
    var sum = equations.Where(IsPossible).Sum(e => e.Total);

    Console.WriteLine($"Sum of qualifying numbers is {sum}");
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

  private static bool IsPossible(Equation equation)
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
}
