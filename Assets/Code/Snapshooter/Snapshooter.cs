using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snapshooter : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private VideoReader videoReader;
    [SerializeField] private Transform cube;
    [SerializeField] private Material snapshotMaterial;
    [SerializeField] private Image snapshotImage;
    [SerializeField] private RectTransform snapshotTransform;

    private SnapshotRenderer snapshotRenderer;

    private Vector3 cubePositionLastFrame;
    private Quaternion cubeRotationLastFrame;

    private void Start()
    {
        videoReader.OnFinishReading.AddListener(GetTextures);
        settings.OnPerspectiveChange.AddListener(OnPerspectiveChange);
    }

    private void Update()
    {
        var cubeHasMoved = cubePositionLastFrame != cube.position;
        var cubeHasRotated = cubeRotationLastFrame.eulerAngles.y != cube.rotation.eulerAngles.y;

        if (!videoReader.IsReading && (cubeHasMoved || cubeHasRotated))
        {
            TakeSnapshot();
        }

        cubePositionLastFrame = cube.position;
        cubeRotationLastFrame = cube.rotation;
    }

    private void GetTextures(List<Texture2D> textures)
    {
        var snapshotTexture = CreateSnapshotTexture(textures[0]);
        snapshotRenderer = new SnapshotRenderer(textures, snapshotTexture, cube);

        TakeSnapshot();
    }

    private Texture2D CreateSnapshotTexture(Texture2D sampleFrame)
    {
        return settings.Perspective == Perspective.Horizontal
            ? CreateHorizontalSnapshotTexture(sampleFrame)
            : CreateVerticalSnapshotTexture(sampleFrame);
    }

    private Texture2D CreateHorizontalSnapshotTexture(Texture2D sampleFrame)
    {
        var snapshotTextureHeight = sampleFrame.height;
        var snapshotTextureWidth = sampleFrame.height * snapshotTransform.rect.width / snapshotTransform.rect.height;

        return new Texture2D((int)snapshotTextureWidth, snapshotTextureHeight, TextureFormat.RGB24, false);
    }

    private Texture2D CreateVerticalSnapshotTexture(Texture2D sampleFrame)
    {
        var snapshotTextureWidth = sampleFrame.width;
        var snapshotTextureHeight = sampleFrame.width * snapshotTransform.rect.height / snapshotTransform.rect.width;

        return new Texture2D(snapshotTextureWidth, (int)snapshotTextureHeight, TextureFormat.RGB24, false);
    }

    private void TakeSnapshot()
    {
        snapshotRenderer.Render(settings.Perspective);
        SetSnapshotTexture(snapshotRenderer.SnapshotTexture);
    }

    private void OnPerspectiveChange(Perspective newPerspective)
    {
        var newSnapshotTexture = CreateSnapshotTexture(snapshotRenderer.Textures[0]);
        snapshotRenderer.UpdateSnapshotTexture(newSnapshotTexture);

        TakeSnapshot();
    }

    private void SetSnapshotTexture(Texture2D texture)
    {
        snapshotMaterial.mainTexture = texture;

        // Due to Unity bug, material needs to be reassigned
        snapshotImage.material = null;
        snapshotImage.material = snapshotMaterial;
    }
}
