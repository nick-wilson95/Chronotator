using System.Collections.Generic;
using UnityEngine;

public class SnapshotRenderer
{
    private readonly List<Texture2D> textures;
    private readonly Transform cube;
    private readonly int frameCount;
    private readonly int frameWidth;

    Color32 resetColor = new(0, 0, 0, 1);
    readonly Color32[] resetColorArray;

    private readonly Texture2D snapshotTexture;
    private bool isReset = false;

    public SnapshotRenderer(List<Texture2D> textures, Texture2D snapshotTexture, Transform cube)
    {
        this.snapshotTexture = snapshotTexture;
        this.cube = cube;
        this.textures = textures;

        resetColorArray = GenerateResetColourArray(snapshotTexture);

        frameCount = textures.Count;
        frameWidth = textures[0].width;
    }

    public void Render()
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

        SetPixels(bounds);
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

    private void SetPixels(Bounds bounds)
    {
        snapshotTexture.SetPixels32(resetColorArray);

        var snapshotTextureCentreX = snapshotTexture.width / 2;

        var leftPixelBound = (int)(snapshotTextureCentreX + bounds.Left * frameWidth / 2);
        var rightPixelBound = (int)(snapshotTextureCentreX + bounds.Right * frameWidth / 2);

        if (rightPixelBound == leftPixelBound) return;

        for (var i = 0; i <= rightPixelBound - leftPixelBound; i++)
        {
            var topFaceLocation = Vector2.Lerp(bounds.RelativeLeft, bounds.RelativeRight, (float)i / (rightPixelBound - leftPixelBound));

            var pixelColumn = GetPixelColumn(topFaceLocation);

            snapshotTexture.SetColumn(leftPixelBound + i, pixelColumn);
        }

        snapshotTexture.Apply();
    }

    private Color[] GetPixelColumn(Vector2 topFaceLocation)
    {
        var textureDepth = (int)Mathf.Clamp((1 + topFaceLocation.y) / 2 * frameCount, 0, frameCount - 1);

        var xIndex = (int)Mathf.Clamp((1 + topFaceLocation.x) / 2 * frameWidth, 0, frameWidth - 1);

        return textures[textureDepth].GetColumn(xIndex);
    }

    private void ResetSnapshotTexture()
    {
        snapshotTexture.SetPixels32(resetColorArray);
        snapshotTexture.Apply();
    }
}
