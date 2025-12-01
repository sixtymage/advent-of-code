
namespace Advent2025.Problem1;

public class Problem(string filename = @"data\problem1-input.txt") : IProblem
{
  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var moves = LoadMoves(lines);

    int position = 50;
    int count = 0;

    foreach (var move in moves)
    {
      position = (position + move) % 100;

      count = position == 0 ? count + 1 : count;
    }

    Console.WriteLine($"Answer is: {count}");
  }

  private static IEnumerable<int> LoadMoves(string[] lines)
  {
    var moves = new List<int>();
    foreach (var line in lines)
    {
      var move = Int32.Parse(line[1..]);

      if (line[0] == 'L')
      {
        move *= -1;
      }

      moves.Add(move);
    }

    return moves;
  }
}
