using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snapshooter : MonoBehaviour
{
    [SerializeField] private VideoReader videoReader;
    [SerializeField] private Transform cube;
    [SerializeField] private Material snapshotMaterial;
    [SerializeField] private Image snapshotImage;
    [SerializeField] private RectTransform snapshotTransform;

    private Texture2D snapshotTexture;
    private SnapshotRenderer snapshotRenderer;

    private Vector3 cubePositionLastFrame;
    private Quaternion cubeRotationLastFrame;

    private void Start()
    {
        videoReader.OnFinishReading.AddListener(GetTextures);
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
        snapshotTexture = CreateSnapshotTexture(textures[0]);

        snapshotRenderer = new SnapshotRenderer(textures, snapshotTexture, cube);

        TakeSnapshot();
    }

    private Texture2D CreateSnapshotTexture(Texture2D sampleFrame)
    {
        var snapshotTextureHeight = sampleFrame.height;
        var snapshotTextureWidth = sampleFrame.height * snapshotTransform.rect.width / snapshotTransform.rect.height;

        return new Texture2D((int)snapshotTextureWidth, snapshotTextureHeight, TextureFormat.RGB24, false);
    }

    public void TakeSnapshot()
    {
        snapshotRenderer.Render();

        SetSnapshotTexture(snapshotTexture);
    }

    private void SetSnapshotTexture(Texture2D texture)
    {
        snapshotMaterial.mainTexture = texture;

        // Due to Unity bug, material needs to be reassigned
        snapshotImage.material = null;
        snapshotImage.material = snapshotMaterial;
    }
}
