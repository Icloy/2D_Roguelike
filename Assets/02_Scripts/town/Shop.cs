using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public GameObject shopPanel;

    private bool endStore = false;
    private int dmgPrice = 1;
    public Text dmgPriceText;

    GameManager gameManager;
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("충돌중");
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(collision.gameObject.tag == "Player")
            {
                shopPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
/*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine("OpenStore");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("상점 끝");
        endStore = true;
        StopCoroutine("OpenStore");
    }

    private IEnumerator OpenStore() 
    {
        while (endStore)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Time.timeScale = 0f;
                shopPanel.SetActive(true);
            }
            yield return null;
        }
    }*/

    public void CloseStore()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpgradeDmg()
    {
        if(gameManager.coin >= dmgPrice)
        {
            gameManager.coin -= dmgPrice;
            gameManager.UpdateCoinCnt();
            player.AtDmg++;
            Debug.Log("무기 업그레이드!");
            dmgPrice += 2;
            dmgPriceText.text = dmgPrice.ToString() + " 코인";
        }
        else
        {
            Debug.Log("돈이 부족합니다!");
        }
    }



}
