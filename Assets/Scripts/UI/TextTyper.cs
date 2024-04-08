using System.Collections;
using TMPro;
using UnityEngine;

public class TextTyper : MonoBehaviour
{
    public float TypingSpeed = 0.05f;
    public delegate void OnTypingCompleted();
    public event OnTypingCompleted TypingCompleted;

    private Coroutine _typingCoroutine;

    public void DisplayText(string textToType, TMP_Text textElement)
    {
        // If there's already a typing event happening, stop it
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        // Start a new typing coroutine
        _typingCoroutine = StartCoroutine(TypeText(textToType, textElement));
    }

    public void CompleteImmediately(TMP_Text textElement, string fullText)
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }
        textElement.text = fullText; // Display the full text immediately
    }

    IEnumerator TypeText(string text, TMP_Text textElement)
    {
        textElement.text = ""; // Clear text element before typing
        foreach (char letter in text.ToCharArray())
        {
            textElement.text += letter; // Add characters one by one
            yield return new WaitForSeconds(TypingSpeed); // Delay for typing speed
        }
        TypingCompleted?.Invoke();
    }
}
