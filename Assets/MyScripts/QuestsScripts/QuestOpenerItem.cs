using UnityEngine;

public class QuestOpenerItem : MonoBehaviour, IInteractable
{
    public GameObject questCanvas;
    private bool isOpen = false;

    public void Interact()
    {
        if (questCanvas != null)
        {
            questCanvas.GetComponent<Canvas>().enabled = true; 

            isOpen = true;
            Debug.Log("[QuestOpenerItem] Quest UI opened");
        }
    }

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            questCanvas.GetComponent<Canvas>().enabled = false; 

            isOpen = false;
            Debug.Log("[QuestOpenerItem] Quest UI closed with Escape");
        }
    }
}
