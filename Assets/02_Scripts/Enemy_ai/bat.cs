using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bat : MonoBehaviour
{
    public float speed;
    public float position_change_second;
    public float delete_time;

    private bool flag = false;

    CircleCollider2D circle;
    Rigidbody2D rigid;
    Transform targetTransform = null;
    Vector3 position;
    Animator animator;
    SpriteRenderer sprite;

    string animationState = "animationState";

    enum States
    {
        idle = 0,
        fly = 1
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        animator.SetInteger(animationState, (int)States.idle);
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            animator.SetInteger(animationState, (int)States.fly);
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
                if(flag == true)
                {
                    Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, -speed * Time.deltaTime);
                    rigid.MovePosition(newposition);
                    remaindistance = (transform.position - position).sqrMagnitude;
                }
                else
                {
                    Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, speed * Time.deltaTime);
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
            targetTransform = collision.gameObject.transform;
            StartCoroutine(Move(rigid, speed));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            flag = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            flag = true;
            //Destroy(this.gameObject);
        }
    }
}
