using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region component
    private float horizontal;
    public float maxSpeed;
    public float jumpingPower = 25f;
    private bool doubleJump;
    private float doubleJumpingPower = 20f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private bool isWallSliding;
    private float wallSlidingSpeed = 3f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 1f;

    private bool canHeal = true;
    private bool isHeal;
    private float HealI = 0f;


    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;


    SpriteRenderer spriteRenderer;
    Animator anim;

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 boxSize;


    // 플레이어 스테이터스
    public int AtDmg; //공격 데미지
    public int maxHp; //최대 체력 
    public int curHp; //현재 체력
    public GameObject Stat;

    /*public GameObject hand1;
    public GameObject hand2;*/
    private int i = 0;
    public GameObject AEffect;

    //오디오
    private AudioSource AudioPlayer; //오디오 소스 컴포넌트
    public AudioClip AttackSound;

    public bool IsDashing { get => isDashing; set => isDashing = value; }

    GameManager gameManager;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        AudioPlayer = GetComponent<AudioSource>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }
    // Update is called once per frame
    #endregion




    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(-1f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(1f, 1f);
        }

        if (rigid.velocity.normalized.x == 0)
            anim.SetBool("Run", false);
        else
            anim.SetBool("Run", true);

        //Jump

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f || doubleJump)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);
                doubleJump = !doubleJump;
                if (jumpBufferCounter > 0f)
                {
                    jumpBufferCounter = 0f;
                }
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }

        if (!IsGrounded())
        {
            coyoteTimeCounter -= Time.deltaTime;
            anim.SetBool("Jump", true);
        }
        else
        {
            coyoteTimeCounter = coyoteTime;
            anim.SetBool("Jump", false);
        }

        WallSlide();
        WallJump();



        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.W) && canDash)
        {   
                StartCoroutine(Dash());
        }

        //Attack
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Q) && !gameManager.isPanelOpen)
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
                        Stat.GetComponent<Stat>().MP += 10;
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

        if (Input.GetKeyDown(KeyCode.A) && canHeal == true)
        {
            StartCoroutine(Heal());         
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Stat.GetComponent<Stat>().MP += 50;
        }




    }

    void FixedUpdate()
    {

        rigid.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // Right Max Speed
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // Left Max Speed

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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Clamp(rigid.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rigid.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;


        }
    }


    private void StopWallJumping()
    {
        isWallJumping = false;
    }



    private IEnumerator Heal()
    {
        if(Stat.GetComponent<Stat>().MP < 100)
        {
            Debug.Log("마나부족");
        }
        else
        {
            canHeal = false;
            isHeal = true;
            maxSpeed = 0;
            yield return new WaitForSeconds(3f);
            canHeal = true;
            isHeal = false;
            curHp += 100;
            Stat.GetComponent<Stat>().MP -= 100;
            maxSpeed = 8;
        }

    }

 

  
}
