using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyingeye : Enemy
{
    public float speed;
    public float position_change_second;
    public float delete_time;
    public float knockbackdis;

    private float dis;
    private bool flag;

    CircleCollider2D circle;
    Rigidbody2D rigid;
    Transform targetTransform = null;
    Vector3 position;
    Animator animator;
    SpriteRenderer sprite;

    public GameObject explosion;

    string animationState = "animationState";

    enum States
    {
        flight = 0,
        boom = 1,
        hit = 2,
        fall = 3,
        die = 4
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        animator.SetInteger(animationState, (int)States.flight);
        flag = true;
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Move(float speed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            if (targetTransform.position.x > rigid.transform.position.x)
            {
                sprite.flipX = false;
                animator.SetInteger(animationState, (int)States.flight);
            }
            else
            {
                sprite.flipX = true;
                animator.SetInteger(animationState, (int)States.flight);
            }
            if (targetTransform != null)
            {
                position = targetTransform.position;
            }
            Vector3 newposition = Vector3.MoveTowards(rigid.position, position, speed * Time.deltaTime);
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

    public IEnumerator KnockBack()
    {
        if (targetTransform != null)
        {
            animator.SetInteger(animationState, (int)States.hit);
            rigid.AddForce(Vector2.up * knockbackdis, ForceMode2D.Impulse);
            if (targetTransform.transform.position.x < rigid.transform.position.x)
            {
                rigid.AddForce(Vector2.right * knockbackdis, ForceMode2D.Impulse);
            }
            else
            {
                rigid.AddForce(Vector2.left * knockbackdis, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.2f);
            flag = true;
            StartCoroutine(Move(speed));
            yield break;
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetTransform = collision.gameObject.transform;
            StartCoroutine(Move(speed));
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dis = Vector2.Distance(targetTransform.transform.position, rigid.transform.position);
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
