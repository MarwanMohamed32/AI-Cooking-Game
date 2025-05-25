using UnityEngine;

public class OvenDoorController : MonoBehaviour, IDoorController
{
    public Transform ovenDoor;
    public float openAngle = 90f;
    public float speed = 2f;
    public Vector3 rotationAxis = Vector3.right;

    private Quaternion closedRot, openRot;
    private bool isOpen = false;

    private void Start()
    {
        closedRot = ovenDoor.localRotation;
        openRot = Quaternion.AngleAxis(openAngle, rotationAxis) * closedRot;
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }

    private void Update()
    {
        ovenDoor.localRotation = Quaternion.Slerp(
            ovenDoor.localRotation,
            isOpen ? openRot : closedRot,
            Time.deltaTime * speed
        );
    }
}
