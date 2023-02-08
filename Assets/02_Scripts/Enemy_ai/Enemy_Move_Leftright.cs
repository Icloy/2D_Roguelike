using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move_Leftright : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    private int nextMove;
    public float movespeed;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextMove = 1;
        StartCoroutine(move());
    }

    void Turn()
    {
        nextMove = nextMove * (-1);
    }

    public IEnumerator move()
    {
        while(true)
        {
            rigid.velocity = new Vector2(nextMove * movespeed, rigid.velocity.y);
            yield return new WaitForSeconds(0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Turn();
        }
    }
}
