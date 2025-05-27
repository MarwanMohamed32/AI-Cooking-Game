using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public TMP_Text descriptionText;
    private int activeQuestIndex = -1;
    private static readonly Color questIdleColor = new Color32(0x72, 0x44, 0x20, 0xFF); // #724420

    [Header("Quest 1")]
    public TMP_Text quest1Label;
    public Image quest1CheckBox;
    public string[] quest1RequiredItems;
    private bool quest1Completed;
    public GameObject quest1DoneLogo;

    [Header("Quest 2")]
    public TMP_Text quest2Label;
    public Image quest2CheckBox;
    public string[] quest2RequiredItems;
    private bool quest2Completed;
    public GameObject quest2DoneLogo;

    [Header("Quest 3")]
    public TMP_Text quest3Label;
    public Image quest3CheckBox;
    public string[] quest3RequiredItems;
    private bool quest3Completed;
    public GameObject quest3DoneLogo;

    [Header("Quest 4")]
    public TMP_Text quest4Label;
    public Image quest4CheckBox;
    public string[] quest4RequiredItems;
    private bool quest4Completed;
    public GameObject quest4DoneLogo;

    private InventoryManager inventory;

    private void Start()
    {
        if (quest1RequiredItems.Length == 1)
            Debug.LogWarning("[QuestManager] quest1RequiredItems is 1");

        inventory = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        quest1DoneLogo.SetActive(false);
        quest2DoneLogo.SetActive(false);
        quest3DoneLogo.SetActive(false);
        quest4DoneLogo.SetActive(false);

        RefreshQuestUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("[DEBUG] Manually checking Quest 1 completion...");
            if (HasAllItems(quest1RequiredItems))
            {
                Debug.Log("[DEBUG] ✅ Apple exists and quest condition met");
            }
            else
            {
                Debug.Log("[DEBUG] ❌ Apple not detected correctly");
            }
        }
    }

    public void SetActiveQuest(int questIndex)
    {
        activeQuestIndex = questIndex;
        Debug.Log($"[QuestManager] Active quest set to: Quest {questIndex + 1}");

        // Optionally update UI to highlight selected quest
        HighlightActiveQuest();
    }


    public void CheckQuests()
    {
        if (activeQuestIndex == -1)
            return; // No quest selected

        switch (activeQuestIndex)
        {
            case 0:
                if (!quest1Completed && HasAllItems(quest1RequiredItems))
                    quest1Completed = true;
                break;
            case 1:
                if (!quest2Completed && HasAllItems(quest2RequiredItems))
                    quest2Completed = true;
                break;
            case 2:
                if (!quest3Completed && HasAllItems(quest3RequiredItems))
                    quest3Completed = true;
                break;
            case 3:
                if (!quest4Completed && HasAllItems(quest4RequiredItems))
                    quest4Completed = true;
                break;
        }

        RefreshQuestUI();
    }

    private bool HasAllItems(string[] requiredItems)
    {
        foreach (string item in requiredItems)
        {
            bool found = false;

            foreach (var slot in inventory.itemSlot)
            {
                Debug.Log($"[QuestManager] Looking for: {item} | In slot: {slot.itemName} (qty: {slot.quantity})");

                if (slot.itemName == item && slot.quantity > 0)
                {
                    Debug.Log($"[QuestManager] Checking item: '{item}' against slot item '{slot.itemName}'");

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.LogWarning($"[QuestManager] ❌ Missing item: {item}");
                return false;
            }
        }

        return true;
    }


    public int GetActiveQuestIndex()
    {
        return activeQuestIndex;
    }

    public bool QuestContainsItem(string itemName, int questIndex)
    {
        string[] requiredItems = questIndex switch
        {
            0 => quest1RequiredItems,
            1 => quest2RequiredItems,
            2 => quest3RequiredItems,
            3 => quest4RequiredItems,
            _ => null
        };
        Debug.Log($"[QuestManager] Checking if quest {questIndex} requires: {itemName}");


        if (requiredItems == null)
            return false;

        foreach (string item in requiredItems)
        {
            if (item.Trim().ToLower() == itemName.Trim().ToLower())
                return true;
        }

        return false;
    }


    public void UpdateDescriptionText(string newDescription)
    {
        if (descriptionText != null)
            descriptionText.text = newDescription;
    }

    private void HighlightActiveQuest()
    {
        quest1Label.color = activeQuestIndex == 0 ? Color.yellow : questIdleColor;
        quest2Label.color = activeQuestIndex == 1 ? Color.yellow : questIdleColor;
        quest3Label.color = activeQuestIndex == 2 ? Color.yellow : questIdleColor;
        quest4Label.color = activeQuestIndex == 3 ? Color.yellow : questIdleColor;
    }


    public string[] GetActiveQuestItems()
    {
        switch (activeQuestIndex)
        {
            case 0: return quest1RequiredItems;
            case 1: return quest2RequiredItems;
            case 2: return quest3RequiredItems;
            case 3: return quest4RequiredItems;
            default: return new string[0];
        }
    }


    private void RefreshQuestUI()
    {
        quest1CheckBox.color = quest1Completed ? Color.green : Color.white;
        quest1DoneLogo.SetActive(quest1Completed && activeQuestIndex == 0);

        quest2CheckBox.color = quest2Completed ? Color.green : Color.white;
        quest2DoneLogo.SetActive(quest2Completed && activeQuestIndex == 1);

        quest3CheckBox.color = quest3Completed ? Color.green : Color.white;
        quest3DoneLogo.SetActive(quest3Completed && activeQuestIndex == 2);

        quest4CheckBox.color = quest4Completed ? Color.green : Color.white;
        quest4DoneLogo.SetActive(quest4Completed && activeQuestIndex == 3);
    }


}
