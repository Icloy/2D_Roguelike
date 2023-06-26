using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Corpse : MonoBehaviour
{
    Rigidbody2D rigid;
    public float knockbackdis;
    Transform PlayerPos;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
        KnockBack();
    }

    void KnockBack()
    {
        rigid.AddForce(Vector2.up * knockbackdis, ForceMode2D.Impulse);
        if (PlayerPos.transform.position.x < rigid.transform.position.x)
        {
            rigid.AddForce(Vector2.right * knockbackdis, ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector2.left * knockbackdis, ForceMode2D.Impulse);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform" || col.gameObject.GetComponent<TilemapCollider2D>() != null)
        {
            rigid.velocity = Vector2.zero;
            rigid.simulated = false;
        }
        else if (!(col.gameObject.GetComponent<TilemapCollider2D>() != null))
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
    }
}
