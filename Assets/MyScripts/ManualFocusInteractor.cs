using UnityEngine;
using UnityEngine.UI;

public class ManualFocusInteractor : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayerMask;

    public GameObject crosshairUI; // UI circle
    public GameObject promptUI;    // "Press E to interact" text

    public static CookingDevice lastCookingDevice; //  Globally tracks the last interacted cooker

    private bool isFocusing = false;
    private IInteractable currentTarget;

    private Camera cam;
    private float normalFOV = 60f;
    private float focusFOV = 40f;

    void Start()
    {
        cam = Camera.main;
        if (crosshairUI) crosshairUI.SetActive(false);
        if (promptUI) promptUI.SetActive(false);
    }

    void Update()
    {
        // Toggle focus mode (F key)
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFocusing = !isFocusing;

            if (crosshairUI) crosshairUI.SetActive(isFocusing);
            if (!isFocusing && promptUI) promptUI.SetActive(false);
        }

        // Smooth zoom effect
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, isFocusing ? focusFOV : normalFOV, Time.deltaTime * 6f);

        if (isFocusing)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayerMask))
            {
                currentTarget = hit.collider.GetComponent<IInteractable>();
                if (promptUI) promptUI.SetActive(currentTarget != null);
            }
            else
            {
                currentTarget = null;
                if (promptUI) promptUI.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
            {
                currentTarget.Interact();

                // 👇 Store cooker if it's the thing we're interacting with
                if (currentTarget is CookingDevice cooker)
                    lastCookingDevice = cooker;
            }
        }
    }
}
