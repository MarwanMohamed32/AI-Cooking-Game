using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class itemSlot : MonoBehaviour, IPointerClickHandler
{
    public bool isCookable;
    public CookedItemData cookedItemData;

    // ITEM DATA
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;

    [SerializeField]
    private int maxNumberOfItems;

    // ITEM SLOT UI
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage;

    // ITEM DESCRIPTION UI
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptText;

    public GameObject selectedShader;
    public bool thisItemSelected;
    public GameObject placeablePrefab;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, bool isCookable = false, CookedItemData cookedData = null, GameObject placeablePrefab = null)
    {
        if (isFull)
            return quantity;

        this.isCookable = isCookable;
        this.cookedItemData = cookedData;
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
        this.itemDescription = itemDescription;
        this.placeablePrefab = placeablePrefab;
        if (placeablePrefab == null)
        {
            Debug.LogWarning("[itemSlot] Missing prefab for " + itemName);
        }

        this.quantity += quantity;

        if (this.quantity >= maxNumberOfItems)
        {
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            quantityText.text = this.quantity.ToString();
            quantityText.enabled = true;
            isFull = true;

            // ⬇️ Added line: check quests after item is added
            FindObjectOfType<QuestManager>()?.CheckQuests();
            Debug.Log("[itemSlot] Called QuestManager.CheckQuests()");

            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        // ⬇️ Added line: check quests after item is added
        FindObjectOfType<QuestManager>()?.CheckQuests();
        Debug.Log("[itemSlot] Called QuestManager.CheckQuests()");

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeSelectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;

        itemDescriptionNameText.text = itemName;
        itemDescriptText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;

        inventoryManager.selectedSlot = this;

        Debug.Log("[Inventory] Selected slot: " + itemName);
    }

    public void OnRightClick()
    {
        if (string.IsNullOrEmpty(itemName) || quantity <= 0)
        {
            Debug.LogWarning("[itemSlot] Cannot place: Slot is empty.");
            return;
        }

        CookingDevice cooker = ManualFocusInteractor.lastCookingDevice;

        if (cooker == null)
        {
            Debug.LogWarning("[itemSlot] No CookingDevice found.");
            return;
        }

        if (cooker.IsReady())
        {
            if (isCookable && cookedItemData != null)
            {
                cooker.StartCooking(cookedItemData);
                isCookable = false;
            }
            else
            {
                Debug.LogWarning("[itemSlot] This item is not cookable.");
                return;
            }

            quantity--;

            if (quantity <= 0)
            {
                ClearSlot();
            }
            else
            {
                UpdateQuantityUI();
            }
        }
        else
        {
            Debug.Log("[itemSlot] Cooker is not ready.");
        }
    }

    public void ClearSlot()
    {
        isFull = false;
        itemName = "";
        itemDescription = "";
        itemSprite = null;
        cookedItemData = null;
        isCookable = false;
        thisItemSelected = false;

        itemImage.enabled = false;
        quantityText.enabled = false;
        quantity = 0;
        quantityText.text = "";
    }

    public void UpdateQuantityUI()
    {
        quantityText.text = quantity.ToString();
        quantityText.enabled = quantity > 0;
    }
}
