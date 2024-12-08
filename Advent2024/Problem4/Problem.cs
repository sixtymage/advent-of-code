namespace Advent2024.Problem4;

public class Problem(string filename = @"data\problem4-input.txt") : IProblem
{
  private const string Word = "XMAS";

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(filename);

    var matrix = CreateMatrix(lines);
    var numWords = CalcWords(matrix, Word);

    Console.WriteLine($"Total number of {Word} words is: {numWords}");
  }

  private static int CalcWords(Matrix<char> matrix, string word)
  {
    return 0;
  }

  private static Matrix<char> CreateMatrix(string[] lines)
  {
    var source = new LinesDataSource(lines);
    return new Matrix<char>(source);
  }
}
