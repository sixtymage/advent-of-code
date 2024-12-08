using System.Diagnostics;

namespace Advent2024.Problem4;

public class Matrix<T>(IMatrixSource<T> source) : IMatrixSource<T>
  where T : struct
{
  private readonly T[,] _data = FromSource(source);

  public int StartRow => source.StartRow;

  public int StartCol => source.StartCol;

  public int Rows => _data.GetLength(0);

  public int Cols => _data.GetLength(1);

  public T ElementAt(int row, int col)
  {
    return _data[row, col];
  }

  public List<Segment<T>> GetSegmentsOfLength(int length)
  {
    ValidateSegmentLength(length);

    var segments = new List<Segment<T>>();

    var maxRow = Rows - length;
    var maxCol = Cols - length;

    for (var boxRow = 0; boxRow <= maxRow; boxRow++)
    {
      for (var boxCol = 0; boxCol <= maxCol; boxCol++)
      {
        var box = GetSubMatrix(boxRow, boxCol, length);

        var horizontalSegments = box.GetHorizontalSegments();
        var verticalSegments = box.GetVerticalSegments();
        var diagonalForwardSegment = box.GetDiagonalForwardSegment();
        var diagonalBackwardSegment = box.GetDiagonalBackwardSegment();

        AddUniqueSegments(segments, horizontalSegments);
        AddUniqueSegments(segments, verticalSegments);
        AddUniqueSegment(segments, diagonalForwardSegment);
        AddUniqueSegment(segments, diagonalBackwardSegment);
      }
    }

    return segments;
  }

  private static void AddUniqueSegments(List<Segment<T>> segments, List<Segment<T>> candidateSegments)
  {
    foreach (var candidateSegment in candidateSegments.Where(candidateSegment => !segments.Contains(candidateSegment)))
    {
      segments.Add(candidateSegment);
    }
  }

  private static void AddUniqueSegment(List<Segment<T>> segments, Segment<T> candidateSegment)
  {
    if (!segments.Contains(candidateSegment))
    {
      segments.Add(candidateSegment);
    }
  }

  private void ValidateSegmentLength(int length)
  {
    if (Rows % length != 0)
    {
      throw new ArgumentException("The length of the segment must evenly divide the matrix.");
    }
  }

  private List<Segment<T>> GetHorizontalSegments()
  {
    var segments = new List<Segment<T>>();
    for (var row = 0; row < Rows; row++)
    {
      var segment = new Segment<T>();
      for (var col = 0; col < Cols; col++)
      {
        var actualRow = StartRow + row;
        var actualCol = StartCol + col;
        segment.AddElement(actualRow, actualCol, _data[row, col]);
      }
      segments.Add(segment);
    }

    return segments;
  }

  private List<Segment<T>> GetVerticalSegments()
  {
    var segments = new List<Segment<T>>();
    for (var col = 0; col < Cols; col++)
    {
      var segment = new Segment<T>();
      for (var row = 0; row < Rows; row++)
      {
        var actualRow = StartRow + row;
        var actualCol = StartCol + col;
        segment.AddElement(actualRow, actualCol, _data[row, col]);
      }
      segments.Add(segment);
    }

    return segments;
  }

  private Segment<T> GetDiagonalForwardSegment()
  {
    Debug.Assert(Cols == Rows);
    var length = Rows;

    var segment = new Segment<T>();
    for (var i = 0; i < length; i++)
    {
      var actualRow = StartRow + i;
      var actualCol = StartCol + i;
      segment.AddElement(actualRow, actualCol, _data[i, i]);
    }

    return segment;
  }

  private Segment<T> GetDiagonalBackwardSegment()
  {
    Debug.Assert(Cols == Rows);
    var length = Rows;

    var segment = new Segment<T>();
    for (var i = 0; i < length; i++)
    {
      var row = length - 1 - i;

      var actualRow = StartRow + row;
      var actualCol = StartCol + i;
      segment.AddElement(actualRow, actualCol, _data[row, i]);
    }

    return segment;
  }

  private Matrix<T> GetSubMatrix(int row, int col, int length)
  {
    return new Matrix<T>(new SubMatrixSource<T>(row, col, length, this));
  }

  private static T[,] FromSource(IMatrixSource<T> source)
  {
    var rows = source.Rows;
    var cols = source.Cols;
    var data = new T[rows, cols];
    for (var row = 0; row < rows; row++)
    {
      for (var col = 0; col < cols; col++)
      {
        data[row, col] = source.ElementAt(row, col);
      }
    }

    return data;
  }
}
