using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move_Jump : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    private int nextMove;
    public float jumpPower;
    public float movespeed;
    public float turnrange;
    public float jumptime;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextMove = 1;
        StartCoroutine(move());
    }

    void Update()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y - 0.5f);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
    }

    void Turn()//방향전환
    {
        nextMove = nextMove * (-1);
        spriteRenderer.flipX = (nextMove == 1);
    }

    public IEnumerator move()//점프이동
    {
        while(true)
        {
            if(rigid.velocity.y == 0)
            {
                Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y - 0.5f);
                Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
                if (rayHit.collider == null)
                {
                    Turn();
                }
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                rigid.velocity = new Vector2(nextMove * movespeed, rigid.velocity.y);
            }
            yield return new WaitForSeconds(jumptime);
        }
        


    }
}
