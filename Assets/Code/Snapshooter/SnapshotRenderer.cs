using System.Collections.Generic;
using UnityEngine;

public class SnapshotRenderer
{
    private readonly Transform cube;
    private readonly int frameCount;
    private readonly int frameWidth;
    private readonly int frameHeight;

    private readonly Color32 resetColor = new(0, 0, 0, 1);
    private Color32[] resetColorArray;

    private bool isReset = false;

    public List<Texture2D> Textures { get; private set; }
    public Texture2D SnapshotTexture { get; private set; }

    public SnapshotRenderer(List<Texture2D> textures, Texture2D snapshotTexture, Transform cube)
    {
        this.cube = cube;

        Textures = textures;
        SnapshotTexture = snapshotTexture;
        resetColorArray = GenerateResetColourArray(SnapshotTexture);

        frameCount = textures.Count;
        frameWidth = textures[0].width;
        frameHeight = textures[0].height;
    }

    public void Render(Perspective perspective)
    {

        var intersectionCalculator = new CubeIntersectionCalculator(cube);

        if (!intersectionCalculator.IntersectsPlane())
        {
            if (!isReset)
            {
                ResetSnapshotTexture();
                isReset = true;
            }

            return;
        }

        isReset = false;

        var bounds = intersectionCalculator.GetIntersectionBounds();

        SetPixels(perspective, bounds);
    }

    public void UpdateSnapshotTexture(Texture2D snapshotTexture)
    {
        SnapshotTexture = snapshotTexture;
        resetColorArray = GenerateResetColourArray(snapshotTexture);
    }

    private Color32[] GenerateResetColourArray(Texture2D snapshotTexture)
    {
        var resetColorArray = snapshotTexture.GetPixels32();

        for (int i = 0; i < resetColorArray.Length; i++)
        {
            resetColorArray[i] = resetColor;
        }

        return resetColorArray;
    }

    private void SetPixels(Perspective perspective, Bounds bounds)
    {
        SnapshotTexture.SetPixels32(resetColorArray);

        if (perspective == Perspective.Horizontal)
        {
            SetColumns(bounds);
        }
        else
        {
            SetRows(bounds);
        }

        SnapshotTexture.Apply();
    }

    private void SetColumns(Bounds bounds)
    {
        var snapshotTextureCentreX = SnapshotTexture.width / 2;

        var leftPixelBound = (int)(snapshotTextureCentreX + bounds.Left * frameWidth / 2);
        var rightPixelBound = (int)(snapshotTextureCentreX + bounds.Right * frameWidth / 2);

        if (rightPixelBound == leftPixelBound) return;

        for (var i = 0; i <= rightPixelBound - leftPixelBound; i++)
        {
            if (leftPixelBound + i < 0 || leftPixelBound + i >= SnapshotTexture.width) continue;

            var topFaceLocation = Vector2.Lerp(bounds.RelativeLeft, bounds.RelativeRight, (float)i / (rightPixelBound - leftPixelBound));

            var pixelColumn = GetPixelColumn(topFaceLocation);

            SnapshotTexture.SetColumn(leftPixelBound + i, pixelColumn);
        }
    }

    private Color[] GetPixelColumn(Vector2 topFaceLocation)
    {
        var textureDepth = (int)Mathf.Clamp((1 + topFaceLocation.y) / 2 * frameCount, 0, frameCount - 1);

        var xIndex = (int)Mathf.Clamp((1 + topFaceLocation.x) / 2 * frameWidth, 0, frameWidth - 1);

        return Textures[textureDepth].GetColumn(xIndex);
    }

    private void SetRows(Bounds bounds)
    {
        var snapshotTextureCentreY = SnapshotTexture.height / 2;

        var topPixelBound = (int)(snapshotTextureCentreY - bounds.Right * frameHeight / 2);
        var bottomPixelBound = (int)(snapshotTextureCentreY - bounds.Left * frameHeight / 2);

        if (topPixelBound == bottomPixelBound) return;

        for (var i = 0; i <= bottomPixelBound - topPixelBound; i++)
        {
            if (topPixelBound + i < 0 || topPixelBound + i >= SnapshotTexture.height) continue;

            var rightFaceLocation = Vector2.Lerp(-bounds.RelativeRight, -bounds.RelativeLeft, (float)i / (bottomPixelBound - topPixelBound));

            var pixelColumn = GetPixelRow(rightFaceLocation);

            SnapshotTexture.SetRow(topPixelBound + i, pixelColumn);
        }
    }

    private Color[] GetPixelRow(Vector2 rightFaceLocation)
    {
        var textureDepth = (int)Mathf.Clamp((1 + rightFaceLocation.y) / 2 * frameCount, 0, frameCount - 1);

        var rowIndex = (int)Mathf.Clamp((1 + rightFaceLocation.x) / 2 * frameHeight, 0, frameHeight - 1);

        return Textures[textureDepth].GetRow(rowIndex);
    }

    private void ResetSnapshotTexture()
    {
        SnapshotTexture.SetPixels32(resetColorArray);
        SnapshotTexture.Apply();
    }
}
