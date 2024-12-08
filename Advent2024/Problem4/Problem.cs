namespace Advent2024.Problem4;

public class Problem(string filename = @"data\problem4-input.txt") : IProblem
{
  private const string SearchWord1 = "XMAS";
  private const string SearchWord2 = "MAS";

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var matrix = CreateMatrix(lines);

    //SolvePart1(matrix);
    SolvePart2(matrix);
  }

  private static void SolvePart2(Matrix<char> matrix)
  {
    // create all the unique boxes of size 3
    var boxes = matrix.GetAllBoxes(SearchWord2.Length);

    // get the \ and / diagonals for each box as segments
    var total = 0;
    foreach (var box in boxes)
    {
      var count = CheckSegment(box.GetDiagonalForwardSegment(), SearchWord2) +
                  CheckSegment(box.GetDiagonalBackwardSegment(), SearchWord2);
      if (count == 2)
      {
        total++;
      }
    }

    Console.WriteLine($"Total number of {SearchWord1} words is: {total}");
  }

  private static void SolvePart1(Matrix<char> matrix)
  {
    var numWords = matrix
      .GetSegmentsOfLength(SearchWord1.Length)
      .Sum(x => CheckSegment(x, SearchWord1));
    Console.WriteLine($"Total number of {SearchWord1} words is: {numWords}");
  }

  private static int CheckSegment(Segment<char> segment, string searchWord)
  {
    var value = segment.Value.ToArray();
    var word = string.Join("", value);
    var count = word == searchWord ? 1 : 0;

    var reversed = value.Reverse();
    var reversedWord = string.Join("", reversed);
    return reversedWord == searchWord ? count + 1 : count;
  }

  private static Matrix<char> CreateMatrix(string[] lines)
  {
    var source = new LinesSource(lines);
    return new Matrix<char>(source);
  }
}
