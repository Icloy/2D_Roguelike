using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private string fullText;
    private string currentText = "";
    private float typingSpeed = 0.1f;

    private float sceneTimer = 0f;
    private float goal = 10f;


    private void Start()
    {
        fullText = textMeshPro.text;
        textMeshPro.text = "";
        StartCoroutine(ShowText());
    }
    private void Update()
    {
        sceneTimer += Time.deltaTime;
        if(sceneTimer > goal) 
        {
            LoadingScene.LoadScene("Menu_Scene");
        }
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
