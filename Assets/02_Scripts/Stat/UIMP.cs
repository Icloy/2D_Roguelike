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
    [SerializeField]
    private Slider sliderHP;

    // Update is called once per frame
    private void Update()
    {
        if (sliderHP != null) sliderHP.value = Utils.Percent(stat.HP, stat.MaxHP);


        if (sliderMP != null) sliderMP.value = Utils.Percent(stat.MP, stat.MaxMP);
    }
}
