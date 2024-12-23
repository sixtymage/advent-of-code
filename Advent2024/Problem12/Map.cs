namespace Advent2024.Problem12;

public class Map(Matrix<char> matrix)
{
  private readonly List<CropLocation> _cropLocations = LoadCrops(matrix);

  private static List<CropLocation> LoadCrops(Matrix<char> matrix)
  {
    var crops = new List<CropLocation>();
    for (var row = 0; row < matrix.Rows; row++)
    {
      for (var col = 0; col < matrix.Cols; col++)
      {
        crops.Add(new CropLocation(matrix[row, col], row, col));
      }
    }

    return crops;
  }

  public Zone[] GetZones()
  {
    return _cropLocations
      .GroupBy(cl => cl.Crop)
      .SelectMany(FindZones)
      .ToArray();
  }

  public int CalcNumSides(Zone zone, Side side)
  {
    return side switch
    {
      Side.Top or Side.Bottom => CalcNumHorizontalSides(zone, side),
      Side.Left or Side.Right => CalcNumVerticalSides(zone, side),
      _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
    };
  }

  private int CalcNumHorizontalSides(Zone zone, Side side)
  {
    var numSides = 0;
    for (var row = 0; row < matrix.Rows; row++)
    {
      var localRow = row;

      var candidateLocations = zone.Locations
        .Where(l => l.Row == localRow && !IsSameCrop(l, side))
        .ToList();

      var count = CalcNumSeparateGroups(candidateLocations, (l1, l2) => Math.Abs(l2.Col - l1.Col) == 1);
      numSides += count;
    }

    return numSides;
  }

  private int CalcNumVerticalSides(Zone zone, Side side)
  {
    var numSides = 0;
    for (var col = 0; col < matrix.Cols; col++)
    {
      var localCol = col;

      var candidateLocations = zone.Locations
        .Where(l => l.Col == localCol && !IsSameCrop(l, side))
        .ToList();

      var count = CalcNumSeparateGroups(candidateLocations, (l1, l2) => Math.Abs(l2.Row - l1.Row) == 1);
      numSides += count;
    }

    return numSides;
  }

  private static int CalcNumSeparateGroups(List<Location> locations, Func<Location, Location, bool> isSameGroup)
  {
    var count = 0;
    while (locations.Count > 0)
    {
      var theseLocations = new List<Location>();
      var thisLocation = locations[0];
      locations.RemoveAt(0);
      theseLocations.Add(thisLocation);

      while (true)
      {
        var connectedLocation = locations
          .FirstOrDefault(x => theseLocations.Any(tl => isSameGroup(tl, x)));

        if (connectedLocation == null)
        {
          break;
        }

        locations.Remove(connectedLocation);
        theseLocations.Add(connectedLocation);
      }

      count++;
    }

    return count;
  }

  private IEnumerable<Zone> FindZones(IGrouping<char, CropLocation> group)
  {
    var cropLocations = group
      .Select(x => x)
      .ToList();

    return FindSeparateZones(group.Key, cropLocations);
  }

  private List<Zone> FindSeparateZones(char crop, List<CropLocation> cropLocations)
  {
    var zones = new List<Zone>();
    while (cropLocations.Count > 0)
    {
      HashSet<Location> connectedLocations = [];
      FindCropLocations(cropLocations[0], connectedLocations);

      zones.Add(new Zone(crop, connectedLocations.ToArray()));
      cropLocations.RemoveAll(connectedLocations.Contains);
    }

    return zones;
  }

  private void FindCropLocations(CropLocation cropLocation, HashSet<Location> locations)
  {
    locations.Add(cropLocation);
    var newCropLocations = FindNearbyNewSameCropLocations(cropLocation, locations);

    foreach (var newCropLocation in newCropLocations)
    {
      FindCropLocations(newCropLocation, locations);
    }
  }

  private List<CropLocation> FindNearbyNewSameCropLocations(CropLocation cropLocation, HashSet<Location> existingLocations)
  {
    List<CropLocation> newLocations = [];

    AddPotentialLocation(cropLocation, -1, 0, newLocations, existingLocations);
    AddPotentialLocation(cropLocation, 0, +1, newLocations, existingLocations);
    AddPotentialLocation(cropLocation, +1, 0, newLocations, existingLocations);
    AddPotentialLocation(cropLocation, 0, -1, newLocations, existingLocations);

    return newLocations;
  }

  private void AddPotentialLocation(
    CropLocation location,
    int rowOffset,
    int colOffset,
    List<CropLocation> newLocations,
    HashSet<Location> existingLocations)
  {
    var candidateLocation = new CropLocation(location.Crop, location.Row + rowOffset, location.Col + colOffset);
    if (IsOnMap(candidateLocation) && IsSameCrop(location, candidateLocation) && !existingLocations.Contains(candidateLocation))
    {
      newLocations.Add(candidateLocation);
    }
  }

  private bool IsSameCrop(CropLocation cropLocation, Location candidateLocation)
  {
    return cropLocation.Crop == matrix[candidateLocation.Row, candidateLocation.Col];
  }

  private bool IsSameCrop(Location location, Side side)
  {
    var compareLocation = side switch
    {
      Side.Top => location with { Row = location.Row - 1 },
      Side.Right => location with { Col = location.Col + 1 },
      Side.Bottom => location with { Row = location.Row + 1 },
      Side.Left => location with { Col = location.Col - 1 },
      _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
    };

    if (!IsOnMap(compareLocation))
    {
      return false;
    }

    return matrix[location.Row, location.Col] == matrix[compareLocation.Row, compareLocation.Col];
  }

  private bool IsOnMap(Location location)
  {
    return location.Row >= 0 && location.Row < matrix.Rows && location.Col >= 0 && location.Col < matrix.Cols;
  }
}
