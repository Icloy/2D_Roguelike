using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    BoxCollider2D box;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Coroutine coroutine;
    string animationState = "animationState";
    private float dis;
    private float nextMove;
    private int direction;
    private int actmove;
    public float movespeed;
    public float jumpPower;
    public float turnrange;
    public GameObject Player;
    public GameObject slime;

    enum States
    {
        walk = 1,
        jump = 2,
        spin = 3
    }
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        box = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        nextMove = 1;
        coroutine = StartCoroutine(move());
    }

    void Update()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
    }

    void Turn()//방향전환
    {
        nextMove = nextMove * (-1);
    }

    public IEnumerator move()
    {
        while(true)
        {
            if (rigid.velocity.x > 0)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            animator.SetInteger(animationState, (int)States.walk);
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null)
            {
                Turn();
            }
            rigid.velocity = new Vector2(nextMove * movespeed, rigid.velocity.y);
        yield return new WaitForSeconds(0f);
        }
    }

    public IEnumerator attack()
    {
        while (true)
        {
            Debug.Log("attack");
            dis = Vector2.Distance(Player.transform.position, slime.transform.position);
            if (Player.transform.position.x < slime.transform.position.x)
            {
                direction = 1;
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                direction = 2;
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            actmove = Random.Range(1, 3);
            switch (actmove)
            {
                case 1:
                    Debug.Log("case1");
                    if (direction == 1)
                    {
                        rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.5f);
                        animator.SetInteger(animationState, (int)States.jump);
                        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                        rigid.AddForce(Vector2.left * dis * 1.2f, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.9f);
                        animator.SetInteger(animationState, (int)States.walk);
                    }
                    else
                    {
                        rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.5f);
                        animator.SetInteger(animationState, (int)States.jump);
                        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                        rigid.AddForce(Vector2.right * dis * 1.2f, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.9f);
                        animator.SetInteger(animationState, (int)States.walk);
                    }
                    break;
                case 2:
                    Debug.Log("case2");
                    if (direction == 1)
                    {
                        rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.5f);
                        animator.SetInteger(animationState, (int)States.spin);
                        rigid.AddForce(Vector2.left * dis * 3, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.walk);
                    }
                    else
                    {
                        rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.5f);
                        animator.SetInteger(animationState, (int)States.spin);
                        rigid.AddForce(Vector2.right * dis * 3, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.walk);
                    }
                    break;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(attack());
        }
        Debug.Log("enter");
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(move());
        }
        Debug.Log("exit");
    }

}
