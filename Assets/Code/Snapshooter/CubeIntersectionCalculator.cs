using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeIntersectionCalculator
{
    private static readonly List<(int v1, int v2)> edges = new() { (0, 1), (1, 2), (2, 3), (3, 0) };

    private static readonly List<Vector2> relativeCorners = new()
    {
        new Vector2(1, 1),
        new Vector2(1, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 1)
    };

    private readonly Vector2[] corners;

    /// <summary>
    /// Calculates information about how a cube intersects with a x,y plane through the origin.
    /// </summary>
    /// <remarks>
    /// The cube is assumed to have edge length 2, rotate in y and moves in z through the origin.
    /// </remarks>
    public CubeIntersectionCalculator(Transform cube)
    {
        corners = GetCornerPositions(cube);
    }

    public bool IntersectsPlane()
    {
        return corners.Any(v => v.y < 0)
            && corners.Any(v => v.y > 0);
    }

    public Bounds GetIntersectionBounds()
    {
        var bounds = GetSnapshotXBounds(corners);
        var relativeBounds = GetSnapshotBoundsRelative(corners);

        return new Bounds(bounds.Item1, bounds.Item2, relativeBounds.Item1, relativeBounds.Item2);
    }

    private static Vector2[] GetCornerPositions(Transform cube)
    {
        var yAngle = cube.localRotation.eulerAngles.y;

        return relativeCorners
        .Select(x => x.Rotate(-yAngle * Mathf.Deg2Rad) + cube.position.z * Vector2.up)
        .ToArray();
    }

    public static (float, float) GetSnapshotXBounds(Vector2[] corners)
    {
        var (l1, l2) = GetLeftIntersectingEdge(corners);
        var (r1, r2) = GetRightIntersectingEdge(corners);

        var leftXBound = GetXIntercept(corners[l1], corners[l2]);
        var rightXBound = GetXIntercept(corners[r1], corners[r2]);

        return (leftXBound, rightXBound);
    }

    public static (Vector2, Vector2) GetSnapshotBoundsRelative(Vector2[] corners)
    {
        var leftIntersectingEdge = GetLeftIntersectingEdge(corners);
        var rightIntersectingEdge = GetRightIntersectingEdge(corners);

        var relativeLeftXBound = GetXInterceptRelative(leftIntersectingEdge, corners);
        var relativeRightXBound = GetXInterceptRelative(rightIntersectingEdge, corners);

        return (relativeLeftXBound, relativeRightXBound);
    }

    private static (int v1, int v2) GetLeftIntersectingEdge(Vector2[] corners)
    {
        return edges.Single(x => corners[x.v1].y < 0 && corners[x.v2].y > 0);
    }

    private static (int v1, int v2) GetRightIntersectingEdge(Vector2[] corners)
    {
        return edges.Single(x => corners[x.v2].y < 0 && corners[x.v1].y > 0);
    }

    private static float GetXIntercept(Vector2 v1, Vector2 v2)
    {
        return v1.x * Mathf.Abs(v2.y / (v1.y - v2.y)) + v2.x * Mathf.Abs(v1.y / (v1.y - v2.y));
    }

    private static Vector2 GetXInterceptRelative((int, int) edge, Vector2[] corners)
    {
        var v1 = relativeCorners[edge.Item1];
        var v2 = relativeCorners[edge.Item2];

        var y1 = corners[edge.Item1].y;
        var y2 = corners[edge.Item2].y;

        return v1 * Mathf.Abs(y2 / (y1 - y2)) + v2 * Mathf.Abs(y1 / (y1 - y2));
    }
}
