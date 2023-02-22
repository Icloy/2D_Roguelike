using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon_2 : MonoBehaviour
{
    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Platform"))
        {
            Destroy(this.gameObject);
        }
    }
}
