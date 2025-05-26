using UnityEngine;
using TMPro;

public class CookingDevice : MonoBehaviour, IInteractable
{
    [Header("Cooking Settings")]
    public float cookingDuration = 5f;
    public UnityEngine.UI.Slider progressBar;
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
    private BirdAssistant birdAssistant;
    private QuestManager questManager;
    private int mistakeCount = 0;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        birdAssistant = FindObjectOfType<BirdAssistant>();
        questManager = FindObjectOfType<QuestManager>();

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
            progressBar.value = 0f;
        }

        if (fireVFX != null)
            fireVFX.SetActive(false);

        if (doorComponent != null)
            doorController = doorComponent as IDoorController;
    }

    public void Interact()
    {
        ToggleInteraction();
    }

    private void Update()
    {
        if (isCooking)
        {
            cookingTimer -= Time.deltaTime;

            float progress = Mathf.Clamp01(1f - (cookingTimer / cookingDuration));
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

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

        // ✅ Check if this item matches the active quest
        bool matchesQuest = false;
        Debug.Log("[CookingDevice] Cooking item: " + data.name);
        Debug.Log("[CookingDevice] Active quest index: " + questManager.GetActiveQuestIndex());

        if (questManager != null)
        {
            switch (questManager.GetActiveQuestIndex())
            {
                case 0: matchesQuest = questManager.QuestContainsItem(data.name, 0); break;
                case 1: matchesQuest = questManager.QuestContainsItem(data.name, 1); break;
                case 2: matchesQuest = questManager.QuestContainsItem(data.name, 2); break;
                case 3: matchesQuest = questManager.QuestContainsItem(data.name, 3); break;
            }
        }

        // ✅ Let the bird respond
        if (birdAssistant != null)
        {
            if (matchesQuest)
            {
                birdAssistant.Say("That’s the right ingredient! You're on fire, chef! 🔥");
                mistakeCount = 0;
            }
            else
            {
                mistakeCount++;
                if (mistakeCount >= 3)
                {
                    birdAssistant.Say("You keep messing up... Try cooking what's needed in the quest!");
                }
                else
                {
                    birdAssistant.Say("Hmm... I don’t think that’s what the quest needs.");
                }
            }
        }

        cookedItem = data;
        isCooking = true;
        isOpen = false;
        isReadyForItem = false;
        cookingTimer = cookingDuration;

        inventoryManager?.CloseInventory();
        doorController?.CloseDoor();

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0f;
        }

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

        if (progressBar != null)
        {
            progressBar.value = 1f;
            progressBar.gameObject.SetActive(false);
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
