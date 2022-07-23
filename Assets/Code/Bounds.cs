using UnityEngine;

public struct Bounds
{
    public Bounds(float left, float right, Vector2 relaitveLeft, Vector2 relativeRight)
    {
        Left = left;
        Right = right;
        RelativeLeft = relaitveLeft;
        RelativeRight = relativeRight;
    }

    /// <summary>
    /// x position of leftmost edge of intersection
    /// </summary>
    public float Left { get; }

    /// <summary>
    /// x position of rightmost edge of intersection
    /// </summary>
    public float Right { get; }

    /// <summary>
    /// Left bound relative to the cube
    /// </summary>
    public Vector2 RelativeLeft { get; }

    /// <summary>
    /// Right bound relative to the cube
    /// </summary>
    public Vector2 RelativeRight { get; }
}
