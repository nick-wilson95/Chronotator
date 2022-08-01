using UnityEngine;

public class PerspectiveShifter : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private Transform horizontalViewOrigin;
    [SerializeField] private Transform verticalViewOrigin;
    [SerializeField] private Transform perceiver;

    private void Start()
    {
        settings.OnPerspectiveToggle.AddListener(Shift);
    }

    public void Shift(Perspective perspective)
    {
        var copyFrom = perspective == Perspective.Horizontal
            ? horizontalViewOrigin
            : verticalViewOrigin;

        perceiver.SetPositionAndRotation(copyFrom.position, copyFrom.rotation);
    }
}
