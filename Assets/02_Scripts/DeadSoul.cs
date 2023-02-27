using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSoul : MonoBehaviour
{

    public int lostCoin; //�Ҿ���� ���μ�
    GameManager gamemanager;

    private float gageTime = 2f;
    private float curTime;

    private void Awake()
    {
        gamemanager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void Start()
    {
        lostCoin = gamemanager.coin;
        gamemanager.coin = 0;
        gamemanager.UpdateCoinCnt();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            ToastMsg.Instance.showMessage("��� �Ͻ÷��� G�� �����ּ���!", 1f);
            StartCoroutine("Consume");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StopCoroutine("Consume");
    }


    private IEnumerator Consume()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.G))
            {
                curTime += Time.deltaTime;
                if(gageTime <= curTime)
                {
                    gamemanager.coin += lostCoin;
                    gamemanager.UpdateCoinCnt();
                    Destroy(gameObject);
                }
            }
            else
            {
                curTime = 0;
            }
            yield return null;
        }
    }
}
