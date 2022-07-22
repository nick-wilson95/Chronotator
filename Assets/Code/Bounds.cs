using UnityEngine;

public struct Bounds
{
    public Bounds(float leftBound, float rightBound, Vector2 leftBoundRelative, Vector2 rightBoundRelative)
    {
        LeftBound = leftBound;
        RightBound = rightBound;
        LeftBoundRelative = leftBoundRelative;
        RightBoundRelative = rightBoundRelative;
    }

    public float LeftBound { get; }
    public float RightBound { get; }
    public Vector2 LeftBoundRelative { get; }
    public Vector2 RightBoundRelative { get; }
}
