using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private BiomColorSetter _colorSetter;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField][Range(0, 1)] private float _plainsSizeFactor;
    [SerializeField] private float _forestSpawnFrequency;

    private void Awake()
    {
        Debug.Log("");
        Map map = new (_plainsSizeFactor, _forestSpawnFrequency);
        TextureCreator textureCreator = new (_colorSetter.BiomColorPairs);
        Texture2D texture = textureCreator.Create(map.MapContent);
        Rect mapRect = new Rect(Vector2.zero, new Vector2(texture.width, texture.height));
        Vector2 pivot = new(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(texture, mapRect, pivot);
        _spriteRenderer.sprite = sprite;
        Debug.Log(Time.realtimeSinceStartup);
    }
}