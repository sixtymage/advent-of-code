namespace Advent2023.Problem3
{
  internal class PartMatrix
  {
    public int Rows { get; }

    public int Cols { get; }

    private readonly char[,] _matrix;

    private static readonly Func<int, int, char> DefaultElementSource = new Func<int, int, char>((_, _) => '.');

    public PartMatrix(int rows, int cols, Func<int, int, char>? elementSource = null)
    {
      Rows = rows >= 1 ? rows : throw new ArgumentOutOfRangeException(nameof(rows));
      Cols = cols >= 1 ? cols : throw new ArgumentOutOfRangeException(nameof(cols));
      _matrix = new char[rows, cols];

      SetElements(elementSource ?? DefaultElementSource);
    }

    public char this[int row, int col]
    {
      get { return _matrix[row, col]; }
      set { _matrix[row, col] = value; }
    }

    public static PartMatrix FromLines(string[] lines)
    {
      var rows = lines.Length >= 1 ? lines.Length : throw new ArgumentException("Need one or more lines", nameof(lines));
      var cols = lines[0].Length >= 1 ? lines[0].Length : throw new ArgumentException("First line can't be empty", nameof(lines));

      var matrix = new PartMatrix(rows, cols, (i, j) => lines[i][j]);
      return matrix;
    }

    public List<PartNumberLocation> GetPartNumberLocations()
    {
      var locations = new List<PartNumberLocation>();

      PartNumberLocation? location = null;

      for (var i = 0; i < Rows; i++)
      {
        if (location != null)
        {
          locations.Add(location);
          location = null;
        }

        for (var j = 0; j < Cols; j++)
        {
          if (char.IsDigit(_matrix[i, j]))
          {
            if (location == null)
            {
              location = new PartNumberLocation()
              {
                Row = i,
                StartCol = j,
                EndCol = j,
              };
            }
            else
            {
              location.EndCol = j;
            }
          }
          else
          {
            if (location != null)
            {
              locations.Add(location);
              location = null;
            }
          }
        }
      }

      if (location != null)
      {
        locations.Add(location);
        location = null;
      }

      return locations;
    }

    public int GetPartNumber(PartNumberLocation location)
    {
      int value = 0;
      for (var i = location.StartCol; i <= location.EndCol; i++)
      {
        var c = _matrix[location.Row, i];
        value = value * 10 + int.Parse($"{c}");
      }
      return value;
    }

    public static bool IsPartNumber(List<SymbolLocation> surroundingSymbols)
    {
      return surroundingSymbols.Any(x => x.Symbol != '.' && !char.IsDigit(x.Symbol));
    }

    public List<SymbolLocation> GetSurroundingSymbols(PartNumberLocation location)
    {
      var symbols = new List<SymbolLocation>();

      symbols.AddRange(FindSymbolsAbove(location));
      symbols.AddRange(FindSymbolsBelow(location));
      symbols.AddRange(FindSymbolsBeside(location));
      return symbols;
    }

    private List<SymbolLocation> FindSymbolsAbove(PartNumberLocation location)
    {
      if (location.Row == 0)
      {
        return [];
      }
      return FindSymbolsInSegment(location.Row - 1, location.StartCol, location.EndCol);
    }

    private List<SymbolLocation> FindSymbolsBelow(PartNumberLocation location)
    {
      if (location.Row == Rows - 1)
      {
        return [];
      }
      return FindSymbolsInSegment(location.Row + 1, location.StartCol, location.EndCol);
    }

    private List<SymbolLocation> FindSymbolsInSegment(int row, int startCol, int endCol)
    {
      startCol = SafeColumnToLeft(startCol);
      endCol = SafeColumnToRight(endCol);

      var symbols = new List<SymbolLocation>();
      for (var col = startCol; col <= endCol; col++)
      {
        symbols.Add(new SymbolLocation(row, col, _matrix[row, col]));
      }

      return symbols;
    }

    private static int SafeColumnToLeft(int col)
    {
      return col == 0 ? 0 : col - 1;
    }

    private int SafeColumnToRight(int col)
    {
      return col == Cols - 1 ? Cols - 1 : col + 1;
    }

    private List<SymbolLocation> FindSymbolsBeside(PartNumberLocation location)
    {
      var leftCol = SafeColumnToLeft(location.StartCol);
      var rightCol = SafeColumnToRight(location.EndCol);
      var symbols = new List<SymbolLocation>
      {
        new SymbolLocation(location.Row, leftCol, _matrix[location.Row, leftCol]),
        new SymbolLocation(location.Row, rightCol, _matrix[location.Row, rightCol])
      };
      return symbols;
    }

    private void SetElements(Func<int, int, char> elementSource)
    {
      for (var i = 0; i < Rows; i++)
      {
        for (var j = 0; j < Cols; j++)
        {
          this[i, j] = elementSource(i, j);
        }
      }
    }
  }
}
