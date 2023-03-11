using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSoul : MonoBehaviour
{
    Player player;
    Shop shop;

    private int lostCoin; //잃어버린 코인수
    private int dmgLvl;
    private int hpLvl;

    private float gageTime = 2f;
    private float curTime;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        shop = GameObject.Find("Shop").GetComponent<Shop>();
    }

    private void Start()
    {
        lostCoin = GameManager.instance.coin;
        dmgLvl = player.AtDmg;
        hpLvl = player.maxHp;
        player.AtDmg = 1;
        player.maxHp = 300;
        GameManager.instance.UpdateCoinCnt(-9999999);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            ToastMsg.Instance.showMessage("흡수 하시려면 G를 눌러주세요!", 1f);
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
                    GameManager.instance.UpdateCoinCnt(lostCoin);
                    player.AtDmg = dmgLvl;
                    player.maxHp = hpLvl;
                    Destroy(gameObject);
                    GameManager.instance.remainSoul = false;
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
