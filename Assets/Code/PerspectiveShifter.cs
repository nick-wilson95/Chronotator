using UnityEngine;

public class PerspectiveShifter : MonoBehaviour
{
    [SerializeField] private Transform horizontalViewOrigin;
    [SerializeField] private Transform verticalViewOrigin;
    [SerializeField] private Transform perceiver;

    public void Shift(Perspective perspective)
    {
        var copyFrom = perspective == Perspective.Horizontal
            ? horizontalViewOrigin
            : verticalViewOrigin;

        perceiver.SetPositionAndRotation(copyFrom.position, copyFrom.rotation);
    }
}
