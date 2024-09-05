using ProceduralNoiseProject;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private const int Size = 4000;

    private readonly byte[] _mapContent = new byte[Size * Size];
    private readonly float _plainsSizeFactor;
    private readonly float _forestSpawnFrequency;
    private readonly float _riverWidth;
    private readonly float _riverFrequency;

    public Map(float plainsSizeFactor, float forestSpawnFrequency, float riverWidth, float riverFrequency)
    {
        _plainsSizeFactor = plainsSizeFactor >= 0 && plainsSizeFactor <= 1 ?
            plainsSizeFactor :
            throw new ArgumentOutOfRangeException(nameof(plainsSizeFactor));

        _forestSpawnFrequency = forestSpawnFrequency > 0 ?
            forestSpawnFrequency :
            throw new ArgumentOutOfRangeException(nameof(forestSpawnFrequency));

        _riverWidth = riverWidth;
        _riverFrequency = riverFrequency;
    }

    public IEnumerable<byte> Data => _mapContent;

    public uint GetSize() => Size;

    public void GenerateForest()
    {
        float perlinNoiseResult;
        int index = 0;

        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                perlinNoiseResult = GetPerlinNoiseValue(x, y, _forestSpawnFrequency);

                if (perlinNoiseResult > _plainsSizeFactor)
                    _mapContent[index] = Encode(Biom.Forest, TileFlags.None);
                else
                    _mapContent[index] = Encode(Biom.Plains, TileFlags.None);

                index++;
            }
        }
    }

    public void GenerateRivers()
    {
        int seed = 0;
        float noiseAmplitude = 20f;
        Noise noise = new VoronoiNoise(seed, _riverFrequency, noiseAmplitude);
        float result;
        int index = 0;

        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                float fx = x / (Size - 1.0f);
                float fy = y / (Size - 1.0f);

                result = noise.Sample2D(fx, fy);

                if (result < _riverWidth)
                    _mapContent[index] = Encode(Biom.Water, TileFlags.None);

                index++;
            }
        }
    }


    private float GetPerlinNoiseValue(int x, int y, float frequency)
    {
        return Mathf.Clamp01(
            Mathf.PerlinNoise(
                (float)x / Size * frequency,
                (float)y / Size * frequency));
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