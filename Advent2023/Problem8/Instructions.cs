namespace Advent2023.Problem8
{
  internal class Instructions(List<Direction> moves)
  {
    private readonly List<Direction> _moves = moves;
    private int _index = moves.Count - 1;

    public static Instructions FromDescription(string description)
    {
      var moves = description
        .Select(c => c switch
        {
          'L' => Direction.Left,
          'R' => Direction.Right,
          var x => throw new Exception($"Unsupported direction '{x}', only permitted directions are 'L' and 'R'"),
        })
        .ToList();

      if (moves.Count == 0)
      {
        throw new Exception("Unsupported list of moves, at least one move must be specified");
      }
      return new Instructions(moves);
    }

    public Direction GetNextMove()
    {
      _index = _index == _moves.Count - 1 ? 0 : _index + 1;
      return _moves[_index];
    }
  }
}
