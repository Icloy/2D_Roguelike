using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyingeye : Enemy
{
    public float position_change_second;
    public float delete_time;
    private bool flag;
    CircleCollider2D circle;
    Rigidbody2D rigid;
    Vector3 position;
    Animator animator;
    SpriteRenderer sprite;
    bool bloodflag;
    float MaxHp;

    public GameObject explosion;
    string animationState = "animationState";

    enum States
    {
        flight = 0,
        boom = 1,
        hit = 2,
        die = 3
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        HpBar = GetComponentInChildren<Canvas>();
    }

    void Start()
    {
        animator.SetInteger(animationState, (int)States.flight);
        flag = bloodflag =true;
        MaxHp = Hp;
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Move(float movespeed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            if (PlayerPos.position.x > rigid.transform.position.x)
            {
                FlipBack();
                animator.SetInteger(animationState, (int)States.flight);
            }
            else
            {
                FlipX();
                animator.SetInteger(animationState, (int)States.flight);
            }
            if (PlayerPos != null)
            {
                position = PlayerPos.position;
            }
            Vector3 newposition = Vector3.MoveTowards(rigid.position, position, movespeed * Time.deltaTime);
            rigid.MovePosition(newposition);
            remaindistance = (transform.position - position).sqrMagnitude;
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator Boom()
    {
        rigid.velocity = Vector2.zero;
        animator.SetInteger(animationState, (int)States.boom);
        yield return new WaitForSeconds(0.5f);
        explosion.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        Destroy(this.gameObject);
    }

    public IEnumerator KnockBack()
    {
        if (PlayerPos != null)
        {
            rigid.velocity = Vector2.zero;
            if(bloodflag == true)
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
            yield return new WaitForSeconds(0.2f);
            flag = true;
            StartCoroutine(Move(movespeed));
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

    void FlipX()
    {
        transform.localScale = new Vector3(-2.5f, transform.localScale.y, transform.localScale.z);
        HpBar.GetComponent<RectTransform>().localScale = new Vector3(-0.4f, 0.4f, 0);
        bloodflag = false;
    }

    void FlipBack()
    {
        transform.localScale = new Vector3(2.5f, transform.localScale.y, transform.localScale.z);
        HpBar.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0);
        bloodflag = true;
    }

    void DropItem()
    {
        Debug.Log("호출");
        for (int i = 0; i < dropcoincnt; i++)
        {
            float x = Random.Range(-1f, 1f); // x축 위치 랜덤 설정
            float y = Random.Range(0f, 1f); // y축 위치 랜덤 설정
            Vector2 position = new Vector2(transform.position.x + x, transform.position.y + y);
            Instantiate(Item, position, Quaternion.identity);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Alert());
            PlayerPos = collision.gameObject.transform;
            StartCoroutine(Move(movespeed));
        }

    }
    void OnCollisionEnter2D(Collision2D col)
    {
            if (col.gameObject.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
            }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            dis = Vector2.Distance(PlayerPos.transform.position, rigid.transform.position);
            if (dis < 1)
            {
                if (flag == true)
                {
                    flag = false;
                    StopAllCoroutines();
                    StartCoroutine(Boom());
                }
            }
        }
    }

}
