using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    public Image image;

    public void DisplayImage()
    {
        image.enabled = true;
    }
}

