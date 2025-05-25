using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Camera Follow")]
    public Transform target;
    public float distance = 3f;
    public float height = 2f;
    public float mouseSensitivity = 2f;
    public float rotationSmoothSpeed = 10f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float yaw = 0f;
    private float pitch = 10f;
    private float ghostRotationY = 0f; // rotation around Y axis
 

    [Header("Placement")]
    public LayerMask raycastMask;
    public GameObject ghost;
    private GameObject prefabToPlace;

    private itemSlot activeSlot;

    void Update()
    {
        HandlePlacement();
    }

    void HandlePlacement()
    {
        if (prefabToPlace == null || ghost == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastMask))
        {
            ghost.SetActive(true);

            // Handle Q/E rotation
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ghostRotationY -= 15f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                ghostRotationY += 15f;
            }

            // Offset the object so its base sits on surface
            Vector3 offset = Vector3.up * GetPlacementOffsetY(prefabToPlace);

            // Apply position and rotation to ghost
            ghost.transform.position = hit.point + offset;
            ghost.transform.rotation = Quaternion.Euler(0, ghostRotationY, 0);

            // LEFT CLICK to place
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(prefabToPlace, hit.point + offset, Quaternion.Euler(0, ghostRotationY, 0));

                if (activeSlot != null)
                {
                    activeSlot.quantity--;

                    if (activeSlot.quantity <= 0)
                    {
                        activeSlot.ClearSlot();
                    }
                    else
                    {
                        activeSlot.UpdateQuantityUI();
                    }
                }

                StopPlacement();
            }

            // RIGHT CLICK to cancel
            if (Input.GetMouseButtonDown(1))
            {
                StopPlacement();
            }
        }
        else
        {
            ghost.SetActive(false);
        }
    }
    



    public void StartPlacement(GameObject prefab, itemSlot slot)
    {
        if (prefab == null || ghost == null)
        {
            Debug.LogWarning("[Camera] StartPlacement FAILED — prefab or ghost is null.");
            return;
        }

        Debug.Log("[Camera] StartPlacement SUCCESS — prefab: " + prefab.name);
        activeSlot = slot;  // Save the slot to subtract later

        // Copy mesh/material
        MeshFilter srcMF = prefab.GetComponent<MeshFilter>();
        MeshRenderer srcMR = prefab.GetComponent<MeshRenderer>();
        MeshFilter ghostMF = ghost.GetComponent<MeshFilter>();
        MeshRenderer ghostMR = ghost.GetComponent<MeshRenderer>();

        if (srcMF && srcMR && ghostMF && ghostMR)
        {
            ghostMF.mesh = srcMF.sharedMesh;
            ghostMR.material = srcMR.sharedMaterial;
        }

        ghost.SetActive(true);
        prefabToPlace = prefab;
    }


    public void StopPlacement()
    {
        prefabToPlace = null;
        if (ghost != null)
            ghost.SetActive(false);
    }

    void LateUpdate()
    {
        if (!target) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch = Mathf.Clamp(pitch - mouseY, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, rotationSmoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * height);
    }

    private Bounds GetPrefabBounds(GameObject prefab)
    {
        MeshRenderer renderer = prefab.GetComponent<MeshRenderer>();
        if (renderer != null)
            return renderer.bounds;

        // fallback if no renderer on root
        Renderer childRenderer = prefab.GetComponentInChildren<Renderer>();
        if (childRenderer != null)
            return childRenderer.bounds;

        return new Bounds(Vector3.zero, Vector3.zero);
    }

    private float GetPlacementOffsetY(GameObject obj)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
            return renderer.bounds.extents.y;

        Renderer childRenderer = obj.GetComponentInChildren<Renderer>();
        if (childRenderer != null)
            return childRenderer.bounds.extents.y;

        return 0f;
    }


}
