using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    public float speed;
    public float position_change_second;
    public float delete_time;
    public int damage;

    bool flag;
    float distance;

    CircleCollider2D circle;
    PolygonCollider2D polygonCollider;
    Rigidbody2D rigid;
    Transform targetTransform = null;
    Vector3 position;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponentInChildren<CircleCollider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("RightTarget");
        if(targetTransform == null)
        {
            targetTransform = player.transform;
            position = targetTransform.position;
        }
        StartCoroutine(Move(rigid, speed));
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                Vector2 direction = position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                Quaternion additionalRotation = Quaternion.Euler(0, 0, -90);
                targetRotation *= additionalRotation;
                transform.rotation = targetRotation;
            }
            if (rigidBodyToMove != null)
            {
                Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, speed * Time.deltaTime);
                rigidBodyToMove.MovePosition(newposition);
                remaindistance = (transform.position - position).sqrMagnitude;
            }
            if(targetTransform != null && Vector2.Distance(rigidBodyToMove.position, position) < 0.05f)
            {
                Destroy(this.gameObject);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player.instance.Damaged(-damage);
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("Platform"))
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
    }
}
