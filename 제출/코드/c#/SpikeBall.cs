using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    BoxCollider2D box;
    Rigidbody2D rigid;

    public int SpikeBallDmg;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponentInChildren<BoxCollider2D>();
    }

    void Start()
    {
        rigid.isKinematic = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Player.instance.Damaged(-SpikeBallDmg);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigid.isKinematic = false;
            Destroy(this.gameObject, 3f);
        }
    }
}
