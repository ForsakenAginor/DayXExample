using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TextureCreator
{
    private readonly IReadOnlyDictionary<Biom, Color> _biomColorsPairs;

    public TextureCreator(IReadOnlyDictionary<Biom, Color> biomColorsPairs)
    {
        _biomColorsPairs = biomColorsPairs != null ? biomColorsPairs : throw new ArgumentNullException(nameof(biomColorsPairs));
    }

    public Texture2D Create(IEnumerable<byte> data)
    {
        byte[] map = data.ToArray();
        int size = (int)Mathf.Sqrt(map.Length);
        Texture2D resultTexture = new Texture2D(size, size);
        int index = 0;
        Color color;

        for (int y = 0; y < resultTexture.height; y++)
        {
            for (int x = 0; x < resultTexture.width; x++)
            {
                color = _biomColorsPairs[GetBiom(map[index])];
                resultTexture.SetPixel(x, y, color);
                index++;
            }
        }

        resultTexture.filterMode = FilterMode.Bilinear;
        resultTexture.Apply();
        return resultTexture;
    }

    private Biom GetBiom(byte data)
    {
        return (Biom)(int)(data >> 4);
    }
}