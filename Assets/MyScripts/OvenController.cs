using UnityEngine;

public class OvenDoorController : MonoBehaviour, IInteractable
{
    public Transform ovenDoor;
    public float openAngle = 90f;
    public float speed = 2f;
    public Vector3 rotationAxis = Vector3.right;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        if (ovenDoor == null)
        {
            ovenDoor = transform;
            Debug.LogWarning("No oven door assigned. Using self.");
        }

        closedRot = ovenDoor.localRotation;
        openRot = Quaternion.AngleAxis(openAngle, rotationAxis) * closedRot;
    }

    public void Interact()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        ovenDoor.localRotation = Quaternion.Slerp(
            ovenDoor.localRotation,
            isOpen ? openRot : closedRot,
            Time.deltaTime * speed
        );
    }
}
