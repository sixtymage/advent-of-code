using System.Text;

namespace Advent2024.Problem9;

public class FileSystem(string input)
{
  private readonly List<Block> _blocks = ParseInput(input);

  public string Render()
  {
    var sb = new StringBuilder();
    foreach (var block in _blocks)
    {
      sb.Append(block);
    }

    return sb.ToString();
  }

  private static List<Block> ParseInput(string input)
  {
    var nextId = 0;
    var isFileNext = true;
    var blocks = new List<Block>();
    foreach (var character in input)
    {
      var numBlocks = int.Parse(character.ToString());
      var contents = isFileNext ? nextId++ : (int?)null;
      blocks.AddRange(Enumerable.Repeat(new Block(contents), numBlocks));

      isFileNext = !isFileNext;
    }

    return blocks;
  }

  public int IndexOfLastFile()
  {
    return _blocks.FindLastIndex(b => !b.IsEmpty);
  }

  public int IndexOfFirstEmptyBlock()
  {
    return _blocks.FindIndex(b => b.IsEmpty);
  }

  public void Swap(int sourceIndex, int targetIndex)
  {
    (_blocks[sourceIndex], _blocks[targetIndex]) = (_blocks[targetIndex], _blocks[sourceIndex]);
  }

  public bool IsCompacted()
  {
    var indexOfLastFile = IndexOfLastFile();
    var indexOfFirstEmptyBlock = IndexOfFirstEmptyBlock();

    return indexOfFirstEmptyBlock > indexOfLastFile;
  }

  public long CalculateChecksum()
  {
    long checksum = 0;
    var emptyIndex = IndexOfFirstEmptyBlock();
    for (var i = 0; i < emptyIndex; i++)
    {
      if (_blocks[i].IsEmpty)
      {
        throw new InvalidOperationException("Checksum cannot include empty blocks");
      }

      checksum += i * _blocks[i].Id!.Value;
    }

    return checksum;
  }
}
