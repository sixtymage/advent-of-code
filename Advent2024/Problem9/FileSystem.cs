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
    for (var i = 0; i < _blocks.Count; i++)
    {
      if (!_blocks[i].IsEmpty)
      {
        checksum += i * _blocks[i].Id!.Value;
      }
    }

    return checksum;
  }

  public void CompactWholeFiles()
  {
    var searchIndex = _blocks.Count - 1;

    while (true)
    {
      var (id, startIndex, numBlocks) = FindLastFile(searchIndex);

      var targetIndex = IndexOfContiguousFreeBlocks(startIndex, numBlocks);

      if (targetIndex != -1)
      {
        MoveFile(startIndex, numBlocks, targetIndex);
      }

      if (id == 0)
      {
        break;
      }

      searchIndex = startIndex - 1;
    }
  }

  private (int, int, int) FindLastFile(int searchIndex)
  {
    while (_blocks[searchIndex].IsEmpty)
    {
      searchIndex--;
    }

    var id = _blocks[searchIndex].Id!.Value;
    var numBlocks = 0;
    while (true)
    {
      searchIndex--;
      numBlocks++;

      if (searchIndex < 0 || _blocks[searchIndex].IsEmpty || _blocks[searchIndex].Id!.Value != id)
      {
        return (id, searchIndex+1, numBlocks);
      }
    }
  }

  private int IndexOfContiguousFreeBlocks(int beforeIndex, int numFreeBlocksNeeded)
  {
    var numFreeBlocks = 0;
    for (var i = 0; i < beforeIndex; i++)
    {
      if (_blocks[i].IsEmpty)
      {
        numFreeBlocks++;
        if (numFreeBlocks == numFreeBlocksNeeded)
        {
          return i - numFreeBlocks + 1;
        }
      }
      else
      {
        numFreeBlocks = 0;
      }
    }

    return -1;
  }

  private void MoveFile(int startIndex, int numBlocks, int targetIndex)
  {
    for (var i = 0; i < numBlocks; i++)
    {
      Swap(startIndex + i, targetIndex + i);
    }
  }
}
