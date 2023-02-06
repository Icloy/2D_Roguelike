using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public GameObject hpbarcanvas;
    public GameObject prfhpbar;

    RectTransform hpbar;

    public int Hp = 5;

    void Start()
    {
        hpbar = Instantiate(prfhpbar, hpbarcanvas.transform).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Hp == 0 && Hp <0)
        {
            Destroy(this);
        }

        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1.5f, 0));
        hpbar.position = _hpBarPos;
    }

    public void TakeDamage(int damage)
    {
        Hp = Hp - damage;
    }

}
