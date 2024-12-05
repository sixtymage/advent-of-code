try
{
  var p = new Advent2024.Problem1.Problem();
  await p.SolveAsync();
}
catch (Exception e)
{
  Console.ForegroundColor = ConsoleColor.Red;
  Console.WriteLine($"Error: {e}");
}
