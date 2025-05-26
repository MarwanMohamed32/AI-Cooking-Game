using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUIButton : MonoBehaviour
{
    [TextArea(2, 5)]
    public string questDescription; // 👈 Set this in the Inspector
    private QuestManager questManager;

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (questManager != null)
        {
            questManager.UpdateDescriptionText(questDescription);
        }
    }
}
