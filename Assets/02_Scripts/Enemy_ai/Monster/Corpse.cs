using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (col.gameObject.tag == "Platform")
        {
            rigid.velocity = Vector2.zero;
            rigid.simulated = false;
        }
    }
}
