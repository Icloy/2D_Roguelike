using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    CircleCollider2D circle;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator animator;

    public int actmove;
    public float movespeed;
    public GameObject Player;
    public GameObject Boss;
    public GameObject trap;
    public GameObject drop;

    private float dis;
    private int direction;
    private int dropcnt;
    private int dropran;
    private bool flag;
    string animationState = "animationState";

    enum States
    {
        idle = 1,
        attack = 4,
        cast = 6,
        disappear = 7,
        appears = 8
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        flag = true;
    }

    void Update()
    {
    }

    public IEnumerator Think()
    {
        while (true)
        {
            animator.SetInteger(animationState, (int)States.idle);
            dis = Vector2.Distance(Player.transform.position, Boss.transform.position);
            Vector2 player = Player.transform.position;
            if (player.x < Boss.transform.position.x)
            {
                direction = 1;
                transform.localScale = new Vector3(4, 5, 1);
            }
            else
            {
                direction = 2;
                transform.localScale = new Vector3(-4, 5, 1);
            }
            actmove = Random.Range(1, 3);
            Debug.Log(dis);
            Debug.Log(direction);
            Debug.Log(actmove);
            if (dis <= 2.5)
            {
                StartCoroutine(act1(actmove));
            }
            else if (dis <= 5)
            {
                StartCoroutine(act2(actmove));
            }
            else
            {
                StartCoroutine(act3(actmove));
            }
            yield return new WaitForSeconds(4f);
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
                        transform.localScale = new Vector3(4, 5, 1);
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        rigid.isKinematic = false;
                        animator.SetInteger(animationState, (int)States.idle);
                        rigid.AddForce(Vector2.down * 30f, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(Player.transform.position.x + -1.5f, Player.transform.position.y + 5f, Player.transform.position.z);
                        transform.localScale = new Vector3(-4, 5, 1);
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
                        transform.localScale = new Vector3(4, 5, 1);
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.attack);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.idle);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(Player.transform.position.x + -1.5f, Player.transform.position.y-0.5f, Player.transform.position.z);
                        transform.localScale = new Vector3(-4, 5, 1);
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
        animator.SetInteger(animationState, (int)States.cast);
        yield return new WaitForSeconds(2.5f);
        switch (actmove)
        {
            case 1:
                Debug.Log("act3_1");
                for (int i = -2; i < 3; i++)
                {
                    Instantiate(trap, new Vector3(Player.transform.position.x + 1f * i, Player.transform.position.y + 4f, Player.transform.position.z), Quaternion.identity);
                }
                break;
            case 2:
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (flag == true)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(Think());
                flag = false;
            }
        }

    }

}