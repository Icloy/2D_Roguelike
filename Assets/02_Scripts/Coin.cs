using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    GameManager gameManager;
    public GameObject target;
    public CircleCollider2D circle;

    float speed;
    Vector3 startPos;

    private void Awake()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        target = GameObject.Find("coincollect");
        circle = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        speed = 13f;
        startPos = this.transform.position;
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            circle.enabled = false;
            Vector3 pos = col.transform.position;
            StartCoroutine(CoinMove());
        }

    }

    IEnumerator CoinMove( )
    {
        while(true)
        {
            float distance = (target.transform.position - this.transform.position).sqrMagnitude;
            if(distance >= 1f)
            {
                transform.position = Vector3.Lerp(this.transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                gameManager.UpdateCoinCnt(1);
                Destroy(this.gameObject);
                break;
            }
            yield return null;
        }
    }
}
