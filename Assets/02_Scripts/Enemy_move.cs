
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_move : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    string animationState = "animationState";
    public int nextMove;

    enum States
    {
        idle = 1,
        move = 2
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        Invoke("Think", 5);
    }

    void Update()
    {
        anim();
        move();
    }


    void Think()
    {
        nextMove = Random.Range(-1, 2);
        if (nextMove != 0)
        {
            spriteRenderer.flipX = (nextMove == 1);
        }
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * (-1);
        spriteRenderer.flipX = (nextMove == 1);


        CancelInvoke();
        Invoke("Think", 2);
    }

    void move()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.8f, rigid.position.y - 0.5f);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    void anim()
    {
        if (rigid.velocity.x < 0 && rigid.velocity.y == 0)
        {
            animator.SetInteger(animationState, (int)States.move);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (rigid.velocity.x > 0 && rigid.velocity.y == 0)
        {
            animator.SetInteger(animationState, (int)States.move);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rigid.velocity.x == 0 && rigid.velocity.y == 0)
        {
            animator.SetInteger(animationState, (int)States.idle);
        }
    }
}
