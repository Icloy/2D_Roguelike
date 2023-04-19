using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBound : MonoBehaviour
{
    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") || collision.gameObject.CompareTag("Enemy"))
        {
            rigid.velocity = Vector2.zero;
            Player.Instance.upforce();
        }
    }



}
