using UnityEngine;

public class PerspectiveShifter : MonoBehaviour
{
    [SerializeField] private Transform horizontalViewOrigin;
    [SerializeField] private Transform verticalViewOrigin;
    [SerializeField] private Transform perceiver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Shift(Perspective.Horizontal);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Shift(Perspective.Vertical);
        }
    }

    public void Shift(Perspective perspective)
    {
        var copyFrom = perspective == Perspective.Horizontal
            ? horizontalViewOrigin
            : verticalViewOrigin;

        perceiver.SetPositionAndRotation(copyFrom.position, copyFrom.rotation);
    }
}
