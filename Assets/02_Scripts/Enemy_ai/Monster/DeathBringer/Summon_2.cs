using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon_2 : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    bool dmgrepeat;
    public int damage;
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
        dmgrepeat = true;
        Invoke("act", 1f);
    }

    void act()
    {
        animator.SetInteger(animationState, (int)States.atk);
        Invoke("damamgetrue", 1f);
        Destroy(this.gameObject, 3.5f);
    }

    void damagetrue()
    {
        dmgrepeat = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!dmgrepeat)
            {
                dmgrepeat = true;
                Player.instance.Damaged(-damage);
            }
        }
    }
}
