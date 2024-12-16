namespace Advent2024.Problem8;

public class Map(Matrix<char> matrix)
{
  private const char EmptyLocation = '.';

  public bool IsOnMap(Location location)
  {
    return location.Row >= 0 && location.Row < matrix.Rows && location.Col >= 0 && location.Col < matrix.Cols;
  }

  public Antenna[] GetAntennae()
  {
    var antennae = new List<Antenna>();
    for (var row = 0; row < matrix.Rows; row++)
    {
      for (var col = 0; col < matrix.Cols; col++)
      {
        var mapObject = matrix.ElementAt(row, col);
        if (mapObject == EmptyLocation)
        {
          continue;
        }

        var location = new Location
        {
          Row = row,
          Col = col
        };
        antennae.Add(new Antenna(location, mapObject));
      }
    }

    return antennae.ToArray();
  }
}
