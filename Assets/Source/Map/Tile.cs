using System;

public class Tile
{
    private readonly byte _content;

    public Tile(Biom biom, TileFlags tileFlag)
    {
        const int BiomShiftFactor = 4;
        const int TileFlagShiftFactor = 0;
        const int MaxValue = 0xF;

        if ((int)biom > MaxValue || (int)biom < 0)
            throw new ArgumentOutOfRangeException(nameof(biom));

        if ((int)tileFlag > MaxValue || (int)tileFlag < 0)
            throw new ArgumentOutOfRangeException(nameof(biom));

        _content = (byte)(
            (byte)(((int)biom & MaxValue) << BiomShiftFactor) +
            (byte)(((int)tileFlag & MaxValue) << TileFlagShiftFactor)
            );
    }

    public byte Content => _content;
}