using UnityEngine;

/// <summary>
/// Handles door animation for any cookable device (microwave, oven, fridge, etc.)
/// Controlled externally by CookingDevice through IDoorController interface.
/// </summary>
public class MicrowaveController : MonoBehaviour, IDoorController
{
    [Header("Door Settings")]
    public Transform door;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door Transform not assigned!");
            enabled = false;
            return;
        }

        closedRot = door.localRotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    void Update()
    {
        door.localRotation = Quaternion.Slerp(
            door.localRotation,
            isOpen ? openRot : closedRot,
            Time.deltaTime * speed
        );
    }
}
