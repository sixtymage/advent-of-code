namespace Advent2023.Problem8
{
  internal class MapNode(string id, string left, string right)
  {
    private const string StartToken = "A";
    private const string EndToken = "Z";

    public string Id { get; } = id;
    private string Left { get; } = left;
    private string Right { get; } = right;

    public string GetNextNode(Direction direction)
    {
      if (direction == Direction.Left)
      {
        return Left;
      }
      if (direction == Direction.Right)
      {
        return Right;
      }
      throw new ArgumentOutOfRangeException(nameof(direction));
    }

    public bool IsStartNode() => Id.EndsWith(StartToken);

    public bool IsEndNode() => Id.EndsWith(EndToken);
  }
}
