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
      .SelectMany(CreateZones)
      .ToArray();
  }

  private IEnumerable<Zone> CreateZones(IGrouping<char, CropLocation> group)
  {
    var cropLocations = group
      .Select(x => x)
      .ToList();

    var zones = new List<Zone>();
    while (cropLocations.Count > 0)
    {
      HashSet<Location> connectedLocations = [];
      FindCropLocations(cropLocations[0], connectedLocations);

      zones.Add(new Zone(group.Key, connectedLocations.ToArray()));
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

  private bool IsOnMap(Location location)
  {
    return location.Row >= 0 && location.Row < matrix.Rows && location.Col >= 0 && location.Col < matrix.Cols;
  }
}
