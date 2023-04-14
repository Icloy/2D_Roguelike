
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goblin : Enemy
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
    bool bloodflag;

    public Image HpFill;
    public int MaxHp;

    public float tracespeed;
    public float turnrange;

    public GameObject Attack1_check;
    public GameObject Attack2_check;
    public GameObject alert;
    enum States
    {
        idle = 0,
        walk = 1,
        attack1 = 2,
        attack2 = 3,
        hit = 4,
        die = 5
    }
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        box = GetComponentInChildren<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        animator.SetInteger(animationState, (int)States.idle);
        trace = turnflag = false;
        HpFill.fillAmount = Hp;
        StartCoroutine(move());
        ThinkCall();
    }

    void Update()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * turnrange, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 255, 0)); // ����׿� �ι��� ���߿� ������ �������
    }

    public IEnumerator Think()
    {
        while (true)
        {
            float nextThinkTime = Random.Range(2f, 5f); // ���ð� 2�ʿ��� 5�� ���̷���
            if (turnflag == true)
            {
                yield return new WaitForSeconds(nextThinkTime);
            }
            nextMove = Random.Range(-1, 2); // -1~1 ���������� �� ������
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
                if (dis < 1.5f)
                {
                    rigid.velocity = Vector2.zero;
                    yield return StartCoroutine(Attack());
                }
            }
            if (nextMove == 1)//�ִϸ��̼� �� ��������Ʈ ���� ���� �ڽ� ��ü���� �ٲ���ϹǷ� ���� ���� transform ����
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
            if (trace == true)//����
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
            if (trace == false) //�� üũ + �� �� �̵�
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
        rigid.velocity = Vector2.zero;
        int actmove = Random.Range(1, 3);
        Debug.Log(dis);
        switch (actmove)
        {
            case 1:
                animator.SetInteger(animationState, (int)States.attack1);
                yield return new WaitForSeconds(0.84f);//�ִϸ��̼� ���� ����Ʈ ��ġ
                Attack1_check.gameObject.SetActive(true);
                break;
            case 2:
                animator.SetInteger(animationState, (int)States.attack2);
                if (PlayerPos != null)
                {
                    rigid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                    if (PlayerPos.transform.position.x < rigid.transform.position.x)
                    {
                        rigid.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.84f);
                        
                        rigid.velocity = Vector2.zero;
                        rigid.AddForce(Vector2.left * 15, ForceMode2D.Impulse);
                    }
                    else
                    {
                        rigid.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
                        yield return new WaitForSeconds(0.84f);
                        rigid.velocity = Vector2.zero;
                        rigid.AddForce(Vector2.right * 15, ForceMode2D.Impulse);
                    }
                }
                yield return new WaitForSeconds(0.24f);
                rigid.velocity = Vector2.zero;
                Attack2_check.gameObject.SetActive(true);
                break;
        }
        yield return new WaitForSeconds(0.16f);// �ִϸ��̼� ������ ��ġ
        Attack_Check_Off();
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
        Instantiate(Corpse, position, Quaternion.identity);
        Destroy(this.gameObject);
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

    void FlipX()
    {
        transform.localScale = new Vector3(-3f, transform.localScale.y, transform.localScale.z);
        bloodflag = false;
    }

    void FlipBack()
    {
        transform.localScale = new Vector3(3f, transform.localScale.y, transform.localScale.z);
        bloodflag = true;
    }

    void Attack_Check_Off()
    {
        Attack1_check.gameObject.SetActive(false);
        Attack2_check.gameObject.SetActive(false);
    }

    void Turn()//��
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
        Debug.Log("ȣ��");
        for (int i = 0; i < dropcoincnt; i++)
        {
            float x = Random.Range(-1f, 1f); // x�� ��ġ ���� ����
            float y = Random.Range(0f, 1f); // y�� ��ġ ���� ����
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

    void OnTriggerEnter2D(Collider2D collision) //Enter���� Stay�� ���� ������ ���ϸ� �ڽ�Ʈ������ exit�� �����ؼ� ���� Ǯ������
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
            trace = false;
        }
    }


}