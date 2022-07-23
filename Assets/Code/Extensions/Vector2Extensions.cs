using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        return new Vector2(
            vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle),
            vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle)
        );
    }
}
