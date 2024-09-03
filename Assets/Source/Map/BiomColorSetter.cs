using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomColorSetter : MonoBehaviour
{
    [SerializeField] private ColorData[] _biomColorDatas;

    private Dictionary<Biom, Color> _biomColorPairs;

    public IReadOnlyDictionary<Biom, Color> BiomColorPairs => _biomColorPairs;

    private void OnValidate()
    {
        _biomColorPairs = new ();

        for (int i = 0; i < _biomColorDatas.Length; i++)
            _biomColorPairs.Add(_biomColorDatas[i].Biom, _biomColorDatas[i].Color);

        if (_biomColorPairs.Count != Enum.GetValues(typeof(Biom)).Length)
            throw new Exception("BiomColorDictionary not completly filled");            
    }

    [Serializable]
    private struct ColorData
    {
        public Color Color;
        public Biom Biom;
    }
}