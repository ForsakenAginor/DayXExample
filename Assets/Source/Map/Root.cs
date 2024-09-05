using UnityEngine;
using System;
using System.Threading.Tasks;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class Root : MonoBehaviour
{
    [SerializeField] private BiomColorSetter _colorSetter;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Nature generation")]
    [SerializeField][Range(0, 1)] private float _plainsSizeFactor;
    [SerializeField] private float _forestSpawnFrequency;
    [SerializeField] private float _riverWidth;
    [SerializeField] private float _riverFrequency;

    [Header("Cities")]
    [SerializeField] private int _distanceBetweenCities;
    [SerializeField] private int _spread;

    [Header("UI")]
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private TextMeshProUGUI _loadingProgressField;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private MapZoom _mapZoom;

    private Map _map;
    private TextureCreator _textureCreator;
    private SaveManager _saveManager;

    private async void Start()
    {
        _loadingScreen.SetActive(true);
        _loadingProgressField.text = "Creating map";
        _map = new(_plainsSizeFactor, _forestSpawnFrequency, _riverWidth, _riverFrequency);
        _textureCreator = new(_colorSetter.BiomColorPairs);

        _loadingProgressField.text = "Generating forests";
        await Task.Run(() => { _map.GenerateForest(); });

        _loadingProgressField.text = "Generating rivers";
        await Task.Run(() => { _map.GenerateRivers(); });

        _loadingProgressField.text = "Spawning cities";
        CitiesSpawner citiesSpawner = new(_map.GetSize(), _distanceBetweenCities, _spread);

        _loadingProgressField.text = "Creating texture";
        CreateTexture(_map.Data);

        _loadingScreen.SetActive(false);
        _saveManager = new SaveManager();
        MouseInput mouseInput = new MouseInput();
        _mapZoom.Init(mouseInput);
    }

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClick);
        _loadButton.onClick.AddListener(OnLoadButtonClick);
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClick);
        _loadButton.onClick.RemoveListener(OnLoadButtonClick);
    }

    private void OnLoadButtonClick()
    {
        CreateTexture(_saveManager.LoadFromFile());
    }

    private void OnSaveButtonClick()
    {
        _saveManager.SaveToFile(_map.Data);
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