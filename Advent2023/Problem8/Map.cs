using System.Diagnostics;

namespace Advent2023.Problem8
{
  internal class Map
  {
    private readonly Dictionary<string, MapNode> _mapNodes = [];

    public static Map FromDescription(IEnumerable<string> lines)
    {
      var map = new Map();
      foreach (var line in lines)
      {
        var mapNode = RecoverMapNode(line);
        map.AddMapNode(mapNode);
      }
      return map;
    }

    public void AddMapNode(MapNode mapNode)
    {
      _mapNodes.Add(mapNode.Id, mapNode);
    }

    public MapNode FindMapNode(string id)
    {
      return _mapNodes[id];
    }

    private static MapNode RecoverMapNode(string line)
    {
      var splitEquals = line.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      if (splitEquals.Length != 2)
      {
        throw new InvalidDataException("Unexpected input, map node should be delimited with an '='");
      }
      var id = splitEquals[0];

      var splitComma = splitEquals[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      if (splitComma.Length != 2)
      {
        throw new InvalidDataException("Unexpected input, map node directions should be delimited with a ','");
      }

      var left = splitComma[0].Trim('(');
      var right = splitComma[1].Trim(')');

      return new MapNode(id, left, right);
    }
  }
}
