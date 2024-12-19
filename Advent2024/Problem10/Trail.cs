using System.Collections.ObjectModel;

namespace Advent2024.Problem10;

public class Trail(Location initialLocation)
{
  private readonly List<Location> _path =
  [
    initialLocation,
  ];

  public Trail(Trail trail) : this(trail._path[0])
  {
    _path.AddRange(trail._path.Skip(1));
  }

  public IReadOnlyList<Location> Locations => new ReadOnlyCollection<Location>(_path);

  public void AddLocation(Location location)
  {
    _path.Add(location);
  }
}
