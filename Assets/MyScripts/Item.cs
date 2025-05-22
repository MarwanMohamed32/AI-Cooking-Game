using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;
    [SerializeField] private InventoryManager inventoryManager;
    [TextArea][SerializeField] private string itemDescription;

    void Start()
    {
        if (inventoryManager == null)
            inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void Interact()
    {
        Debug.Log($"[Item] Interacted: {itemName}");
        int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
        if (leftOverItems <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            quantity = leftOverItems;
        }

    }
}
