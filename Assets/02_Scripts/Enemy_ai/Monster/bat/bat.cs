using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bat : Enemy
{
    public float position_change_second;
    public float delete_time;

    float MaxHp;
    bool bloodflag;
    private bool traceflag;
    private bool Collision_Damage_Delay;
    CircleCollider2D circle;
    Rigidbody2D rigid;
    Vector3 position;
    Animator animator;
    SpriteRenderer sprite;
    Coroutine coroutine;

    string animationState = "animationState";
    enum States
    {
        idle = 0,
        fly = 1,
        hit = 2
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        HpBar = GetComponentInChildren<Canvas>();
    }

    void start()
    {
        MaxHp = Hp;
        traceflag = Collision_Damage_Delay = false;
        animator.SetInteger(animationState, (int)States.idle);
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float movespeed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            animator.SetInteger(animationState, (int)States.fly);
            Debug.Log(dis);
            if (PlayerPos != null)
            {
                position = PlayerPos.position;
            }
            if (rigidBodyToMove != null)
            {
                if (PlayerPos.position.x < rigid.transform.position.x)
                {
                    HpBar.GetComponent<RectTransform>().localScale = new Vector3(5f, 5f, 0);
                    sprite.flipX = false;
                    bloodflag = false;
                }
                else
                {
                    HpBar.GetComponent<RectTransform>().localScale = new Vector3(-5f, 5f, 0);
                    sprite.flipX = true;
                    bloodflag = true;
                }
                if (traceflag == true)
                {
                    Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, -movespeed * Time.deltaTime);
                    rigid.MovePosition(newposition);
                    remaindistance = (transform.position - position).sqrMagnitude;
                }
                else
                {
                    Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, movespeed * 2 * Time.deltaTime);
                    rigid.MovePosition(newposition);
                    remaindistance = (transform.position - position).sqrMagnitude;
                }
            }
            yield return new WaitForFixedUpdate();
        }
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
            Debug.Log("knockback check");
            if (PlayerPos.transform.position.x < rigid.transform.position.x)
            {
                rigid.AddForce(Vector2.right * knockbackdis, ForceMode2D.Impulse);
            }
            else
            {
                rigid.AddForce(Vector2.left * knockbackdis, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(1.2f);
            MoveCall();
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

    public override void TakeDamage(int AtDmg)
    {
        Hp = Hp - AtDmg;
        HpFill.fillAmount = Hp / MaxHp;
        Debug.Log(Hp);
        StopAllCoroutines();
        StartCoroutine(KnockBack());
        if (Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        DropItem();
        Vector2 position = new Vector2(rigid.position.x, rigid.position.y + 0.2f);
        Instantiate(Corpse, position, Quaternion.identity);
        Destroy(this.gameObject);
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

    void MoveCall()
    {
        coroutine = StartCoroutine(Move(rigid, movespeed));
    }

    void MoveStop()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("enter");
            StartCoroutine(Alert());
            PlayerPos = collision.gameObject.transform;
            MoveCall();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            traceflag = false;
            if (Collision_Damage_Delay == true)
            {
                Collision_Damage_Delay = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (Collision_Damage_Delay == false)
        {
            if (col.gameObject.tag == "Player")
            {
                Collision_Damage_Delay = true;
                Debug.Log("false");
                traceflag = true;
                Player.instance.Damaged(-collision_damage);
            }
            else if (col.gameObject.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
            }
        }
    }
}
