namespace Advent2024.Problem3;

public class StateData
{
  private const string DoInstruction = "do()";
  private const string DontInstruction = "don't()";

  public State State { get;  }

  private readonly List<char> _mulInstruction;
  private readonly List<char> _nonMulInstruction;
  private readonly List<char> _firstNumberDigits;
  private readonly List<char> _secondNumberDigits;

  public StateData()
  {
    State = State.ExpectingM;
    _mulInstruction = [];
    _nonMulInstruction = [];
    _firstNumberDigits = [];
    _secondNumberDigits = [];
  }

  public StateData(State state, char? mulInstructionChar, char? nonMulInstructionChar)
  {
    State = state;
    _mulInstruction = mulInstructionChar is not null ? [mulInstructionChar.Value] : [];
    _nonMulInstruction = nonMulInstructionChar is not null ? [nonMulInstructionChar.Value] : [];
    _firstNumberDigits = [];
    _secondNumberDigits = [];
  }

  private StateData(
    State state,
    List<char>? mulInstruction,
    List<char>? nonMulInstruction,
    List<char>? firstNumberDigits,
    List<char>? secondNumberDigits,
    char? mulInstructionChar,
    char? nonMulInstructionChar,
    char? firstNumberDigitsChar,
    char? secondNumberDigitsChar)
  {
    State = state;
    _mulInstruction = ToList(mulInstruction, mulInstructionChar);
    _nonMulInstruction = ToList(nonMulInstruction, nonMulInstructionChar);
    _firstNumberDigits = ToList(firstNumberDigits, firstNumberDigitsChar);
    _secondNumberDigits = ToList(secondNumberDigits, secondNumberDigitsChar);
  }

  private static List<char> ToList(List<char>? list, char? character)
  {
    return character is not null
      ? [..list ?? [], character.Value]
      : [..list ?? []];
  }

  public Instruction RenderInstruction()
  {
    var left = ParseNumber(_firstNumberDigits);
    var right = ParseNumber(_secondNumberDigits);

    return new Instruction
    {
      Left = left,
      Right = right
    };
  }

  public int GetFirstNumberDigitsCount()
  {
    return _firstNumberDigits.Count;
  }

  public int GetSecondNumberDigitsCount()
  {
    return _secondNumberDigits.Count;
  }

  public bool IsDoInstruction()
  {
    var lastChars = string.Join("", _nonMulInstruction.TakeLast(4));
    return lastChars == DoInstruction;
  }

  public bool IsDontInstruction()
  {
    var lastChars = string.Join("", _nonMulInstruction.TakeLast(7));
    return lastChars == DontInstruction;
  }

  public StateData AddMulInstructionCharacter(char character, State nextState)
  {
    return new StateData(nextState, _mulInstruction, null, _firstNumberDigits, _secondNumberDigits, character, null, null, null);
  }

  public StateData AddFirstNumberDigit(char character, State nextState)
  {
    return new StateData(nextState, _mulInstruction, null, _firstNumberDigits, _secondNumberDigits, character, null, character, null);
  }

  public StateData AddSecondNumberDigit(char character, State nextState)
  {
    return new StateData(nextState, _mulInstruction, null, _firstNumberDigits, _secondNumberDigits, character, null, null, character);
  }

  public StateData AddNonMulInstructionChar(char character)
  {
    return new StateData(State, null, _nonMulInstruction, null, null, null, character, null, null);
  }

  private static int ParseNumber(List<char> digits)
  {
    var value = string.Join("", digits);
    if (int.TryParse(value, out var result))
    {
      return result;
    }
    throw new InvalidDataException($"Could not parse \"{value}\" as a number");
  }
}
