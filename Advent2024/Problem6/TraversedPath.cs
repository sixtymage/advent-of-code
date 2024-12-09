using System.Collections.ObjectModel;

namespace Advent2024.Problem6;

public class TraversedPath(Location initialLocation)
{
  private readonly List<Location> _path = [new(initialLocation)];

  public IReadOnlyList<Location> Locations => new ReadOnlyCollection<Location>(_path);

  public void AddLocation(Location location)
  {
    _path.Add(new Location(location));
  }
}
