using Advent2023;
using Advent2023.Problem2;

namespace Problem2;

public class Problem : IProblem
{
  private const int MaxRed = 12;
  private const int MaxGreen = 13;
  private const int MaxBlue = 14;
  private const string GamePrefix = "Game ";

  private string _filename;

  public Problem(string filename = @"data\problem2-input.txt")
  {
    _filename = filename;
  }

  public async Task SolveAsync()
  {
    var lines = await File.ReadAllLinesAsync(_filename);

    int sumGameId = 0;
    int sumPower = 0;
    foreach (var line in lines)
    {
      (string game, string actions) = SplitLine(line);

      var gameId = RecoverGameId(game);
      var handfuls = RecoverHandfuls(actions);

      if (IsGamePossible(handfuls))
      {
        sumGameId += gameId;
      }

      var minimum = FindMinimumForLegalGame(handfuls);
      var power = CalculatePower(minimum);
      sumPower += power;
    }
    Console.WriteLine($"Sum of ids of possible games: {sumGameId}");
    Console.WriteLine($"Sum of power of all games: {sumPower}");
  }

  private static (string first, string second) SplitLine(string line)
  {
    var split = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (split.Length != 2)
    {
      throw new InvalidDataException($"Colon missing, invalid line: {line}");
    }
    return (split[0], split[1]);
  }

  private static int RecoverGameId(ReadOnlySpan<char> line)
  {
    var prefixLength = GamePrefix.Length;
    var gameIdSlice = line.Slice(prefixLength, line.Length - prefixLength);
    return int.Parse(gameIdSlice);
  }

  private static List<Handful> RecoverHandfuls(string actions)
  {
    var handfulDescriptions = actions.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    var handfuls = new List<Handful>();
    foreach (var handfulDescription in handfulDescriptions)
    {
      var boxSelections = RecoverBoxSelections(handfulDescription);
      var handful = RecoverHandful(boxSelections);
      handfuls.Add(handful);
    }
    return handfuls;
  }

  private static List<BoxSelection> RecoverBoxSelections(string handfulDescription)
  {
    var boxDescriptions = handfulDescription.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    var boxes = new List<BoxSelection>();
    foreach (var boxDescription in boxDescriptions)
    {
      var boxSelection = RecoverBoxSelection(boxDescription);
      boxes.Add(boxSelection);
    }
    return boxes;
  }

  private static BoxSelection RecoverBoxSelection(string boxDescription)
  {
    var split = boxDescription.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (split.Length != 2)
    {
      throw new InvalidDataException($"Unxpected format for boxes. Should be [number] [colour]: {boxDescription}");
    }

    return new BoxSelection
    {
      Count = int.Parse(split[0]),
      BoxColour = (BoxColour)Enum.Parse(typeof(BoxColour), split[1], true),
    };
  }

  private static Handful RecoverHandful(List<BoxSelection> boxSelections)
  {
    var handful= new Handful();
    foreach (var boxSelection in boxSelections)
    {
      switch (boxSelection.BoxColour)
      {
        case BoxColour.Red:
          handful.NumRed += boxSelection.Count;
          break;

        case BoxColour.Green:
          handful.NumGreen += boxSelection.Count;
          break;

        case BoxColour.Blue:
          handful.NumBlue += boxSelection.Count;
          break;
      }
    }
    return handful;
  }

  private static bool IsGamePossible(List<Handful> handfuls)
  {
    // at each stage, the  number revealed can't exceeed the maximum for the game
    foreach (var handful in handfuls)
    {
      if (!IsHandfulPossible(handful))
      {
        return false;
      }
    }
    return true;
  }

  private static bool IsHandfulPossible(Handful handful)
  {
    if (handful.NumRed > MaxRed)
    {
      return false;
    }
    if (handful.NumGreen > MaxGreen)
    {
      return false;
    }
    if (handful.NumBlue > MaxBlue)
    {
      return false;
    }
    return true;
  }

  private static Handful FindMinimumForLegalGame(List<Handful> handfuls)
  {
    var minimum = new Handful();
    foreach (var handful in handfuls)
    {
      minimum.NumRed = handful.NumRed > minimum.NumRed ? handful.NumRed : minimum.NumRed;
      minimum.NumGreen = handful.NumGreen > minimum.NumGreen ? handful.NumGreen: minimum.NumGreen;
      minimum.NumBlue = handful.NumBlue > minimum.NumBlue ? handful.NumBlue : minimum.NumBlue;
    }
    return minimum;
  }

  private int CalculatePower(Handful minimum)
  {
    return minimum.NumRed * minimum.NumGreen * minimum.NumBlue;
  }
}
