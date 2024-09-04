using System;
using UnityEngine;

public class MapZoom : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minimumZoom;
    [SerializeField] private float _maximumZoom;

    private IZoomInput _input;

    private void FixedUpdate()
    {
        if (_input == null)
            return;

        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + (_input.GetZoomValue() * _zoomSpeed), _minimumZoom, _maximumZoom);
    }

    public void Init(IZoomInput input)
    {
        _input = input != null ? input : throw new ArgumentNullException(nameof(input));
    }
}