using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    public float jumpCount;
    private int jump;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localScale = new Vector3(-1f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localScale = new Vector3(1f, 1f);
        }

        //Jump
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            Debug.Log("점프");
            anim.SetBool("Jump", true);
            jumpCount++;
        }

        if (jumpCount == 1)
        {
            jumpPower = 12;
        }
        else
            jumpPower = 15;



        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash)
        {
            StartCoroutine(Dash());
        }


        //Attack
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (i % 2 == 0 && i == 0)
                {
                    AEffect.gameObject.SetActive(true);

                    hand1.gameObject.SetActive(false);
                    hand2.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.15f);

                }
                else if (i % 2 == 1)
                {
                    AEffect.gameObject.SetActive(true);

                    hand1.gameObject.SetActive(true);
                    hand2.gameObject.SetActive(false);
                    Invoke("HideEffect", 0.15f);

                }
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Enemy")
                    {
                        Debug.Log("Hit");
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

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // Right Max Speed
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // Left Max Speed

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                Debug.Log("취소");
                anim.SetBool("Jump", false);

                if (rayHit.distance < 0.5f)
                {

                    jumpCount = 0;
                }
            }

        }

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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        maxSpeed = 15;
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rigid.gravityScale = originalGravity;
        isDashing = false;
        maxSpeed = 4;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
