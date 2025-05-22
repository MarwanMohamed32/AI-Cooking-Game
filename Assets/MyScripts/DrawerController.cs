using UnityEngine;

public class DrawerController : MonoBehaviour, IInteractable
{
    public Transform drawer;
    public float slideDistance = 0.5f;
    public float speed = 2f;
    public Vector3 direction = Vector3.forward;

    private bool isOpen = false;
    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        if (drawer == null)
        {
            drawer = transform;
            Debug.LogWarning("Drawer not assigned. Using self.");
        }

        closedPos = drawer.localPosition;
        openPos = closedPos + direction.normalized * slideDistance;
    }

    public void Interact()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        drawer.localPosition = Vector3.Lerp(
            drawer.localPosition,
            isOpen ? openPos : closedPos,
            Time.deltaTime * speed
        );
    }
}
