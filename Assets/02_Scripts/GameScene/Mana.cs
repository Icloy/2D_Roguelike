using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    private Image content;

    private float currentFill;


    [SerializeField] private float IerpSpeed;

    public float MyMaxValue { get; set; }

    // 마나 현재값 설정
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MyMaxValue) currentValue = MyMaxValue;
            else if (value < 0) currentValue = 0;
            else currentValue = value;

            currentFill = currentValue / MyMaxValue;
        }
    }

    private float currentValue;

    void Start()
    {
        content = GetComponent<Image>();
    }

    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * IerpSpeed);

        }
    }

    public void Initialized(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
   
}
