namespace Advent2024.Problem3;

public enum State
{
  ExpectingM,
  ExpectingU,
  ExpectingL,
  ExpectingOpenParenthesis,
  ExpectingFirstNumberDigit,
  ExpectingFirstNumberDigitOrComma,
  ExpectingComma,
  ExpectingSecondNumberDigit,
  ExpectingSecondNumberDigitOrCloseParenthesis,
  ExpectingCloseParenthesis,
  InstructionFound,
}
