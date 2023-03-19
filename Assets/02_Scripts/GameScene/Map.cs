using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Map : MonoBehaviour
{
    public Button[] TP;

    private void Awake()
    {
        TP = GetComponentsInChildren<Button>().Where(b => b.gameObject.name.Contains("TP")).ToArray();
    }

    private void Start()
    {
     /*   
       for (int i = 0; i < TP.Length; i++)
        {
            TP[i].interactable = false;
        }
    */    
    }


}
