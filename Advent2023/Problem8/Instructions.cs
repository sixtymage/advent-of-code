namespace Advent2023.Problem8
{
  internal class Instructions(Direction[] moves)
  {
    private readonly Direction[] _moves = moves;
    private int _index = moves.Length - 1;

    public static Instructions FromDescription(string description)
    {
      var moves = description
        .Select(c => c switch
        {
          'L' => Direction.Left,
          'R' => Direction.Right,
          var x => throw new Exception($"Unsupported direction '{x}', only permitted directions are 'L' and 'R'"),
        })
        .ToArray();

      if (moves.Length == 0)
      {
        throw new Exception("Unsupported list of moves, at least one move must be specified");
      }
      return new Instructions(moves);
    }

    public Direction GetNextMove()
    {
      _index = _index == _moves.Length - 1 ? 0 : _index + 1;
      return _moves[_index];
    }
  }
}
