using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region component
    public float maxSpeed;
    public float jumpPower;

    public float jumpCount;
    private int jump;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 1f;


    [SerializeField] private TrailRenderer tr;


    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 boxSize;
    public ParticleSystem Dust;

    // 플레이어 스테이터스
    public int AtDmg; //공격 데미지
    public int maxHp; //최대 체력 
    public int curHp; //현재 체력

    /*public GameObject hand1;
    public GameObject hand2;*/
    public int i = 0;
    public GameObject AEffect;

    //오디오
    private AudioSource AudioPlayer; //오디오 소스 컴포넌트
    public AudioClip AttackSound;

    public bool IsDashing { get => isDashing; set => isDashing = value; }


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        AudioPlayer = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    #endregion



    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.A))
        {

            transform.localScale = new Vector3(-1f, 1f);
            CreateDust();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localScale = new Vector3(1f, 1f);
            CreateDust();
        }

        if (rigid.velocity.normalized.x == 0)
        {
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
        }


        //Jump
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            CreateDust();
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("Jump", true);
            jumpCount++;
        }

        if (jumpCount == 1)
        {
            jumpPower = 15;
        }
        else
        {
            jumpPower = 20;
        }


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
                anim.SetTrigger("Attack");
                AudioPlayer.PlayOneShot(AttackSound);
                if (i % 2 == 0 && i == 0)
                {
                    AEffect.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.15f);

                }
                else if (i % 2 == 1)
                {
                    AEffect.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.15f);

                }
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Enemy")
                    {
                        Debug.Log(AtDmg + " 로 공격");
                        collider.GetComponent<Enemy>().TakeDamage(AtDmg);
                    }
                }

                curTime = coolTime;
                /*i++;
                if (i == 2)
                {
                    i = 0;
                }*/
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

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
        IsDashing = true;
        maxSpeed = 40;
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rigid.gravityScale = originalGravity;
        IsDashing = false;
        maxSpeed = 8;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void CreateDust()
    {
        Dust.Play();
    }

}
