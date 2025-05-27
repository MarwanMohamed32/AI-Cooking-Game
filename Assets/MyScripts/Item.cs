using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [Header("Raw Item Info")]
    [SerializeField] public string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;
    [TextArea][SerializeField] private string itemDescription;
    [SerializeField] private GameObject placeablePrefab;

    [Header("Cooked Output")]
    public bool isCookable = false;
    public string cookedItemName;
    public int cookedQuantity = 1;
    [SerializeField] private GameObject cookedPlaceablePrefab;

    public Sprite cookedSprite;
    [TextArea] public string cookedDescription;

    [SerializeField] private InventoryManager inventoryManager;

    private void Start()
    {
        if (inventoryManager == null)
            inventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();

        if (placeablePrefab != null)
        {
            PrefabRegistry.Register(itemName, placeablePrefab);
        }
        else
        {
            placeablePrefab = PrefabRegistry.Get(itemName);
            if (placeablePrefab != null)
            {
                Debug.Log($"[Item] Recovered missing prefab for '{itemName}' from PrefabRegistry.");
            }
            else
            {
                Debug.LogWarning($"[Item] placeablePrefab was null for '{itemName}' and could not be recovered.");
            }
        }
    }

    public void Interact()
    {
        Debug.Log($"[Item] Interacted: {itemName}");

        int leftOverItems = inventoryManager.AddItem(
            itemName,
            quantity,
            sprite,
            itemDescription,
            isCookable,
            isCookable ? GetCookedData() : null,
            GetPlaceablePrefab()
        );

        // 🐦 Bird reacts if this item is needed for the current quest
        QuestManager questManager = FindObjectOfType<QuestManager>();
        BirdAssistant bird = FindObjectOfType<BirdAssistant>();

        if (questManager != null && bird != null)
        {
            string[] neededItems = questManager.GetActiveQuestItems();
            foreach (string item in neededItems)
            {
                if (item == itemName)
                {
                    bird.Say("Nice! That’s one of the quest items! 🍎");
                    break;
                }
            }
        }

        if (leftOverItems <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            quantity = leftOverItems;
        }
    }

    public GameObject GetPlaceablePrefab()
    {
        return placeablePrefab;
    }

    public CookedItemData GetCookedData()
    {
        return new CookedItemData
        {
            name = cookedItemName,
            quantity = cookedQuantity,
            sprite = cookedSprite,
            description = cookedDescription,
            placeablePrefab = cookedPlaceablePrefab
        };
    }
}
