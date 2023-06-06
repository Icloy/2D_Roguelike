using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_BigHead : Enemy
{
    CircleCollider2D circle;
    Rigidbody2D rigid;
    float MaxHp;

    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;
    public GameObject hand4;
    public GameObject hand5;
    public GameObject hand6;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        HpBar = GetComponentInChildren<Canvas>();
    }

    private void OnEnable()
    {
        MaxHp = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void TakeDamage(int AtDmg)
    {
        Hp = Hp - AtDmg;
        HpFill.fillAmount = Hp / MaxHp;
        Debug.Log(Hp);
        if(Hp < 20)
        {
            hand1.gameObject.SetActive(true);
        }
        if (Hp < 18)
        {
            hand2.gameObject.SetActive(true);
        }
        if (Hp < 16)
        {
            hand3.gameObject.SetActive(true);
        }
        if (Hp < 14)
        {
            hand4.gameObject.SetActive(true);
        }
        if (Hp < 12)
        {
            hand5.gameObject.SetActive(true);
        }
        if (Hp < 10)
        {
            hand6.gameObject.SetActive(true);
        }

        if (Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
}
