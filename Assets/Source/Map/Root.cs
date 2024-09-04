using UnityEngine;
using System;
using System.Threading.Tasks;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Root : MonoBehaviour
{
    [SerializeField] private BiomColorSetter _colorSetter;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Nature generation")]
    [SerializeField][Range(0, 1)] private float _plainsSizeFactor;
    [SerializeField] private float _forestSpawnFrequency;
    [SerializeField] private float _riverWidth;
    [SerializeField] private float _riverFrequency;
    [SerializeField] private float _riverPerlinFactor;

    [Header("UI")]
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private TextMeshProUGUI _loadingProgressField;

    private Map _map;
    private TextureCreator _textureCreator;

    private async void Start()
    {
        _loadingScreen.SetActive(true);
        _loadingProgressField.text = "Creating map";
        _map = new(_plainsSizeFactor, _forestSpawnFrequency, _riverWidth, _riverFrequency, _riverPerlinFactor);
        _textureCreator = new(_colorSetter.BiomColorPairs);

        _loadingProgressField.text = "Generating forests";
        await Task.Run(() => { _map.GenerateForest(); });

        _loadingProgressField.text = "Generating rivers";
        await Task.Run(() => { _map.GenerateRivers(); });

        CreateTexture(_map.Data);

        _loadingScreen.SetActive(false);
    }

    private void CreateTexture(IEnumerable<byte> map)
    {
        Texture2D texture = _textureCreator.Create(map);
        Rect mapRect = new Rect(Vector2.zero, new Vector2(texture.width, texture.height));
        Vector2 pivot = new(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(texture, mapRect, pivot);
        _spriteRenderer.sprite = sprite;
    }
}