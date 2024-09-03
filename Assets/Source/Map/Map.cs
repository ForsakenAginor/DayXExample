using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private const int Size = 4000;

    private readonly byte[] _mapContent = new byte[Size * Size];
    private readonly float _plainsSizeFactor;
    private readonly float _forestSpawnFrequency;

    public Map(float plainsSizeFactor, float forestSpawnFrequency)
    {
        _plainsSizeFactor = plainsSizeFactor >= 0 && plainsSizeFactor <= 1 ?
            plainsSizeFactor :
            throw new ArgumentOutOfRangeException(nameof(plainsSizeFactor));

        _forestSpawnFrequency = forestSpawnFrequency > 0 ?
            forestSpawnFrequency :
            throw new ArgumentOutOfRangeException(nameof(forestSpawnFrequency));

        int index = 0;

        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                _mapContent[index] = GenerateTile(x, y);
                index++;
            }
        }
        _forestSpawnFrequency = forestSpawnFrequency;
    }

    public IEnumerable<byte> MapContent => _mapContent;

    private byte GenerateTile(int x, int y)
    {
        float perlinNoiseResult = Mathf.Clamp01(
            Mathf.PerlinNoise(
                (float)x / Size * _forestSpawnFrequency,
                (float)y / Size * _forestSpawnFrequency));

        if (perlinNoiseResult > _plainsSizeFactor)
            return Encode(Biom.Forest, TileFlags.None);
        else
            return Encode(Biom.Plains, TileFlags.None);
    }

    private byte Encode(Biom biom, TileFlags tileFlag)
    {
        const int BiomShiftFactor = 4;
        const int TileFlagShiftFactor = 0;
        const int MaxValue = 0xF;

        if ((int)biom > MaxValue || (int)biom < 0)
            throw new ArgumentOutOfRangeException(nameof(biom));

        if ((int)tileFlag > MaxValue || (int)tileFlag < 0)
            throw new ArgumentOutOfRangeException(nameof(biom));

        return (byte)(
            (byte)(((int)biom & MaxValue) << BiomShiftFactor) +
            (byte)(((int)tileFlag & MaxValue) << TileFlagShiftFactor)
            );
    }
}