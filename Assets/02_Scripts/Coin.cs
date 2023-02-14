using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody2D rigid;
    public float updistance;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        rigid.AddForce(Vector2.up * updistance, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.coin++;
            gameManager.UpdateCoinCnt(); //코인계수 업데이트
        }
    }
}
