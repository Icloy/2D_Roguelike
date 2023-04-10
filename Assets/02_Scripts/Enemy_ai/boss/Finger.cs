using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    public float speed;
    public float position_change_second;
    public float delete_time;

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
                position = targetTransform.position;
                Vector2 direction = position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                Quaternion additionalRotation = Quaternion.Euler(0, 0, -90);
                targetRotation *= additionalRotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
            }
            if (rigidBodyToMove != null)
            {
                Vector3 newposition = Vector3.MoveTowards(rigidBodyToMove.position, position, speed * Time.deltaTime);
                rigidBodyToMove.MovePosition(newposition);
                remaindistance = (transform.position - position).sqrMagnitude;
            }
            if(Vector2.Distance(rigidBodyToMove.position, targetTransform.position) < 0.01f)
            {
                Destroy(this.gameObject);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RightTarget"))
        {
            targetTransform = collision.gameObject.transform;
            StartCoroutine(Move(rigid, speed));
        }
    }
}
