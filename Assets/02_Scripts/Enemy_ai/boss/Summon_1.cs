using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon_1 : MonoBehaviour
{
    public float speed;
    public float position_change_second;
    public float delete_time;

    CircleCollider2D circle;
    Rigidbody2D rigid;
    Transform targetTransform = null;
    Vector3 position;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        Debug.DrawLine(rigid.position, position, Color.red);
    }

    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        Destroy(this.gameObject, delete_time);
        float remaindistance = (transform.position - position).sqrMagnitude;
        while (remaindistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                position = targetTransform.position;
                Vector2 direction = new Vector2(transform.position.x - targetTransform.position.x, transform.position.y - targetTransform.position.y);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                Quaternion rotation = Quaternion.Slerp(targetTransform.rotation, angleAxis, speed);
                transform.rotation = rotation;
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

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetTransform = collision.gameObject.transform;
            StartCoroutine(Move(rigid, speed));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
