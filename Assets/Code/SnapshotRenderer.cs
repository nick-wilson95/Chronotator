using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapshotRenderer
{
    private List<Texture2D> textures;
    private readonly Transform cube;
    private int frameCount;
    private int frameWidth;
    private int frameHeight;

    Color32 resetColor = new Color32(0, 0, 0, 1);
    Color32[] resetColorArray;

    private Texture2D snapshotTexture;

    public SnapshotRenderer(List<Texture2D> textures, Texture2D snapshotTexture, Transform cube)
    {
        this.snapshotTexture = snapshotTexture;
        this.cube = cube;
        this.textures = textures;

        resetColorArray = GenerateResetColourArray(snapshotTexture);

        frameCount = textures.Count;
        frameWidth = textures[0].width;
        frameHeight = textures[0].height;
    }

    public Texture2D Render()
    {
        var intersectionCalculator = new CubeIntersectionCalculator(cube);

        if (!intersectionCalculator.IntersectsPlane())
        {
            return new Texture2D(0, frameHeight, TextureFormat.RGB24, false);
        }

        var bounds = intersectionCalculator.GetIntersectionBounds();

        DrawXBounds(bounds);

        SetPixels(bounds);

        return textures[0];
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

        var leftPixelBound = (int)((snapshotTexture.width / 2) + bounds.LeftBound * frameWidth / 2);
        var rightPixelBound = (int)((snapshotTexture.width / 2) + bounds.RightBound * frameWidth / 2);

        Debug.Log("Starting render");

        for (var i = 0; i <= rightPixelBound - leftPixelBound; i++)
        {
            var topFaceLocation = bounds.LeftBoundRelative + (bounds.RightBoundRelative - bounds.LeftBoundRelative) * ((float)i / (rightPixelBound - leftPixelBound));

            var textureDepth = (int)Mathf.Clamp((1 + topFaceLocation.y) / 2 * frameCount, 0, frameCount - 1);

            var xLocation = (int)Mathf.Clamp((1 + topFaceLocation.x) / 2 * frameWidth, 0, frameWidth - 1);

            var pixels = textures[textureDepth].GetPixels(xLocation, 0, 1, snapshotTexture.height);

            snapshotTexture.SetPixels(i + leftPixelBound, 0, 1, snapshotTexture.height, pixels);
        }

        Debug.Log("Finished render");

        snapshotTexture.Apply();
    }

    private void DrawXBounds(Bounds bounds)
    {
        Debug.DrawLine(Vector3.right * bounds.LeftBound + Vector3.up, Vector3.right * bounds.RightBound + Vector3.up, Color.green);
    }
}
