using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private string fullText;
    private string currentText = "";
    private float typingSpeed = 0.1f;

    private void Start()
    {
        fullText = textMeshPro.text;
        textMeshPro.text = "";
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textMeshPro.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
