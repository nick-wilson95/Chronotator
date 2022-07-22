using UnityEngine;

public class CubeControls : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private Vector3 MovementUnit => speed * Time.deltaTime * Vector3.forward;
    private float RotationDegrees => Time.deltaTime * rotationSpeed;

    private void Update()
    {
        HandleZMovement();
        HandleRotation();
    }

    private void HandleZMovement()
    {
        var moveForwards = Input.GetKey(KeyCode.UpArrow);
        var moveBackwards = Input.GetKey(KeyCode.DownArrow);

        if (moveForwards && !moveBackwards)
        {
            transform.position += MovementUnit;
        }
        else if (!moveForwards && moveBackwards)
        {
            transform.position -= MovementUnit;
        }
    }

    private void HandleRotation()
    {
        var rotateClock = Input.GetKey(KeyCode.RightArrow);
        var rotateAntiClock = Input.GetKey(KeyCode.LeftArrow);

        if (rotateClock && !rotateAntiClock)
        {
            transform.Rotate(Vector3.up, RotationDegrees);
        }
        else if (!rotateClock && rotateAntiClock)
        {
            transform.Rotate(Vector3.up, -RotationDegrees);
        }
    }
}
