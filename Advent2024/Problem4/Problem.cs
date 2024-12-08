namespace Advent2024.Problem4;

public class Problem(string filename = @"data\problem4-input.txt") : IProblem
{
  private const string SearchWord = "XMAS";

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var matrix = CreateMatrix(lines);
    var numWords = CalcWords(matrix, SearchWord);

    Console.WriteLine($"Total number of {SearchWord} words is: {numWords}");
  }

  private static int CalcWords(Matrix<char> matrix, string searchWord)
  {
    return matrix
      .GetSegmentsOfLength(searchWord.Length)
      .Sum(x => CheckSegment(x, searchWord));
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
