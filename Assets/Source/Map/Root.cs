using UnityEngine;
using UnityEngine.UI;

public class Root : MonoBehaviour
{
    [SerializeField] private BiomColorSetter _colorSetter;
    [SerializeField] private RawImage _image;


    private void Awake()
    {
        Map map = new Map();
        TextureCreator textureCreator = new(_colorSetter.BiomColorPairs);
        _image.texture = textureCreator.Create(map.MapContent);
    }
}