
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
      (int newPosition, int zeroHits) = PerformMove(position, move, false);

      position = newPosition;
      count += zeroHits;
    }

    Console.WriteLine($"Answer is: {count}");
  }

  private static (int, int) PerformMove(int position, int move, bool isPart1)
  {
    return isPart1
      ? PerformMovePart1(position, move)
      : PerformMovePart2(position, move);
  }

  private static (int, int) PerformMovePart2(int position, int move)
  {
    var numClicks = 0;
    while (move != 0)
    {
      if (move < 0)
      {
        position = MovePosition(position, -1);
        move++;
      }
      else
      {
        position = MovePosition(position, 1);
        move--;
      }

      if (position == 0)
      {
        numClicks++;
      }
    }

    return (position, numClicks);
  }

  private static int MovePosition(int position, int move)
  {
    int newPosition = (position + move) % 100;

    return newPosition < 0
      ? newPosition + 100 
      : newPosition;
  }

  private static (int, int) PerformMovePart1(int position, int move)
  {
    // this is easy - just add move and mod by 100 and see if we landed on 0
    position = MovePosition(position, move);
    int numClicks = position == 0 ? 1 : 0;

    return (position, numClicks);
  }

  private static List<int> LoadMoves(string[] lines)
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
