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

    private Texture2D horizontalSnapshotTexture;
    private SnapshotRenderer horizontalSnapshotRenderer;

    private Texture2D verticalSnapshotTexture;
    private SnapshotRenderer verticalSnapshotRenderer;

    private Vector3 cubePositionLastFrame;
    private Quaternion cubeRotationLastFrame;

    private void Start()
    {
        videoReader.OnFinishReading.AddListener(GetTextures);
        //settings.OnPerspectiveChange.AddListener(TakeSnapshot);
    }

    private void Update()
    {
        var cubeHasMoved = cubePositionLastFrame != cube.position;
        var cubeHasRotated = cubeRotationLastFrame.eulerAngles.y != cube.rotation.eulerAngles.y;

        if (!videoReader.IsReading && (cubeHasMoved || cubeHasRotated))
        {
            TakeSnapshot(settings.Perspective);
        }

        cubePositionLastFrame = cube.position;
        cubeRotationLastFrame = cube.rotation;
    }

    private void GetTextures(List<Texture2D> textures)
    {
        horizontalSnapshotTexture = CreateHorizontalSnapshotTexture(textures[0]);
        horizontalSnapshotRenderer = new SnapshotRenderer(textures, horizontalSnapshotTexture, cube, Perspective.Horizontal);

        verticalSnapshotTexture = CreateVerticalSnapshotTexture(textures[0]);
        verticalSnapshotRenderer = new SnapshotRenderer(textures, verticalSnapshotTexture, cube, Perspective.Vertical);

        TakeSnapshot(settings.Perspective);
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

    public void TakeSnapshot(Perspective perspective)
    {
        if (perspective == Perspective.Horizontal)
        {
            horizontalSnapshotRenderer.Render();
            SetSnapshotTexture(horizontalSnapshotTexture);
        }
        else
        {
            verticalSnapshotRenderer.Render();
            SetSnapshotTexture(verticalSnapshotTexture);
        }
    }

    private void SetSnapshotTexture(Texture2D texture)
    {
        snapshotMaterial.mainTexture = texture;

        // Due to Unity bug, material needs to be reassigned
        snapshotImage.material = null;
        snapshotImage.material = snapshotMaterial;
    }
}
