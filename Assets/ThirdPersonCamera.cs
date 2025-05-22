using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 3f;
    public float height = 2f;
    public float mouseSensitivity = 2f;
    public float rotationSmoothSpeed = 10f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw = 0f;
    private float pitch = 10f;

    void LateUpdate()
    {
        if (!target) return;

        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Rotation and position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, rotationSmoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * height);
    }
}
