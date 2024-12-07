using System.Text;

namespace Advent2024.Problem3;

public class StateData
{
  public State State { get; init; }

  private readonly List<char> _instruction;
  private readonly List<char> _firstNumberDigits;
  private readonly List<char> _secondNumberDigits;

  public StateData(State state)
  {
    State = state;
    _instruction = [];
    _firstNumberDigits = [];
    _secondNumberDigits = [];
  }

  public StateData(State state, char initialChar)
  {
    State = state;
    _instruction = [initialChar];
    _firstNumberDigits = [];
    _secondNumberDigits = [];
  }

  private StateData(StateData from, State state)
  {
    State = state;
    _instruction = [..from._instruction];
    _firstNumberDigits = [..from._firstNumberDigits];
    _secondNumberDigits = [..from._secondNumberDigits];
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

  public StateData AddExpectedCharacter(char character, State nextState)
  {
    var stateData = new StateData(this, nextState);
    stateData._instruction.Add(character);
    return stateData;
  }

  public StateData AddFirstNumberDigit(char character, State nextState)
  {
    var stateData = new StateData(this, nextState);
    stateData._instruction.Add(character);
    stateData._firstNumberDigits.Add(character);

    return stateData;
  }

  public StateData AddSecondNumberDigit(char character, State nextState)
  {
    var stateData = new StateData(this, nextState);
    stateData._instruction.Add(character);
    stateData._secondNumberDigits.Add(character);

    return stateData;
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
