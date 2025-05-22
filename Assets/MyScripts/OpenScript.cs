using UnityEngine;

public class OpenScript : MonoBehaviour, IInteractable
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

    void Update()
    {
        door.localRotation = Quaternion.Slerp(
            door.localRotation,
            isOpen ? openRot : closedRot,
            Time.deltaTime * speed
        );
    }
}
