using System;
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
    public int jumpLeft;

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
    private Vector2 wallJumpingPower = new Vector2(16f, 32f);

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 100f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 1f;

    SpriteRenderer spriteRenderer;
    Animator anim;

    private bool _IsGrounded;
    private Transform _transform;

    private float Laddervertical;
    private float Ladderspeed = 8f;
    private bool isLadder;
    private bool isClimbing;


      [SerializeField] private Rigidbody2D rigid;
      [SerializeField] private Transform groundCheck;
      [SerializeField] private LayerMask groundLayer;
      [SerializeField] private Transform wallCheck;
      [SerializeField] private LayerMask wallLayer;
      [SerializeField] private TrailRenderer tr;

    private float curTime;
    public float coolTime = 0.5f;
    [HideInInspector]  public Transform pos;

    bool zoomIn;

    
    [Range(2, 100)]
    public float zoomSize;

    [HideInInspector]
    [Range(0.01f, 0.1f)]
    public float zoomSpeed;

    //힐 쿨타임
    float hcurT;
    float hgoalT = 1.5f;

    // 플레이어 스테이터스
    public int AtDmg; //공격 데미지
    public int maxHp; //최대 체력 
    public int curHp; //현재 체력

    [HideInInspector]  public GameObject Stat;
    private int i = 0;
    [HideInInspector] public GameObject AEffect;
    [HideInInspector] public GameObject AEffect_Up;
    [HideInInspector] public GameObject AEffect_Down;
    Camera cam;
    public bool fadeInOut;
    public bool SmoothMoving;

    [SerializeField]
    [HideInInspector] float lowpassValue = 100;

    [HideInInspector]  public GameObject HealEffect;
    [HideInInspector]  public AudioClip AttackSound;
    [HideInInspector]  public AudioClip HealSound;
    [HideInInspector]  public AudioClip DashSound;
    [HideInInspector] public AudioClip DamagedSound;

    //오디오
    private AudioSource AudioPlayer; //오디오 소스 컴포넌트
    private AudioLowPassFilter audioLowPassFilter;

    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public bool IsWallJumping { get => isWallJumping; set => isWallJumping = value; }

    public static Player instance = null;

    public static Player Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        AudioPlayer = GetComponent<AudioSource>();
        cam = Camera.main;
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
    }
    // Update is called once per frame
    #endregion


    private void Start()
    {
        Hp.instance.udtHp(curHp, maxHp);
    }

    void Update()
    {
        updatePlayerState();

        #region Move

        horizontal = Input.GetAxisRaw("Horizontal");
        float moveDirection = transform.localScale.x * horizontal;



        if (Input.GetKey(KeyCode.LeftArrow)&&!GameManager.instance.isPanelOpen)
        {
            transform.localScale = new Vector3(-1f, 1f);
            anim.SetBool("IsRun", true);

        }
        else if (Input.GetKey(KeyCode.RightArrow)&& !GameManager.instance.isPanelOpen)
        {
            transform.localScale = new Vector3(1f, 1f);
            anim.SetBool("IsRun", true);

        }
            if (rigid.velocity.normalized.x == 0)
        {
            anim.SetTrigger("StopRun");
            anim.SetBool("IsRun", false);
        }
        else
        {
            anim.ResetTrigger("StopRun");
        }

        #endregion

        #region Jump

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        if (Input.GetButtonDown("Jump") && !GameManager.instance.isShopOpen)
        {
            jumpLeft -= 1;
            if (jumpLeft == 1)
            {
                anim.SetTrigger("IsJumpFirst");
            }
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

        if (!IsGrounded() && !IsWalled() && isLadder == false)
        {
        anim.SetBool("IsJump", true);
            coyoteTimeCounter -= Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = coyoteTime;
            anim.SetBool("IsJump", false);
        }

        WallSlide();
        WallJump();



        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.W) && canDash &&(!isWallSliding))
        {
            StartCoroutine(Dash());
        }

        #endregion


        #region Attack

        if (curTime <= 0)
        {
           
            if(Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Q) && !GameManager.instance.isPanelOpen && !isWallSliding)
            {
                anim.SetTrigger("IsAttackUp");
                AudioPlayer.PlayOneShot(AttackSound);
                if (i % 2 == 0 && i == 0)
                {
                    AEffect_Up.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.1f);

                }
                else if (i % 2 == 1)
                {
                    AEffect_Up.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.1f);

                }

                curTime = coolTime;
                i++;
                if (i == 2)
                {
                    i = 0;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Q) && !GameManager.instance.isPanelOpen && !isWallSliding)
            {
                anim.SetTrigger("IsAttackDown");
                AudioPlayer.PlayOneShot(AttackSound);
                if (i % 2 == 0 && i == 0)
                {
                    AEffect_Down.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.1f);

                }
                else if (i % 2 == 1)
                {
                    AEffect_Down.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.1f);

                }

                curTime = coolTime;
                i++;
                if (i == 2)
                {
                    i = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q) && !GameManager.instance.isPanelOpen && !isWallSliding)
            {
                anim.SetTrigger("IsAttack");
                AudioPlayer.PlayOneShot(AttackSound);
                if (i % 2 == 0 && i == 0)
                {
                    AEffect.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.1f);

                }
                else if (i % 2 == 1)
                {
                    AEffect.gameObject.SetActive(true);
                    Invoke("HideEffect", 0.1f);

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




        if (Input.GetKeyDown(KeyCode.S))
        {
            Stat.GetComponent<Stat>().MP += 50;
        }
        #endregion

        #region Heal


        //Down부분에 이럴 경우 코루틴이 시작되면 안된다 하는 경우의 수 추가 
        if (Input.GetKeyDown(KeyCode.A) && curHp < maxHp && IsGrounded() && Stat.GetComponent<Stat>().MP >= 100)
        {
            StartCoroutine("Heal");
        }
        else if (Input.GetKeyDown(KeyCode.A) && maxHp == curHp)
        {
            ToastMsg.Instance.showMessage("회복할 체력이 없습니다!", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Stat.GetComponent<Stat>().MP < 100)
        {
            ToastMsg.Instance.showMessage("마나가 부족합니다!", 0.5f);

        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            StopCoroutine("Heal");
            hcurT = 0f;
            rigid.constraints = RigidbodyConstraints2D.None;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            ZoomOut();
            canDash = true;
            anim.SetBool("Sit", false);
            HealEffect.gameObject.SetActive(false);

        }
        #endregion


        /*#region Ladder

        Laddervertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(Laddervertical) > 0f)
        {
            isClimbing = true;
        }

        if (isLadder && isClimbing)
        {
            anim.SetBool("Ladder", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Run", false);
        }

        #endregion*/

    }

    void FixedUpdate()
    {

        rigid.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // Right Max Speed
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // Left Max Speed

        if (isClimbing)
        {
            rigid.gravityScale = 0f;
            rigid.velocity = new Vector2(rigid.velocity.x, Laddervertical * Ladderspeed);

        }
        else
        {
            rigid.gravityScale = 4f;
        }
    }



    private void HideEffect()
    {
        AEffect.gameObject.SetActive(false);
        AEffect_Up.gameObject.SetActive(false);
        AEffect_Down.gameObject.SetActive(false);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        IsDashing = true;
        maxSpeed = 80;
        DashAnim();
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        AudioPlayer.PlayOneShot(DashSound);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rigid.gravityScale = originalGravity;
        IsDashing = false;
        maxSpeed = 8;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }



    #region Check
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
            anim.SetBool("IsClimb", true);

            isWallSliding = true;
            rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Clamp(rigid.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            anim.SetBool("IsClimb", false);
            isWallSliding = false;
        }

    
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            IsWallJumping = false;
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
            IsWallJumping = true;
            rigid.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;


        }
    }


    private void StopWallJumping()
    {
        IsWallJumping = false;
    }

    #endregion

    IEnumerator Heal()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                //누루는 동안 제한해야하는것들
                hcurT += Time.deltaTime;
                canDash = false;
                anim.SetBool("Idle", false);
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;
                ZoomIn();
                HealEffect.gameObject.SetActive(true);
                anim.SetBool("Sit", true);

                if (hgoalT <= hcurT)
                {
                    //힐 구현부
                    AudioPlayer.PlayOneShot(HealSound);
                    HealEffect.gameObject.SetActive(false);
                    ZoomOut();
                    StartCoroutine(StageMgr.Instance.MoveNext3(fadeInOut, SmoothMoving));
                    ShakeCamera.instance.StartShake(0.2f, 0.2f);
                    anim.SetBool("Sit", false);
                    Stat.GetComponent<Stat>().MP -= 100;
                    canDash = true;
                    rigid.constraints = RigidbodyConstraints2D.None;
                    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                    Damaged(1);
                    hcurT = 0f;
                    yield break;
                }
            }
            yield return null;
        }
    }

  void HideHealEffect()
    {
        HealEffect.gameObject.SetActive(false);

    }


    public void DashAnim()
    {
        anim.SetTrigger("IsDash");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamagedTile"))
        {
            Damaged(-1);
        }
    }


  /*  private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            anim.SetBool("Ladder", false);
            isLadder = false;
            isClimbing = false;
        }
    }
  */
    public void Damaged(int dmg)
    {
        if(dmg > 0)
        {
            Debug.Log("1");

            for (int i = 0; i < dmg; i++)
            {
                Hp.instance.Recover(curHp);
                curHp++;
            }
            return;
        }
        curHp += dmg;
        Hp.instance.udtHp(curHp, maxHp);
        if (curHp <= 0)
        {
            GameManager.instance.isGameOver = true;
            GameManager.instance.PlayerDead();
            gameObject.SetActive(false); //나중에 프리팹화해서 파괴로 바꿀예정
        }

        if(dmg < 0)
        {
            SlowMotionClass.instance.DoSlowMotion();
            AudioPlayer.PlayOneShot(DamagedSound);
            SetAudioEffect(true);
            Invoke("ResetAudioEffect", 0.35f);


        }
    }

    public void ZoomIn()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed);
    }

    public void ZoomOut()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 9, 1);
    }

    public void SetAudioEffect(bool is1stView)
    {
        audioLowPassFilter.cutoffFrequency = (is1stView == true) ? lowpassValue : 22000;
    }
    void ResetAudioEffect()
    {
        audioLowPassFilter.cutoffFrequency = 22000;
        AudioPlayer.volume = 1;

    }


    private void updatePlayerState()
    {
        _IsGrounded = IsGrounded();
        anim.SetBool("IsGround", _IsGrounded);
        float verticalVelocity = rigid.velocity.y;
        anim.SetBool("IsDown", verticalVelocity < 0);
        if (IsGrounded() || IsWalled())
        {
            jumpLeft = 1;
            anim.SetBool("IsJump", false);
            anim.ResetTrigger("IsJumpFirst");
            anim.SetBool("IsDown", false);

        }
    }

}
