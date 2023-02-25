using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon_2 : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    string animationState = "animationState";

    enum States
    {
        idle = 1,
        atk = 2
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger(animationState, (int)States.idle);
        Invoke("act", 1f);
    }

    void act()
    {
        animator.SetInteger(animationState, (int)States.atk);
        Destroy(this.gameObject, 3.5f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Platform"))
        {
            
        }
    }
}
