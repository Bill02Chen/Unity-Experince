using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public Vector2 crosshairSize = new Vector2(12, 12);

    void OnGUI()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Rect crosshairRect = new Rect(
            screenCenter.x - crosshairSize.x / 2,
            screenCenter.y - crosshairSize.y / 2,
            crosshairSize.x,
            crosshairSize.y
        );
        GUI.DrawTexture(crosshairRect, crosshairTexture);
    }
}
