namespace Advent2023.Problem5
{
  internal class RangedMap(string name)
  {
    private List<(long Destination, long Source, long Length)> _ranges = new();

    public string Name { get; } = name;

    public void AddRange(long destination, long source, long length)
    {
      _ranges.Add((destination, source, length));
    }

    public long Lookup(long source)
    {
      foreach (var range in _ranges)
      {
        if (IsInRange(range, source))
        {
          return CalcDestination(range, source);
        }
      }
      return source;
    }

    private static bool IsInRange((long Destination, long Source, long Length) range, long source)
    {
      if (source >= range.Source && source < range.Source + range.Length)
      {
        return true;
      }
      return false;
    }

    private long CalcDestination((long Destination, long Source, long Length) range, long source)
    {
      var offset = source - range.Source;
      return range.Destination + offset;
    }
  }
}
