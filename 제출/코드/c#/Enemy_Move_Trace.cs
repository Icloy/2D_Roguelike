using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy_Move_Trace : MonoBehaviour
{
    public float wanderspeed;
    public float tracespeed;
    private float movespeed;

    public float position_change_second;
    public bool followtrue;

    Coroutine movecoroutine;
    CircleCollider2D circle;
    Rigidbody2D rigid;
    Transform targetTransform = null;
    Vector3 position;
    float angle = 0;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        movespeed = wanderspeed;
        StartCoroutine(Wander());
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Wander()
    {
        while(true)
        {
            findmovepoint();
            if(movecoroutine != null)
            {
                StopCoroutine(movecoroutine);
            }
            movecoroutine = StartCoroutine(Move(rigid, movespeed));
            yield return new WaitForSeconds(position_change_second); 
        }
    }

    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputangle = inputAngleDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(inputangle), Mathf.Sin(inputangle), 0);
    }

    void findmovepoint()
    {
        angle += Random.Range(0, 360);
        angle = Mathf.Repeat(angle, 360);
        position += Vector3FromAngle(angle);

    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while(remaindistance > float.Epsilon)
        {
            if(targetTransform != null)
            {
                position = targetTransform.position;
            }
            if(rigidBodyToMove != null)
            {
                Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, speed * Time.deltaTime);
                rigid.MovePosition(newposition);
                remaindistance = (transform.position - position).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && followtrue)
        {
            movespeed = tracespeed;
            targetTransform = collision.gameObject.transform;
            if(movecoroutine != null)
            {
                StopCoroutine(movecoroutine);
            }
            movecoroutine = StartCoroutine(Move(rigid, movespeed));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(movecoroutine != null)
            {
                movespeed = wanderspeed;
                StopCoroutine(movecoroutine);
            }
            targetTransform = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    void OnDeawGizmos()
    {
        if (circle != null)
        {
            Gizmos.DrawWireSphere(transform.position, circle.radius);
        } 
    }

}
