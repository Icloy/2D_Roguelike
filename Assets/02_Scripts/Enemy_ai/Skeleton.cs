
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    BoxCollider2D box;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Coroutine coroutine;

    string animationState = "animationState";
    private float nextMove;
    private bool trace;
    public float movespeed;
    public float tracespeed;
    public float turnrange;
    public GameObject Player;

    enum States
    {
        idle = 0,
        walk = 1
    }
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        box = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        coroutine = StartCoroutine(move());
    }

    void Start()
    {
        nextMove = 1;
    }

    void Update()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0)); // 디버그용 두문장 나중에 지워도 상관없음
    }

    void Turn()//턴
    {
        nextMove = nextMove * (-1);
    }

    public IEnumerator move()
    {
        while (true)
        {
            animator.SetInteger(animationState, (int)States.walk);
            if (rigid.velocity.x > 0)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            if (trace == true)
            {
                if (Player.transform.position.x < rigid.transform.position.x)
                {
                    rigid.velocity = new Vector2(-1 * tracespeed, rigid.velocity.y);
                }
                else
                {
                    rigid.velocity = new Vector2(1 * tracespeed, rigid.velocity.y);
                }
            }
            else
            {
                Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
                Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
                RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
                if (rayHit.collider == null)
                {
                    Turn();
                }
                rigid.velocity = new Vector2(nextMove * movespeed, rigid.velocity.y);
            }
            yield return new WaitForSeconds(0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            trace = true;
        }
        Debug.Log("enter");
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            trace = false;
        }
        Debug.Log("exit");
    }
}