using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public GameObject hpbarcanvas;
    public GameObject prfhpbar;
    Animator anim;

    RectTransform hpbar;

    public int damage = 100; //플레이어한테 줄 데미지
    public int Hp = 5;
    public int dropcoincnt;
    public GameObject Item;
    public GameObject parent;

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    void Start()
    {
        hpbar = Instantiate(prfhpbar, hpbarcanvas.transform).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1.5f, 0));
        hpbar.position = _hpBarPos;
       
    }

    public void TakeDamage(int damage)
    {
        Hp = Hp - damage;
        if (Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        StopAllCoroutines();
        anim.SetTrigger("Death");
        for (int i = 0; i < dropcoincnt; i++)
        {
            DropItem();
        }
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(0.5f); // n초 대기후 자기자신 제거
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        Instantiate(Item, this.transform.position, Quaternion.identity).transform.SetParent(parent.transform);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player") //태그가 플레이어일경우 체력 감소 처리
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.Damaged(damage);
        }
    }
}
