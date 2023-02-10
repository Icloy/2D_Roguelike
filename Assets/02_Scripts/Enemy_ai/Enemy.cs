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

    public int Hp = 5;
    public GameObject Item;

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
        if (Hp == 0)
        {
            Die();
        }
    }

    void Die()
    {
        StopAllCoroutines();
        anim.SetTrigger("Death");
        DropItem();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(0.5f); // n초 대기후 자기자신 제거
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        Instantiate(Item, this.transform.position, Quaternion.identity);
    }
}
