using System.Collections.ObjectModel;

namespace Advent2024.Problem6;

public class TraversedPath(Location initialLocation, Direction initialDirection)
{
  private readonly List<TraversedLocation> _path =
  [
    new(initialLocation, initialDirection)
  ];

  public IReadOnlyList<TraversedLocation> TraversedLocations => new ReadOnlyCollection<TraversedLocation>(_path);

  public void AddLocation(Location location, Direction direction)
  {
    _path.Add(new TraversedLocation(location, direction));
  }
}
