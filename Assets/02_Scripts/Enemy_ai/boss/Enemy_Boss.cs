using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    CircleCollider2D circle;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator animator;

    public float jumpPower;
    public GameObject Player;
    public GameObject Boss;
    public GameObject trap;
    public GameObject drop;

    private int actmove;
    private int direction;
    private int dropcnt;
    private int dropran;
    string animationState = "animationState";

    public GameObject Attack1_check;
    public GameObject Attack2_check;

    enum States
    {
        idle = 1,
        attack = 4,
        cast = 6,
        disappear = 7,
        appears = 8,
        throwback = 9
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponentInChildren<CircleCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        Player = GameObject.FindWithTag("Player");
        StartCoroutine(Think());
    }

    void Update()
    {
    }

    public IEnumerator Think()
    {
        while (true)
        {
            animator.SetInteger(animationState, (int)States.idle);
            float dis = Vector2.Distance(Player.transform.position, Boss.transform.position);
            Vector2 player = Player.transform.position;
            if (player.x < Boss.transform.position.x)
            {
                direction = 1;
                FlipBack();
            }
            else
            {
                direction = 2;
                FlipX();
            }
            actmove = Random.Range(1, 3);
            Debug.Log(dis);
            Debug.Log(direction);
            Debug.Log(actmove);
            if (dis <= 2.5)
            {
                yield return StartCoroutine(act1(actmove));
            }
            else if (dis <= 5)
            {
                yield return StartCoroutine(act2(actmove));
            }
            else
            {
                yield return StartCoroutine(act3(actmove));
            }
            yield return new WaitForSeconds(0f);
        }
        
    }

    public IEnumerator act1(int actmove)
    {
        Debug.Log("act1");
        switch (actmove)
        {
            case 1:
                Debug.Log("act1_1");
                animator.SetInteger(animationState, (int)States.attack);
                yield return new WaitForSeconds(1f);
                animator.SetInteger(animationState, (int)States.idle);
                break;
            case 2:
                Debug.Log("act1_2");
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                switch (direction)
                {
                    case 1:
                        rigid.velocity = new Vector2(1 * movespeed, rigid.velocity.y);
                        break;
                    case 2:
                        rigid.velocity = new Vector2(-1 * movespeed, rigid.velocity.y);
                        break;
                }
                break;
        }

    }

    public IEnumerator act2(int actmove)
    {
        Debug.Log("act2");
        switch (actmove)
        {
            case 1:
                Debug.Log("act2_1");
                animator.SetInteger(animationState, (int)States.disappear);
                yield return new WaitForSeconds(1f);
                int j = Random.Range(1, 3);
                rigid.isKinematic = true;
                switch (j)
                {
                    case 1:
                        gameObject.transform.position = new Vector3(Player.transform.position.x + 1.5f, Player.transform.position.y + 5f, Player.transform.position.z);
                        FlipBack();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        rigid.isKinematic = false;
                        animator.SetInteger(animationState, (int)States.idle);
                        rigid.AddForce(Vector2.down * 30f, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(Player.transform.position.x + -1.5f, Player.transform.position.y + 5f, Player.transform.position.z);
                        FlipX();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        rigid.isKinematic = false;
                        animator.SetInteger(animationState, (int)States.idle);
                        rigid.AddForce(Vector2.down * 30f, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        break;
                }
                break;
            case 2:
                Debug.Log("act2_1");
                animator.SetInteger(animationState, (int)States.disappear);
                yield return new WaitForSeconds(1f);
                int i = Random.Range(1, 3);
                switch (i)
                {
                    case 1:
                        gameObject.transform.position = new Vector3(Player.transform.position.x + 1.5f, Player.transform.position.y-0.5f, Player.transform.position.z);
                        FlipBack();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.attack);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.idle);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(Player.transform.position.x + -1.5f, Player.transform.position.y-0.5f, Player.transform.position.z);
                        FlipX();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.attack);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.idle);
                        break;
                }
                break;
        }
    }

    public IEnumerator act3(int actmove)
    {
        Debug.Log("act3");
        switch (actmove)
        {
            case 1:
                animator.SetInteger(animationState, (int)States.throwback);
                yield return new WaitForSeconds(0.85f);
                Debug.Log("act3_1");
                if(direction == 1)
                {
                        Instantiate(trap, new Vector3(Boss.transform.position.x + -2f, Boss.transform.position.y + 1f, Boss.transform.position.z), Quaternion.identity);
                }
                else
                {
                        Instantiate(trap, new Vector3(Boss.transform.position.x + 2f, Boss.transform.position.y + 1f, Boss.transform.position.z), Quaternion.identity);
                }
                yield return new WaitForSeconds(1.65f);

                break;
            case 2:
                animator.SetInteger(animationState, (int)States.cast);
                yield return new WaitForSeconds(2.5f);
                Debug.Log("act3_2");
                dropcnt = Random.Range(-3, 5);
                for (int i = -3; i <= dropcnt; i++)
                {
                    dropran = Random.Range(1, 6);
                    if (dropran <= 4)
                    {
                        Instantiate(drop, new Vector3(Player.transform.position.x + 1f * i, Player.transform.position.y + 2.5f, Player.transform.position.z), Quaternion.identity);
                    }
                }
                break;
        }
        animator.SetInteger(animationState, (int)States.idle);
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
        for (int i = 0; i < dropcoincnt; i++)
        {
            float x = Random.Range(-1f, 1f); // x축 위치 랜덤 설정
            float y = Random.Range(0f, 1f); // y축 위치 랜덤 설정
            Vector2 position = new Vector2(transform.position.x + x, transform.position.y + y);
            Instantiate(Item, position, Quaternion.identity);
        }
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-4, 5, 1);
    }

    void FlipBack()
    {
        transform.localScale = new Vector3(4, 5, 1);
    }
}