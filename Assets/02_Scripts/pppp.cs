using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pppp : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 boxSize;
    public GameObject hand1;
    public GameObject hand2;
    public int i = 0;
    public GameObject AEffect;

    /*//mouselook
     float angle;
     Vector2 target, mouse;


     private void Start()
     {
         target = transform.position;
     }*/

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        //Jump
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        //Dash
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Dash", true);
        }
        else
        {
            anim.SetBool("Dash", false);

        }
        */
        
        //Attack
        if(curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if(i % 2 == 0 && i == 0)
                {
                    AEffect.gameObject.SetActive(true);

                    hand1.gameObject.SetActive(false);
                    hand2.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.15f);

                }
                else if(i % 2 == 1)
                {
                    AEffect.gameObject.SetActive(true);

                    hand1.gameObject.SetActive(true);
                    hand2.gameObject.SetActive(false);
                    Invoke("HideEffect", 0.15f);

                }
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().TakeDamage(1);
                    }
                }
                
                curTime = coolTime;
                i++;
                if (i == 2)
                {
                    i = 0;
                }
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        /*//mouselook
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);*/
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

    }

    private void OnDrawGizmos() //공격박스표시
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void HideEffect()
    {
        AEffect.gameObject.SetActive(false);
    }
}
