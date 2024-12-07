namespace Advent2024.Problem3;

public class StateMachineInstructionExtractor : IInstructionExtractor
{
  private const char Comma = ',';
  private const char CloseParenthesis = ')';
  private const char StartChar = 'm';

  public List<Instruction> ExtractInstructions(string input)
  {
    List<Instruction> instructions = [];
    var instructionsEnabled = true;

    var stateData = new StateData();
    foreach (var character in input)
    {
      stateData = Transition(stateData, character);
      HandleStateActions(stateData, instructions, ref instructionsEnabled);
    }

    return instructions;
  }

  private static void HandleStateActions(StateData stateData, List<Instruction> instructions, ref bool enableInstructions)
  {
    switch (stateData.State)
    {
      case State.InstructionFound:
        HandleInstructionFound(stateData, instructions, enableInstructions);
        break;

      case State.ExpectingM:
        CheckForActivationInstructions(stateData, ref enableInstructions);
        break;

      case State.ExpectingU:
      case State.ExpectingL:
      case State.ExpectingOpenParenthesis:
      case State.ExpectingFirstNumberDigit:
      case State.ExpectingFirstNumberDigitOrComma:
      case State.ExpectingComma:
      case State.ExpectingSecondNumberDigit:
      case State.ExpectingSecondNumberDigitOrCloseParenthesis:
      case State.ExpectingCloseParenthesis:
      default:
        break;
    }
  }

  private static void HandleInstructionFound(StateData stateData, List<Instruction> instructions, bool instructionsEnabled)
  {
    if (instructionsEnabled)
    {
      instructions.Add(stateData.RenderInstruction());
    }
  }

  private static void CheckForActivationInstructions(StateData stateData, ref bool instructionsEnabled)
  {
    if (stateData.IsDoInstruction())
    {
      instructionsEnabled = true;
    }

    if (stateData.IsDontInstruction())
    {
      instructionsEnabled = false;
    }
  }

  private static StateData Transition(StateData stateData, char character)
  {
    return stateData.State switch
    {
      State.ExpectingM => HandleExpectingM(stateData, character),
      State.ExpectingU => HandleExpectingU(stateData, character),
      State.ExpectingL => HandleExpectingL(stateData, character),
      State.ExpectingOpenParenthesis => HandleExpectingOpenParenthesis(stateData, character),
      State.ExpectingFirstNumberDigit => HandleExpectingFirstNumberDigit(stateData, character),
      State.ExpectingFirstNumberDigitOrComma => HandleExpectingFirstNumberDigitOrComma(stateData, character),
      State.ExpectingComma => HandleExpectingComma(stateData, character),
      State.ExpectingSecondNumberDigit => HandleExpectingSecondNumberDigit(stateData, character),
      State.ExpectingSecondNumberDigitOrCloseParenthesis => HandleExpectingSecondNumberDigitOrCloseParenthesis(stateData, character),
      State.ExpectingCloseParenthesis => HandleExpectingCloseParenthesis(stateData, character),
      State.InstructionFound => HandleInstructionFound(character),
      _ => throw new InvalidOperationException($"Unexpected state: {stateData.State}")
    };
  }

  private static StateData HandleExpectingM(StateData stateData, char character)
  {
    return character == StartChar
      ? HandleExpectedInstructionChar(stateData, character, 'm', State.ExpectingU)
      : HandleNonInstructionChar(stateData, character);
  }

  private static StateData HandleExpectingU(StateData stateData, char character)
  {
    return HandleExpectedInstructionChar(stateData, character, 'u', State.ExpectingL);
  }

  private static StateData HandleExpectingL(StateData stateData, char character)
  {
    return HandleExpectedInstructionChar(stateData, character, 'l', State.ExpectingOpenParenthesis);
  }

  private static StateData HandleExpectingOpenParenthesis(StateData stateData, char character)
  {
    return HandleExpectedInstructionChar(stateData, character, '(', State.ExpectingFirstNumberDigit);
  }

  private static StateData HandleExpectingFirstNumberDigit(StateData stateData, char character)
  {
    return !char.IsDigit(character)
      ? InitialStateData(character)
      : stateData.AddFirstNumberDigit(character, State.ExpectingFirstNumberDigitOrComma);
  }

  private static StateData HandleExpectingFirstNumberDigitOrComma(StateData stateData, char character)
  {
    if (!char.IsDigit(character))
    {
      return HandleExpectingComma(stateData, character);
    }

    var nextState = stateData.GetFirstNumberDigitsCount() == 1
      ? State.ExpectingFirstNumberDigitOrComma
      : State.ExpectingComma;

    return stateData.AddFirstNumberDigit(character, nextState);
  }

  private static StateData HandleExpectingComma(StateData stateData, char character)
  {
    return character == Comma
      ? stateData.AddMulInstructionCharacter(Comma, State.ExpectingSecondNumberDigit)
      : InitialStateData(character);
  }

  private static StateData HandleExpectingSecondNumberDigit(StateData stateData, char character)
  {
    return !char.IsDigit(character)
      ? InitialStateData(character)
      : stateData.AddSecondNumberDigit(character, State.ExpectingSecondNumberDigitOrCloseParenthesis);
  }

  private static StateData HandleExpectingSecondNumberDigitOrCloseParenthesis(StateData stateData, char character)
  {
    if (!char.IsDigit(character))
    {
      return HandleExpectingCloseParenthesis(stateData, character);
    }

    var nextState = stateData.GetSecondNumberDigitsCount() == 1
      ? State.ExpectingSecondNumberDigitOrCloseParenthesis
      : State.ExpectingCloseParenthesis;

    return stateData.AddSecondNumberDigit(character, nextState);
  }

  private static StateData HandleExpectingCloseParenthesis(StateData stateData, char character)
  {
    return character == CloseParenthesis
      ? stateData.AddMulInstructionCharacter(CloseParenthesis, State.InstructionFound)
      : InitialStateData(character);
  }

  private static StateData HandleInstructionFound(char character)
  {
    return InitialStateData(character);
  }

  private static StateData HandleExpectedInstructionChar(
    StateData stateData,
    char character,
    char expectedCharacter,
    State nextState)
  {
    return character != expectedCharacter
      ? InitialStateData(character)
      : stateData.AddMulInstructionCharacter(character, nextState);
  }

  private static StateData HandleNonInstructionChar(StateData stateData, char character)
  {
    return stateData.AddNonMulInstructionChar(character);
  }

  private static StateData InitialStateData(char character)
  {
    return character == StartChar
      ? new StateData(State.ExpectingU, StartChar, null)
      : new StateData(State.ExpectingM, null, character);
  }
}
