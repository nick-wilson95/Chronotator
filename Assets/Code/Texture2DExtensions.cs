using UnityEngine;

public static class Texture2DExtensions
{
    public static void SetColumn(this Texture2D texture, int x, Color[] pixels)
    {
        texture.SetPixels(x, 0, 1, texture.height, pixels);
    }

    public static Color[] GetColumn(this Texture2D texture, int x)
    {
        return texture.GetPixels(x, 0, 1, texture.height);
    }
}
