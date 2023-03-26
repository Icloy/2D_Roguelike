
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    BoxCollider2D box;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Coroutine thinkcoroutine;

    string animationState = "animationState";
    private bool trace;
    private bool turnflag;
    private int nextMove;

    public float tracespeed;
    public float turnrange;

    public GameObject Attack1_check;
    public GameObject Attack2_check;

    public AudioClip EnemyAttackSound;

    private AudioSource AudioPlayer; //오디오 소스 컴포넌트

    enum States
    {
        idle = 0,
        walk = 1,
        attack1 = 2,
        attack2 = 3,
        hit = 4
    }
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        box = GetComponentInChildren<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        AudioPlayer = GetComponent<AudioSource>();

    }

    void Start()
    {
        animator.SetInteger(animationState, (int)States.idle);
        trace = turnflag = false;
        StartCoroutine(move());
        ThinkCall();
    }

    void Update()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0)); // 디버그용 두문장 나중에 지워도 상관없음
    }

    public IEnumerator Think()
    {
        while (true)
        {
            float nextThinkTime = Random.Range(2f, 5f); // 대기시간 2초에서 5초 사이랜덤
            if (turnflag == true)
            {
                yield return new WaitForSeconds(nextThinkTime);
            }
            nextMove = Random.Range(-1, 2); // -1~1 랜덤돌리고 값 던지기
            yield return new WaitForSeconds(nextThinkTime);
        }
    }

    public IEnumerator move()
    {
        while (true)
        {
            if (PlayerPos != null)
            {
                dis = Vector2.Distance(PlayerPos.transform.position, rigid.transform.position);
                if (dis < 1.4f)
                {
                    rigid.velocity = Vector2.zero;
                    yield return StartCoroutine(Attack());
                }
            }
            if (nextMove == 1)//애니메이션 및 스프라이트 방향 변경 자식 개체까지 바꿔야하므로 가장 상위 transform 변경
            {
                FlipBack();
                animator.SetInteger(animationState, (int)States.walk);
            }
            else if (nextMove == -1)
            {
                FlipX();
                animator.SetInteger(animationState, (int)States.walk);
            }
            else if (nextMove == 0)
            {
                animator.SetInteger(animationState, (int)States.idle);
            }
            if (trace == true)//추적
            {
                if (PlayerPos.transform.position.x < rigid.transform.position.x)
                {
                    FlipX();
                    nextMove = -1;
                    rigid.velocity = new Vector2(nextMove * tracespeed, rigid.velocity.y);
                    animator.SetInteger(animationState, (int)States.walk);
                }
                else
                {
                    FlipBack();
                    nextMove = 1;
                    rigid.velocity = new Vector2(nextMove * tracespeed, rigid.velocity.y);
                    animator.SetInteger(animationState, (int)States.walk);
                }
            }
            if (trace == false) //땅 체크 + 턴 및 이동
            {
                Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
                Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
                RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
                if (rayHit.collider == null)
                {
                    Turn();
                }
                rigid.velocity = new Vector2(nextMove * movespeed, rigid.velocity.y);
            }
            yield return new WaitForSeconds(0f);
        }
    }

    public IEnumerator Attack()
    {
        int actmove = Random.Range(1, 3);
        Debug.Log(dis);
        switch (actmove)
        {
            case 1:
                animator.SetInteger(animationState, (int)States.attack1);
                yield return new WaitForSeconds(0.84f);//애니메이션 공격 이펙트 위치
                Attack1_check.gameObject.SetActive(true);
                AudioPlayer.PlayOneShot(EnemyAttackSound);

                break;
            case 2:
                animator.SetInteger(animationState, (int)States.attack2);
                yield return new WaitForSeconds(0.84f);
                Attack2_check.gameObject.SetActive(true);
                AudioPlayer.PlayOneShot(EnemyAttackSound);

                break;
        }
        yield return new WaitForSeconds(0.16f);// 애니메이션 끝나는 위치
        Attack_Check_Off();
        yield break;
    }

    public override void TakeDamage(int AtDmg)
    {
        Hp = Hp - AtDmg;
        Debug.Log(Hp);
        StopAllCoroutines();
        StartCoroutine(KnockBack());
        if (Hp <= 0)
        {
            Die();
        }

    }

    void FlipX() // transform 변경 transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); 이 아닌 고정값으로 하는 이유는 트리거 안에 들어갔다 나오는식으로 플레이어가 행동하게되면 *-1이라서 반전 버그남
    {
        transform.localScale = new Vector3(-5, transform.localScale.y, transform.localScale.z);
    }

    void FlipBack()
    {
        transform.localScale = new Vector3(5, transform.localScale.y, transform.localScale.z);
    }

    void Attack_Check_Off()
    {
        Attack1_check.gameObject.SetActive(false);
        Attack2_check.gameObject.SetActive(false);
    }

    public IEnumerator KnockBack()
    {
        if (PlayerPos != null)
        {
            rigid.velocity = Vector2.zero;
            animator.SetInteger(animationState, (int)States.hit);
            rigid.AddForce(Vector2.up * knockbackdis, ForceMode2D.Impulse);
            if (PlayerPos.transform.position.x < rigid.transform.position.x)
            {
                rigid.AddForce(Vector2.right * knockbackdis, ForceMode2D.Impulse);
            }
            else
            {
                rigid.AddForce(Vector2.left * knockbackdis, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(move());
            ThinkCall();
            yield break;
        }
    }

    void Turn()//턴
    {
        nextMove = nextMove * (-1);
        turnflag = true;
        ThinkStop();
        ThinkCall();
    }

    void ThinkCall()
    {
        thinkcoroutine = StartCoroutine(Think());
    }

    void ThinkStop()
    {
        if (thinkcoroutine != null)
        {
            StopCoroutine(thinkcoroutine);
        }
    }

    void Die()
    {
        DropItem();
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        Debug.Log("호출");
        for (int i = 0; i < dropcoincnt; i++)
        {
            float x = Random.Range(-1f, 1f); // x축 위치 랜덤 설정
            float y = Random.Range(-1f, 1f); // y축 위치 랜덤 설정
            Vector2 position = new Vector2(transform.position.x + x, transform.position.y + y);
            Instantiate(Item, position, Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player.instance.Damaged(-collision_damage);
        }
    }

    void OnTriggerStay2D(Collider2D collision) //Enter에서 Stay로 변경 공격을 당하면 자식트리거쪽 exit도 반응해서 추적 풀려버림
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerPos == null)
            {
                PlayerPos = collision.gameObject.transform;
            }
            trace = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("player exit");
            trace = false;
        }
    }


}