using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bat : Enemy
{
    public float movespeed;
    public float rushspeed;
    public float position_change_second;
    public float delete_time;

    private float dis;
    public float savespeed;
    private bool traceflag;
    private bool startflag;

    CircleCollider2D circle;
    Rigidbody2D rigid;
    Transform targetTransform = null;
    Vector3 position;
    Animator animator;
    SpriteRenderer sprite;
    Coroutine coroutine;

    string animationState = "animationState";

    enum States
    {
        idle = 0,
        fly = 1
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void start()
    {
        savespeed = movespeed;
        traceflag = startflag = false;
        animator.SetInteger(animationState, (int)States.idle);
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float movespeed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            animator.SetInteger(animationState, (int)States.fly);
            Debug.Log(dis);
            if (targetTransform.position.x < rigid.transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
            if (targetTransform != null)
            {
                position = targetTransform.position;
            }
            if (rigidBodyToMove != null)
            {
                movespeed = savespeed;
                dis = Vector2.Distance(targetTransform.transform.position, rigidBodyToMove.transform.position);
                if (dis < 4f)
                {
                    Debug.Log("2f");
                    movespeed = rushspeed;
                }
                Debug.Log(dis);
                if (traceflag == true)
                {
                    if (targetTransform.position.x < rigid.transform.position.x)
                    {
                        sprite.flipX = true;
                    }
                    else
                    {
                        sprite.flipX = false;
                    }
                    Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, -movespeed * Time.deltaTime);
                    rigid.MovePosition(newposition);
                    remaindistance = (transform.position - position).sqrMagnitude;
                }
                else
                {
                    Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, movespeed * Time.deltaTime);
                    rigid.MovePosition(newposition);
                    remaindistance = (transform.position - position).sqrMagnitude;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("enter");
            targetTransform = collision.gameObject.transform;
            if (coroutine == null)
            {
                Debug.Log("s");
                coroutine = StartCoroutine(Move(rigid, movespeed));
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            traceflag = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("false");
            traceflag = true;
        }
    }
}
