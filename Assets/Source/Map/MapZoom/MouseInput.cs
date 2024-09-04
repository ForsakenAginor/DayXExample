using UnityEngine;

public class MouseInput : IZoomInput
{
    public float GetZoomValue() => Input.mouseScrollDelta.y;
}
