using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    GameManager gameManager;
    public float updistance;

    public float speed;
    public float position_change_second;

    Coroutine movecoroutine;
    CircleCollider2D circle;
    Transform targetTransform = null;
    Vector3 position;
    Rigidbody2D rigid;

    private void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        rigid.AddForce(Vector2.up * updistance, ForceMode2D.Impulse);
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            targetTransform = collision.gameObject.transform;
            StartCoroutine(Move(rigid, speed));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //코인계수 업데이트
            gameManager.UpdateCoinCnt(1);
            Destroy(this.gameObject);
        }
    }
}
