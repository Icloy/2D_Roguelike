using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


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
    private bool canHeal = true;
   

    SpriteRenderer spriteRenderer;
    Animator anim;

    private bool _IsGrounded;
    private Transform _transform;
    private bool isClimbing;


    [HideInInspector] [SerializeField] private Rigidbody2D rigid;
    [HideInInspector] [SerializeField] private Transform groundCheck;
    [HideInInspector] [SerializeField] private LayerMask groundLayer;
    [HideInInspector] [SerializeField] private Transform wallCheck;
    [HideInInspector] [SerializeField] private LayerMask wallLayer;

    private float curTime;
    public float coolTime = 0.5f;
    [HideInInspector]  public Transform pos;

    bool zoomIn;

    
    [Range(2, 100)]
    public float zoomSize;

    [HideInInspector]
    [Range(0.01f, 0.1f)]
    private float zoomSpeed = 0.1f;

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
    [HideInInspector] public GameObject Damaged_Effect;
    [HideInInspector] public GameObject Dash_Effect;
    Camera cam;
    public bool fadeInOut;
    public bool SmoothMoving;

      public GameObject HealEffect;
    [HideInInspector]  public AudioClip AttackSound;
    [HideInInspector]  public AudioClip HealSound;
    [HideInInspector]  public AudioClip DashSound;
    [HideInInspector] public AudioClip DamagedSound;
    private Vector2 direction;

    private float _fallSpeedYDampingChangeThreshold;
    private bool IsLeft;
    private bool IsRight;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

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
        if (Singleton.Instance.newGame)
        {
            Hp.instance.udtHp(curHp, maxHp);
        }
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
    }

    void Update()
    {
        updatePlayerState();

        if (rigid.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if(rigid.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;

            CameraManager.instance.LerpYDamping(false);
        }



        #region Move

        horizontal = Input.GetAxisRaw("Horizontal");
        float moveDirection = transform.localScale.x * horizontal;



        if (Input.GetKey(KeyCode.LeftArrow)&&!GameManager.instance.isPanelOpen)
        {
            transform.localScale = new Vector3(-1f, 1f);
            anim.SetBool("IsRun", true);
            IsLeft = true;
            IsRight = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow)&& !GameManager.instance.isPanelOpen)
        {
            transform.localScale = new Vector3(1f, 1f);
            anim.SetBool("IsRun", true);
            IsRight = true;
            IsLeft = false;

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

        if (!IsGrounded() && !IsWalled())
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
        if (Input.GetKeyDown(KeyCode.W) && canDash &&(!isWallSliding) && canHeal)
        {
            StartCoroutine(Dash());
            Dash_Effect.gameObject.SetActive(true);
            Invoke("HideDashEffect", 0.3f);
        }

        #endregion


        #region Attack

        if (curTime <= 0)
        {

            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Q) && !GameManager.instance.isPanelOpen && !isWallSliding)
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
            
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Q) && !GameManager.instance.isPanelOpen && !isWallSliding && !IsGrounded())
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
        if (Input.GetKeyDown(KeyCode.A) && curHp < maxHp && IsGrounded() && Stat.GetComponent<Stat>().MP >= 100 && canHeal)
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
            canHeal = true;
            StopCoroutine("Heal");
            hcurT = 0f;
            rigid.constraints = RigidbodyConstraints2D.None;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            ZoomOut();
            anim.SetBool("Sit", false);
            HealEffect.gameObject.SetActive(false);

        }
        #endregion


    }

    void FixedUpdate()
    {

        rigid.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // Right Max Speed
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // Left Max Speed

    }



    private void HideEffect()
    {
        AEffect.gameObject.SetActive(false);
        AEffect_Up.gameObject.SetActive(false);
        AEffect_Down.gameObject.SetActive(false);
    }

    private void HideDamagedEffect()
    {
        Damaged_Effect.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(dashingTime);
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
            anim.SetTrigger("IsClimbJump");
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
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;
                canHeal = false;
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
                    CSake.instance.Vibrate(1f);
                    anim.SetBool("Sit", false);
                    Stat.GetComponent<Stat>().MP -= 100;
                    canHeal = true;
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

    void HideDashEffect()
    {
        Dash_Effect.gameObject.SetActive(false);

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
            collision.gameObject.GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f);
        }
    }


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
            AudioPlayer.PlayOneShot(DamagedSound);
            Damaged_Effect.gameObject.SetActive(true);
            Invoke("HideDamagedEffect", 0.2f);
            anim.SetTrigger("IsHurt");
        }

    }

    public void ZoomIn()
    {
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, zoomSize, zoomSpeed * Time.deltaTime);
    }

    public void ZoomOut()
    {
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, 60, 45);
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

    public void upforce()
    {
        rigid.AddForce(Vector2.up * 1200f);
    }

    public void sideForce()
    {
        if (IsLeft)
        {
            rigid.AddForce(Vector2.right * 1200f);
        }
        if(IsRight)
        {
            rigid.AddForce(Vector2.left * 1200f);
        }

    }



}
