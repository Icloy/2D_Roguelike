using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    CircleCollider2D circle;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator animator;
    Coroutine thinkcoroutine;

    public float jumpPower;
    public GameObject trap;
    public GameObject drop;

    public float tracespeed;
    public float turnrange;

    private bool trace;
    private bool turnflag;
    bool bloodflag;
    private int nextMove;
    private int actmove;
    private int direction;
    private int dropran;
    private int dropcnt;
    string animationState = "animationState";

    public GameObject Attack1_check;
    public GameObject Attack2_check;
    public GameObject alert;
    enum States
    {
        idle = 1,
        walk = 2,
        hit = 3,
        attack = 4,
        die = 5,
        cast = 6,
        disappear = 7,
        appears = 8,
        throwback = 9
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponentInChildren<CircleCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(AttackThink());
    }

    void Update()
    {
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
                rigid.velocity = Vector2.zero;
                yield return StartCoroutine(AttackThink());
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

    public IEnumerator AttackThink()
    {
            animator.SetInteger(animationState, (int)States.idle);
            dis = Vector2.Distance(PlayerPos.transform.position, rigid.transform.position);
            Vector2 player = PlayerPos.transform.position;
            if (player.x < rigid.transform.position.x)
            {
                direction = 1;
                FlipBack();
            }
            else
            {
                direction = 2;
                FlipX();
            }
            actmove = Random.Range(1, 3);
            Debug.Log(dis);
            Debug.Log(direction);
            Debug.Log(actmove);
            if (dis <= 2.5)
            {
                yield return StartCoroutine(act1(actmove));
            }
            else if (dis <= 5)
            {
                yield return StartCoroutine(act2(actmove));
            }
            else
            {
                yield return StartCoroutine(act3(actmove));
            }
    }

    public IEnumerator act1(int actmove)
    {
        Debug.Log("act1");
        switch (actmove)
        {
            case 1:
                Debug.Log("act1_1");
                animator.SetInteger(animationState, (int)States.attack);
                yield return new WaitForSeconds(1f);
                animator.SetInteger(animationState, (int)States.idle);
                break;
            case 2:
                Debug.Log("act1_2");
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                switch (direction)
                {
                    case 1:
                        rigid.velocity = new Vector2(1 * movespeed, rigid.velocity.y);
                        break;
                    case 2:
                        rigid.velocity = new Vector2(-1 * movespeed, rigid.velocity.y);
                        break;
                }
                break;
        }

    }

    public IEnumerator act2(int actmove)
    {
        Debug.Log("act2");
        switch (actmove)
        {
            case 1:
                Debug.Log("act2_1");
                animator.SetInteger(animationState, (int)States.disappear);
                yield return new WaitForSeconds(1f);
                int j = Random.Range(1, 3);
                rigid.isKinematic = true;
                switch (j)
                {
                    case 1:
                        gameObject.transform.position = new Vector3(PlayerPos.transform.position.x + 1.5f, PlayerPos.transform.position.y + 5f, PlayerPos.transform.position.z);
                        FlipBack();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        rigid.isKinematic = false;
                        animator.SetInteger(animationState, (int)States.idle);
                        rigid.AddForce(Vector2.down * 30f, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(PlayerPos.transform.position.x - 1.5f, PlayerPos.transform.position.y + 5f, PlayerPos.transform.position.z);
                        FlipX();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        rigid.isKinematic = false;
                        animator.SetInteger(animationState, (int)States.idle);
                        rigid.AddForce(Vector2.down * 30f, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(1f);
                        break;
                }
                break;
            case 2:
                Debug.Log("act2_1");
                animator.SetInteger(animationState, (int)States.disappear);
                yield return new WaitForSeconds(1f);
                int i = Random.Range(1, 3);
                switch (i)
                {
                    case 1:
                        gameObject.transform.position = new Vector3(PlayerPos.transform.position.x + 1.5f, PlayerPos.transform.position.y - 0.5f, PlayerPos.transform.position.z);
                        FlipBack();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.attack);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.idle);
                        break;
                    case 2:
                        gameObject.transform.position = new Vector3(PlayerPos.transform.position.x -1.5f, PlayerPos.transform.position.y-0.5f, PlayerPos.transform.position.z);
                        FlipX();
                        animator.SetInteger(animationState, (int)States.appears);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.attack);
                        yield return new WaitForSeconds(1f);
                        animator.SetInteger(animationState, (int)States.idle);
                        break;
                }
                break;
        }
    }

    public IEnumerator act3(int actmove)
    {
        Debug.Log("act3");
        switch (actmove)
        {
            case 1:
                animator.SetInteger(animationState, (int)States.throwback);
                yield return new WaitForSeconds(0.85f);
                Debug.Log("act3_1");
                if(direction == 1)
                {
                    Instantiate(trap, new Vector3(rigid.transform.position.x - 2f, rigid.transform.position.y + 1f, rigid.transform.position.z), Quaternion.identity);
                }
                else
                {
                    Instantiate(trap, new Vector3(rigid.transform.position.x + 2f, rigid.transform.position.y + 1f, rigid.transform.position.z), Quaternion.identity);
                }
                yield return new WaitForSeconds(1.65f);

                break;
            case 2:
                animator.SetInteger(animationState, (int)States.cast);
                yield return new WaitForSeconds(2.5f);
                Debug.Log("act3_2");
                dropcnt = Random.Range(-3, 5);
                for (int i = -3; i <= dropcnt; i++)
                {
                    dropran = Random.Range(1, 6);
                    if (dropran <= 4)
                    {
                        Instantiate(drop, new Vector3(PlayerPos.transform.position.x + 1f * i, PlayerPos.transform.position.y + 2.5f, PlayerPos.transform.position.z), Quaternion.identity);
                    }
                }
                break;
        }
        animator.SetInteger(animationState, (int)States.idle);
    }

    public IEnumerator KnockBack()
    {
        if (PlayerPos != null)
        {
            rigid.velocity = Vector2.zero;
            if (bloodflag == true)
            {
                StartCoroutine(LBlood());
            }
            else
            {
                StartCoroutine(RBlood());
            }
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

    IEnumerator Alert()
    {
        alert.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        alert.gameObject.SetActive(false);
    }

    IEnumerator LBlood()
    {
        lblood.Play();
        yield return new WaitForSeconds(0.6f);
        lblood.Stop();
    }

    IEnumerator RBlood()
    {
        rblood.Play();
        yield return new WaitForSeconds(0.6f);
        rblood.Stop();
    }

    IEnumerator Die()
    {
        rigid.velocity = Vector2.zero;
        animator.SetInteger(animationState, (int)States.die);
        yield return new WaitForSeconds(0.2f);
        DropItem();
        Destroy(this.gameObject);
    }

    public override void TakeDamage(int AtDmg)
    {
        Hp = Hp - AtDmg;
        Debug.Log(Hp);
        if (Hp <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(KnockBack());
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

    void DropItem()
    {
        for (int i = 0; i < dropcoincnt; i++)
        {
            float x = Random.Range(-1f, 1f); // x축 위치 랜덤 설정
            float y = Random.Range(0f, 1f); // y축 위치 랜덤 설정
            Vector2 position = new Vector2(transform.position.x + x, transform.position.y + y);
            Instantiate(Item, position, Quaternion.identity);
        }
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-4, 5, 1);
    }

    void FlipBack()
    {
        transform.localScale = new Vector3(4, 5, 1);
    }

    void OnTriggerEnter2D(Collider2D collision) //Enter에서 Stay로 변경 공격을 당하면 자식트리거쪽 exit도 반응해서 추적 풀려버림
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Alert());
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
            if (PlayerPos != null)
            {
                PlayerPos = null;
            }
            trace = false;
        }
    }
}