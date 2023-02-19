using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    CircleCollider2D circle;
    Rigidbody2D rigid;

    Coroutine actcoroutine;

    public int actmove;
    public GameObject Player;
    public GameObject Boss;
    public GameObject trap;

    private float dis;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
    }

    public IEnumerator Think()
    {
        while (true)
        {
            dis = Vector2.Distance(Player.transform.position, Boss.transform.position);
            actmove = Random.Range(1, 3);
            if (dis <= 2.5)
            {
                StartCoroutine(act1(actmove));
            }
            else if (dis <= 5)
            {
                StartCoroutine(act2(actmove));
            }
            else
            {
                for (int i = -2; i < 3; i++)
                {
                    Instantiate(trap, new Vector3(Player.transform.position.x + 1f * i, Player.transform.position.y + 4f, Player.transform.position.z), Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(3f);
        }
       
    }

    public IEnumerator act1(int actmove)
    {
        Debug.Log("act1");
        switch (actmove)
        {
            case 1:
                break;
            case 2:
                break;
        }
        yield return new WaitForSeconds(3f);
    }

    public IEnumerator act2(int actmove)
    {
        switch (actmove)
        {
            case 1:
                break;
            case 2:
                break;
        }
        yield return new WaitForSeconds(3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Think());
        }
    }
}