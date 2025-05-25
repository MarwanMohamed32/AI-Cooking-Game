using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [Header("Raw Item Info")]
    [SerializeField] public string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;
    [TextArea][SerializeField] private string itemDescription;
    public GameObject placeablePrefab;

    [Header("Cooked Output")]
    public bool isCookable = false;
    public string cookedItemName;
    public int cookedQuantity = 1;
    public Sprite cookedSprite;
    [TextArea] public string cookedDescription;

    [SerializeField] private InventoryManager inventoryManager;

    void Start()
    {
        if (inventoryManager == null)
            inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
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
          placeablePrefab  // ← now you're passing this too!
      );


        if (leftOverItems <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            quantity = leftOverItems;
        }
    }

    // Optional: used by the oven
    public CookedItemData GetCookedData()
    {
        return new CookedItemData
        {
            name = cookedItemName,
            quantity = cookedQuantity,
            sprite = cookedSprite,
            description = cookedDescription
        };
    }
}
