namespace Advent2024.Problem4;

public class Segment<T> : IEquatable<Segment<T>> where T : struct
{
  private readonly List<(int, int)> _points = [];

  public List<T> Value { get; } = [];

  public void AddElement(int row, int col, T value)
  {
    _points.Add((row, col));
    Value.Add(value);
  }

  public bool Equals(Segment<T>? other)
  {
    if (other is null)
    {
      return false;
    }

    return ReferenceEquals(this, other) || ArePointsTheSame(_points, other._points);
  }

  private static bool ArePointsTheSame(List<(int, int)> points, List<(int, int)> otherPoints)
  {
    if (points.Count != otherPoints.Count)
    {
      return false;
    }

    for (var i = 0; i < points.Count; i++)
    {
      if (points[i].Item1 != otherPoints[i].Item1 || points[i].Item2 != otherPoints[i].Item2)
      {
        return false;
      }
    }

    return true;
  }

  public override bool Equals(object? obj)
  {
    if (obj is null)
    {
      return false;
    }

    if (ReferenceEquals(this, obj))
    {
      return true;
    }

    return obj.GetType() == GetType() && Equals((Segment<T>)obj);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(_points);
  }
}
