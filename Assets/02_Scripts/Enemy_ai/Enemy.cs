using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    Animator anim;

    public int damage; //�÷��̾����� �� ������
    public int Hp;
    public int dropcoincnt;
    public GameObject Item;
    public GameObject parent;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual void TakeDamage(int damage)
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
        yield return new WaitForSeconds(0.5f); // n�� ����� �ڱ��ڽ� ����
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        Instantiate(Item, this.transform.position, Quaternion.identity).transform.SetParent(parent.transform);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player") //�±װ� �÷��̾��ϰ�� ü�� ���� ó��
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.Damaged(-damage);
        }
    }
}
