using UnityEngine;

public class CubeControls : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] private VideoReader videoReader;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private float minRotationSpeed;
    [SerializeField] private float maxRotationSpeed;

    private float speed;
    private float rotationSpeed;

    private Vector3 MovementUnit => speed * Time.deltaTime * Vector3.forward;
    private float RotationDegrees => Time.deltaTime * rotationSpeed;

    private void Start()
    {
        settings.OnSpeedChange.AddListener(SetSpeed);
        settings.OnRotationSpeedChange.AddListener(SetRotationSpeed);
    }

    private void Update()
    {
        if (videoReader.IsReading) return;

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

    private void SetSpeed(float value)
    {
        speed = Mathf.Lerp(minSpeed, maxSpeed, value);
    }

    private void SetRotationSpeed(float value)
    {
        rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, value);
    }
}
