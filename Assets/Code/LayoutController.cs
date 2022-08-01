using UnityEngine;

public class LayoutController : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private RectTransform UI;
    [SerializeField] private RectTransform Snapshot;

    private readonly LayoutAnchors horizontalAnchors = new()
    {
        UIAnchorMin = new Vector2(0, 0),
        UIAnchorMax = new Vector2(1, 0.5f),
        SnapshotAnchorMin = new Vector2(0, 0.5f),
        SnapshotAnchorMax = new Vector2(1, 1)
    };

    private readonly LayoutAnchors verticalAnchors = new()
    {
        UIAnchorMin = new Vector2(0, 0),
        UIAnchorMax = new Vector2(0.5f, 1),
        SnapshotAnchorMin = new Vector2(0.5f, 0),
        SnapshotAnchorMax = new Vector2(1, 1)
    };

    public void Start()
    {
        settings.OnPerspectiveChange.AddListener(OnPerspectiveChange);
    }

    private void OnPerspectiveChange(Perspective perspective)
    {
        var newAnchors = perspective == Perspective.Horizontal
            ? horizontalAnchors
            : verticalAnchors;

        SetAnchors(newAnchors);
    }

    private void SetAnchors(LayoutAnchors anchors)
    {
        UI.anchorMin = anchors.UIAnchorMin;
        UI.anchorMax = anchors.UIAnchorMax;
        Snapshot.anchorMin = anchors.SnapshotAnchorMin;
        Snapshot.anchorMax = anchors.SnapshotAnchorMax;
    }

    private struct LayoutAnchors {
        public Vector2 UIAnchorMin { get; set; }
        public Vector2 UIAnchorMax { get; set; }
        public Vector2 SnapshotAnchorMin { get; set; }
        public Vector2 SnapshotAnchorMax { get; set; }
    }
}
