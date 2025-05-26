using UnityEngine;
using TMPro;
using System.Collections;

public class BirdAssistant : MonoBehaviour
{
    [Header("Speech Settings")]
    public TextMeshProUGUI speechText;        // Assigned from scene
    public float speechDuration = 3f;

    [Header("Random Quotes")]
    public string[] randomQuotes = {
        "Are you just throwing things in the pot now?",
        "That smells... experimental 🫠",
        "Did you check the quest? Just saying.",
        "You're cooking like my cousin and he's a bird.",
        "No judgment, but... what *is* that?"
    };

    private Coroutine speechRoutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Say("Testing 123 from key press!");
        }
    }

    public void Say(string message)
    {
        if (speechText == null)
        {
            Debug.LogWarning("[BirdAssistant] Speech text is not assigned!");
            return;
        }

        if (speechRoutine != null)
            StopCoroutine(speechRoutine);

        speechRoutine = StartCoroutine(ShowSpeech(message));
    }

    private IEnumerator ShowSpeech(string message)
    {
        speechText.text = message;
        speechText.gameObject.SetActive(true);

        yield return new WaitForSeconds(speechDuration);

        speechText.gameObject.SetActive(false);
    }

    public void SayRandomQuote()
    {
        if (randomQuotes.Length > 0)
        {
            int index = Random.Range(0, randomQuotes.Length);
            Say(randomQuotes[index]);
        }
    }
}
