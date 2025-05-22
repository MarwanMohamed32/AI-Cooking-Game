using UnityEngine;

public class InventoryManager:MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;

    public itemSlot[] itemSlot;


    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && menuActivated)
        {
            InventoryMenu.SetActive(false);
            menuActivated = false;

        }
        else if (Input.GetKeyDown(KeyCode.I) && !menuActivated)
        {
            InventoryMenu.SetActive(true);
            menuActivated= true;

        }

    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
            {
                Debug.Log($"[InventoryManager] Adding item {itemName} with quantity {quantity} to slot {i}");
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOverItems > 0)
                {
                    // Keep trying to add leftovers
                    return AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }
                return 0;
            }
        }

        Debug.LogWarning("Inventory is full, couldn't add item.");
        return quantity;
    }


    public void DeSelectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }

}
