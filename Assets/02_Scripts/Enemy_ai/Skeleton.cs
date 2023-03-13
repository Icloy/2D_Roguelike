
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    BoxCollider2D box;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Coroutine coroutine;

    string animationState = "animationState";
    private bool trace;
    private int nextMove;

    public float movespeed;
    public float tracespeed;
    public float turnrange;
    public Transform Player;

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
        Invoke("Think", 5);
    }

    void Start()
    {
        animator.SetInteger(animationState, (int)States.idle);
    }

    void Update()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0)); // 디버그용 두문장 나중에 지워도 상관없음
    }

    void Turn()//턴
    {
        nextMove = nextMove * (-1);
        CancelInvoke();
        Invoke("Think", 3);
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }
    public IEnumerator move()
    {
        while (true)
        {
            if (nextMove == 1)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
                animator.SetInteger(animationState, (int)States.walk);
            }
            else if (nextMove == -1)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
                animator.SetInteger(animationState, (int)States.walk);
            }
            else if (nextMove == 0)
            {
                animator.SetInteger(animationState, (int)States.idle);
            }
            if (trace == true)
            {
                if (Player.transform.position.x < rigid.transform.position.x)
                {
                    rigid.velocity = new Vector2(-1 * tracespeed, rigid.velocity.y);
                    GetComponentInChildren<SpriteRenderer>().flipX = true;
                    animator.SetInteger(animationState, (int)States.walk);
                }
                else
                {
                    rigid.velocity = new Vector2(1 * tracespeed, rigid.velocity.y);
                    GetComponentInChildren<SpriteRenderer>().flipX = false;
                    animator.SetInteger(animationState, (int)States.walk);
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

    public override void TakeDamage(int AtDmg)
    {
        Hp = Hp - AtDmg;
        Debug.Log(Hp);
        if (Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        DropItem();
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        Debug.Log("호출");
        for(int i = 0;i < dropcoincnt; i++)
        {
            float x = Random.Range(-1f, 1f); // x축 위치 랜덤 설정
            float y = Random.Range(-1f, 1f); // y축 위치 랜덤 설정
            Vector2 position = new Vector2(transform.position.x + x, transform.position.y + y);
            Instantiate(Item, position, Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.Damaged(-damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player = collision.gameObject.transform;
            trace = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            trace = false;
        }
    }
}