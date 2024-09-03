using System.Collections.Generic;

public class Map
{
    private const int Size = 4000;

    private readonly byte[] _mapContent = new byte[Size * Size];

    public Map()
    {
        Tile tile;

        for (int i = 0; i < _mapContent.Length; i++)
        {
            tile = new Tile(Biom.Forest, TileFlags.None);
            _mapContent[i] = tile.Content;
        }
    }

    public IEnumerable<byte> MapContent => _mapContent;
}