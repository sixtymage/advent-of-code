namespace Advent2023.Problem4
{
  internal class Card(int id, int numMatches, int score)
  {
    public int Id { get; } = id;

    public int NumMatches { get; } = numMatches;

    public int Score { get; } = score;

    public int Count { get; set; } = 1;
  }
}
