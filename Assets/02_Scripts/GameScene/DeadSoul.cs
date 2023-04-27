using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeadSoul : MonoBehaviour
{
    private int lostCoin; //�Ҿ���� ���μ�
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
        GameManager.instance.UpdateCoinCnt(-9999999); //������ ���ΰ�꿡�� ������ �������� 0���� ���
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            ToastMsg.Instance.showMessage("��� �Ͻ÷��� G�� �����ּ���!", 1f);
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
        //���̵�� �������� fix
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
