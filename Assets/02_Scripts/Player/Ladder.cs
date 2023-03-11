using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    Animator anim;


    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if(isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);

        }
        else
        {
            rb.gravityScale = 4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            anim.SetBool("Jump", false);
            anim.SetBool("Ladder", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            anim.SetBool("Ladder", false);
            isLadder = false;
            isClimbing = false;
        }
    }
}

