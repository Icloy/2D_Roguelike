using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Coin : MonoBehaviour
{
    public GameObject target;
    CircleCollider2D circle;
    Rigidbody2D rigid;

    float speed;

    private void Awake()
    {
        target = GameObject.Find("coincollect");
        circle = GetComponent<CircleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed = 7f;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            circle.enabled = false;
            rigid.velocity = Vector3.zero;
            rigid.gravityScale = 0;
            StartCoroutine(CoinMove());
        }
        /*
        else if(!(col.gameObject.GetComponent<TilemapCollider2D>() != null))
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
        */
        else if (col.gameObject.CompareTag("Item"))
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
    }

    IEnumerator CoinMove( )
    {
        while(true)
        {
            float distance = (target.transform.position - this.transform.position).sqrMagnitude;
            if(distance >= 1.5f)
            {
                transform.position = Vector3.Lerp(this.transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                GameManager.instance.UpdateCoinCnt(1);
                Destroy(this.gameObject);
                break;
            }
            yield return null;
        }
    }
}


/*
    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                position = targetTransform.position;
            }
            if (rigidBodyToMove != null)
            {
                Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, speed * Time.deltaTime);
                rigid.MovePosition(newposition);
                remaindistance = (transform.position - position).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
    }
*/
