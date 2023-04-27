using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeadSoul : MonoBehaviour
{
    private int lostCoin; //잃어버린 코인수
    public int dmgLv;
    public int hpLv;

    private float gageTime = 2f;
    private float curTime;

    private void Awake()
    {
    }

    private void Start()
    {
        lostCoin = GameManager.instance.coin;
        dmgLv = Player.instance.AtDmg;
        hpLv = Player.instance.maxHp;
        Player.instance.AtDmg = 1;
        Player.instance.maxHp = 300;
        GameManager.instance.UpdateCoinCnt(-9999999); //어차피 코인계산에서 음수로 내려가면 0으로 계산
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            ToastMsg.Instance.showMessage("흡수 하시려면 G를 눌러주세요!", 1f);
            StartCoroutine("Consume");
        }
        else if(!(col.gameObject.GetComponent<Collider2D>() != null))
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }

    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        { 
            StopCoroutine("Consume");
        }
    }

    public void duplicate()
    {
        //아이디어 생각나면 fix
        GameManager.instance.UpdateCoinCnt(lostCoin);
        Player.instance.AtDmg = dmgLv;
        Player.instance.maxHp = hpLv;
        Hp.instance.udtHp(Player.instance.curHp, Player.instance.maxHp);

        Destroy(gameObject);
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
                    Player.instance.AtDmg = dmgLv;
                    Player.instance.maxHp = hpLv;
                    Hp.instance.udtHp(Player.instance.curHp, Player.instance.maxHp);
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
