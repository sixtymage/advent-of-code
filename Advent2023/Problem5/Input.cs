namespace Advent2023.Problem5
{
  internal class Input(List<long> seeds, List<RangedMap> maps)
  {
    public List<long> Seeds { get; } = seeds;

    public List<RangedMap> Maps { get; } = maps;
  }
}
