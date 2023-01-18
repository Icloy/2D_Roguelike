using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동속도 조절")]
    [SerializeField]
    [Range(1f, 5f)]
    private float moveSpeed = 1.0f;
    [SerializeField]
    [Range(1f, 10f)]
    private float jumpSpeed = 5.0f;

    Rigidbody2D rigid;
    /*Animator animator;
    string animationState = "animationState";

    enum States
    {
        idle = 1,
        move = 2,
        jump = 3,
        fall = 4,
        attack = 5,
        croush = 6,
        heavyattack = 7
    }*/
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        /*animator = GetComponent<Animator>();*/
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        /*anim();*/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        
    }

    void Move()
    {
        Vector3 movePosition = Vector3.zero;

        // 왼쪽으로 움직임
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            movePosition = Vector3.left;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        // 오른쪽으로 움직임
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            movePosition = Vector3.right;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        transform.position += movePosition * moveSpeed * Time.deltaTime;
    }

    /*void anim()
    {
        if (rigid.velocity.y > 0)
        {
            animator.SetInteger(animationState, (int)States.jump);
        }
        else if (rigid.velocity.y < 0)
        {
            animator.SetInteger(animationState, (int)States.fall);
        }
        else if (Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetInteger(animationState, (int)States.move);
        }
        else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetInteger(animationState, (int)States.heavyattack);
        }
        else if (Input.GetMouseButton(0))
        {
            animator.SetInteger(animationState, (int)States.attack);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetInteger(animationState, (int)States.croush);
        }
        else if (rigid.velocity.x == 0 && rigid.velocity.y == 0)
        {
            animator.SetInteger(animationState, (int)States.idle);
        }
    }
    */
}