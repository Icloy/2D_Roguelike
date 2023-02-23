using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMP : MonoBehaviour
{
    [SerializeField]
    private Stat stat;
    [SerializeField]
    private Slider sliderMP;

    // Update is called once per frame
    private void Update()
    {
        if (sliderMP != null) sliderMP.value = Utils.Percent(stat.MP, stat.MaxMP);
    }
}
