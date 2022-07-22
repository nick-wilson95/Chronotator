using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snapshooter : MonoBehaviour
{
    [SerializeField] private bool manualSnapshots;
    [SerializeField] private VideoReader videoReader;
    [SerializeField] private Transform cube;
    [SerializeField] private Material snapshotMaterial;
    [SerializeField] private Image snapshotImage;

    private bool readyToSnap = false;
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

        readyToSnap = true;

        TakeSnapshot();
    }

    private void Update()
    {
        var cubeHasMoved = cubePositionLastFrame != cube.position
            || cubeRotationLastFrame.eulerAngles.y != cube.rotation.eulerAngles.y;

        if (cubeHasMoved && (!manualSnapshots || Input.GetKeyDown(KeyCode.Space)))
        {
            TakeSnapshot();
        }

        cubePositionLastFrame = cube.position;
        cubeRotationLastFrame = cube.rotation;
    }

    public void TakeSnapshot()
    {
        if (!readyToSnap)
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

        // Due to Unity bug, material needs to be refreshed
        snapshotImage.material = null;
        snapshotImage.material = snapshotMaterial;
    }
}
