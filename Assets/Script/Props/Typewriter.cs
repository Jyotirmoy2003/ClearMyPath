using UnityEngine;
using TMPro;
using System.Collections;

public class Typewriter : MonoSingleton<Typewriter>
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private string fullText;
    private bool isTyping;

    #region PUBLIC API

    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        fullText = text;
        typingCoroutine = StartCoroutine(TypeRoutine());
    }

    public void SkipTyping()
    {
        if (!isTyping)
            return;

        StopCoroutine(typingCoroutine);
        targetText.text = fullText;
        isTyping = false;
    }

    #endregion

    private IEnumerator TypeRoutine()
    {
        isTyping = true;
        targetText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            targetText.text += fullText[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
