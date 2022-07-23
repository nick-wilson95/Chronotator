using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snapshooter : MonoBehaviour
{
    [SerializeField] private VideoReader videoReader;
    [SerializeField] private Transform cube;
    [SerializeField] private Material snapshotMaterial;
    [SerializeField] private Image snapshotImage;

    private bool texturesLoaded = false;
    private Texture2D snapshotTexture;
    private SnapshotRenderer snapshotRenderer;

    private Vector3 cubePositionLastFrame;
    private Quaternion cubeRotationLastFrame;

    private void Start()
    {
        videoReader.OnFinishReading.AddListener(GetTextures);
    }

    private void GetTextures(List<Texture2D> textures)
    {
        snapshotTexture = new Texture2D((int)(1.5f * textures[0].width), textures[0].height, TextureFormat.RGB24, false);

        snapshotRenderer = new SnapshotRenderer(textures, snapshotTexture, cube);

        texturesLoaded = true;

        TakeSnapshot();
    }

    private void Update()
    {
        var cubeHasMoved = cubePositionLastFrame != cube.position;
        var cubeHasRotated = cubeRotationLastFrame.eulerAngles.y != cube.rotation.eulerAngles.y;

        if (cubeHasMoved || cubeHasRotated)
        {
            TakeSnapshot();
        }

        cubePositionLastFrame = cube.position;
        cubeRotationLastFrame = cube.rotation;
    }

    public void TakeSnapshot()
    {
        if (!texturesLoaded)
        {
            Debug.Log("Can't build snapshot - textures not loaded");
            return;
        }

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
