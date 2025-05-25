using UnityEngine;
using TMPro;

public class CookingDevice : MonoBehaviour, IInteractable

{
    [Header("Cooking Settings")]
    public float cookingDuration = 5f;
    public TMP_Text timerText;
    public GameObject fireVFX;

    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Dependencies")]
    public MonoBehaviour doorComponent;
    private IDoorController doorController;

    private bool isCooking = false;
    private bool isReadyForItem = false;
    private bool isOpen = false;

    private float cookingTimer;
    private CookedItemData cookedItem;
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();

        if (timerText != null)
        {
            timerText.text = "";
            timerText.gameObject.SetActive(false);
        }

        if (fireVFX != null)
            fireVFX.SetActive(false);

        if (doorComponent != null)
            doorController = doorComponent as IDoorController;
    }

    public void Interact()
    {
        ToggleInteraction(); // open/close inventory and door
    }

    private void Update()
    {

        if (isCooking)
        {
            cookingTimer -= Time.deltaTime;

            if (timerText != null)
                timerText.text = $"Cooking: {Mathf.CeilToInt(cookingTimer)}s";

            if (cookingTimer <= 0f)
                FinishCooking();
        }
    }

    private void ToggleInteraction()
    {
        isOpen = !isOpen;
        isReadyForItem = isOpen;

        if (isOpen)
        {
            inventoryManager?.OpenInventory();
            doorController?.OpenDoor(); // 🔓 open door
            Debug.Log("[CookingDevice] Opened for interaction.");
        }
        else
        {
            inventoryManager?.CloseInventory();
            doorController?.CloseDoor(); // 🔒 close door
            Debug.Log("[CookingDevice] Closed.");
        }
    }

    public void StartCooking(CookedItemData data)
    {
        if (isCooking || data == null)
            return;

        cookedItem = data;
        isCooking = true;
        isOpen = false;
        isReadyForItem = false;
        cookingTimer = cookingDuration;

        inventoryManager?.CloseInventory();
        doorController?.CloseDoor(); // 🚪 auto close after placing item

        if (timerText != null)
            timerText.gameObject.SetActive(true);

        if (fireVFX != null)
            fireVFX.SetActive(true);

        Debug.Log($"[CookingDevice] Started cooking: {cookedItem.name}");
    }

    private void FinishCooking()
    {
        isCooking = false;

        if (cookedItem != null && inventoryManager != null)
        {
            inventoryManager.AddItem(
                cookedItem.name,
                cookedItem.quantity,
                cookedItem.sprite,
                cookedItem.description
            );
            Debug.Log($"[CookingDevice] Finished cooking: {cookedItem.name}");
        }

        cookedItem = null;

        if (timerText != null)
        {
            timerText.text = "";
            timerText.gameObject.SetActive(false);
        }

        if (fireVFX != null)
            fireVFX.SetActive(false);

        isReadyForItem = true;
    }

    public bool IsReady() => isReadyForItem && !isCooking;
    public bool IsOpen() => isOpen;
    public bool IsCooking() => isCooking;
    public void SetReadyForItem(bool ready) => isReadyForItem = ready;
}
