using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable, IDoorController
{
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

    public void Interact()
    {
        isOpen = !isOpen;
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
