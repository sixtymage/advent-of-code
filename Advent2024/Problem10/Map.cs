namespace Advent2024.Problem10;

public class Map(Matrix<int> matrix)
{
  public Location[] GetLocations(int height)
  {
    var locations = new List<Location>();

    for (var row = 0; row < matrix.Rows; row++)
    {
      for (var col = 0; col < matrix.Cols; col++)
      {
        if (matrix[row, col] == height)
        {
          locations.Add(new Location(row, col));
        }
      }
    }

    return locations.ToArray();
  }

  public bool DoesTrailExist(Location start, Location end, int numSteps)
  {
    if (Math.Abs(end.Row - start.Row) > numSteps || Math.Abs(end.Col - start.Col) > numSteps)
    {
      return false;
    }

    if (numSteps == 0 && start == end)
    {
      return true;
    }

    // get nearby locations that are one step higher than the start height
    var candidateNextLocations = GetNextLocation(start, GetHeight(start) + 1);

    // if no such locations, we're done
    if (candidateNextLocations.Length == 0)
    {
      return false;
    }

    // otherwise iterate each location and see if there is a trail of length numSteps-1 from there to the end
    foreach (var candidateNextLocation in candidateNextLocations)
    {
      if (DoesTrailExist(candidateNextLocation, end, numSteps - 1))
      {
        return true;
      }
    }

    // no route exists
    return false;
  }

  private Location[] GetNextLocation(Location location, int height)
  {
    // find all the nearby horizontal or vertical locations that have the desired height
    var locations = new List<Location>();
    AddPotentialLocation(location, -1, 0, height, locations);
    AddPotentialLocation(location, 0, +1, height, locations);
    AddPotentialLocation(location, +1, 0, height, locations);
    AddPotentialLocation(location, 0, -1, height, locations);

    return locations.ToArray();
  }

  private void AddPotentialLocation(Location location, int rowOffset, int colOffset, int height, List<Location> locations)
  {
    var newLocation = new Location(location.Row + rowOffset, location.Col + colOffset);
    if (IsOnMap(newLocation) && GetHeight(newLocation) == height)
    {
      locations.Add(newLocation);
    }
  }

  private int GetHeight(Location location)
  {
    return matrix[location.Row, location.Col];
  }

  private bool IsOnMap(Location location)
  {
    return location.Row >= 0 && location.Row < matrix.Rows && location.Col >= 0 && location.Col < matrix.Cols;
  }
}
