using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_hpbar : MonoBehaviour
{
    public Slider slider;
    public int nowhp;
    GameObject obj;

    void Start()
    {
        obj = GameObject.Find("TestPlayer");
        nowhp = obj.GetComponent<Enemy>().Hp;
        setmaxhp(nowhp);
    }

    void Update()
    {
        nowhp = obj.GetComponent<Enemy>().Hp;
        sethp(nowhp);
        
    }
    public void setmaxhp(int hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
    }

    public void sethp(int hp)
    {
        slider.value = hp;
    }
}
