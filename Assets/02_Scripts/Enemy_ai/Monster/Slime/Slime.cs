using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Slime : Enemy
{
    BoxCollider2D box;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Coroutine coroutine;
    Coroutine thinkcoroutine;
    string animationState = "animationState";
    private float nextMove;
    private int direction;
    private int actmove;
    private bool attackflag;
    private bool turnflag;
    bool bloodflag;
    float MaxHp;

    public float jumpPower;
    public float turnrange;

    enum States
    {
        walk = 1,
        jump = 2,
        spin = 3,
        die = 4
    }
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        box = GetComponentInChildren<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        HpBar = GetComponentInChildren<Canvas>();
    }
    void Start()
    {
        MaxHp = Hp;
        coroutine = StartCoroutine(move());
        ThinkCall();
        attackflag = turnflag = false;
    }

    void Update()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
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
        while(true)
        {
            if (rigid.velocity.x > 0)
            {
                HpBar.GetComponent<RectTransform>().localScale = new Vector3(-0.3f, 0.35f, 0);
                GetComponentInChildren<SpriteRenderer>().flipX = false;
                bloodflag = true;
            }
            else
            {
                HpBar.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.35f, 0);
                GetComponentInChildren<SpriteRenderer>().flipX = true;
                bloodflag = false;
            }
            animator.SetInteger(animationState, (int)States.walk);
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null && GetComponent<TilemapCollider2D>() == null)
            {
                Turn();
            }
            rigid.velocity = new Vector2(nextMove * movespeed, rigid.velocity.y);
            yield return new WaitForSeconds(0f);
        }
    }

    public IEnumerator attack()
    {
        Debug.Log("attack");
        dis = Vector2.Distance(PlayerPos.transform.position, rigid.transform.position);
        if (PlayerPos.transform.position.x < rigid.transform.position.x)
        {
            direction = 1;
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else
        {
            direction = 2;
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        actmove = Random.Range(1, 3);
        switch (actmove)
        {
            case 1:
                Debug.Log("case1");
                if (direction == 1)
                {
                    rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.5f);
                    animator.SetInteger(animationState, (int)States.jump);
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    rigid.AddForce(Vector2.left * dis * 1.2f, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.9f);
                    animator.SetInteger(animationState, (int)States.walk);
                }
                else
                {
                    rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.5f);
                    animator.SetInteger(animationState, (int)States.jump);
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    rigid.AddForce(Vector2.right * dis * 1.2f, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.9f);
                    animator.SetInteger(animationState, (int)States.walk);
                }
                break;
            case 2:
                Debug.Log("case2");
                if (direction == 1)
                {
                    rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.5f);
                    animator.SetInteger(animationState, (int)States.spin);
                    rigid.AddForce(Vector2.left * dis * 3, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(1f);
                    animator.SetInteger(animationState, (int)States.walk);
                }
                else
                {
                    rigid.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(0.5f);
                    animator.SetInteger(animationState, (int)States.spin);
                    rigid.AddForce(Vector2.right * dis * 3, ForceMode2D.Impulse);
                    yield return new WaitForSeconds(1f);
                    animator.SetInteger(animationState, (int)States.walk);
                }
                break;
        }
        yield break;
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
            animator.SetInteger(animationState, (int)States.walk);
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
            coroutine = StartCoroutine(move());
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

    public override void TakeDamage(int AtDmg)
    {
        Hp = Hp - AtDmg;
        HpFill.fillAmount = Hp / MaxHp;
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

    IEnumerator Die()
    {
        rigid.velocity = Vector2.zero;
        animator.SetInteger(animationState, (int)States.die);
        rigid.AddForce(Vector2.up * knockbackdis, ForceMode2D.Impulse);
        if (PlayerPos.transform.position.x < rigid.transform.position.x)
        {
            rigid.AddForce(Vector2.right * knockbackdis, ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector2.left * knockbackdis, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.2f);
        DropItem();
        Vector2 position = new Vector2(rigid.position.x, rigid.position.y + 0.2f);
        Instantiate(Corpse, position, Quaternion.identity, transform);
        Destroy(this.gameObject);
    }

    IEnumerator LBlood()
    {
        lblood.Play();
        int angle = Random.Range(-45, 45);
        hiteffect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        hiteffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hiteffect.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        lblood.Stop();
    }

    IEnumerator RBlood()
    {
        rblood.Play();
        int angle = Random.Range(-45, 45);
        hiteffect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        hiteffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hiteffect.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        rblood.Stop();
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
        Debug.Log("호출");
        for (int i = 0; i < dropcoincnt; i++)
        {
            float x = Random.Range(-1f, 1f); // x축 위치 랜덤 설정
            float y = Random.Range(0.5f, 2f); // y축 위치 랜덤 설정
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
        else if (col.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerPos == null)
            {
                PlayerPos = collision.gameObject.transform;
            }
            if (attackflag == false)
            {
                StartCoroutine(Alert());
                attackflag = true;
                StopAllCoroutines();
                coroutine = StartCoroutine(attack());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (attackflag == true)
            {
                attackflag = false;
                StopAllCoroutines();
                ThinkCall();
                coroutine = StartCoroutine(move());
            }
        }
    }

}
