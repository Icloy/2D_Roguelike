using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyingeye : MonoBehaviour
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
        flight = 0,
        boom = 1,
        hit = 2,
        fall = 3,
        die = 4
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        animator.SetInteger(animationState, (int)States.flight);
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
            if (targetTransform.position.x > rigid.transform.position.x)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
                animator.SetInteger(animationState, (int)States.flight);
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
                animator.SetInteger(animationState, (int)States.flight);
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
