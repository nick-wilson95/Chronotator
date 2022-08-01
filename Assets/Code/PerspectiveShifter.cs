using UnityEngine;

public class PerspectiveShifter : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform horizontalViewOrigin;
    [SerializeField] private Transform verticalViewOrigin;
    [SerializeField] private Transform perceiver;

    private readonly Rect verticalCameraRect = new(0, 0, 0.5f, 1);
    private readonly Rect horizontalCameraRect = new(0, 0, 1, 1);

    private void Start()
    {
        settings.OnPerspectiveChange.AddListener(Shift);
    }

    public void Shift(Perspective perspective)
    {
        mainCamera.rect = perspective == Perspective.Vertical
            ? verticalCameraRect
            : horizontalCameraRect;

        var copyFrom = perspective == Perspective.Horizontal
            ? horizontalViewOrigin
            : verticalViewOrigin;

        perceiver.SetPositionAndRotation(copyFrom.position, copyFrom.rotation);
    }
}
