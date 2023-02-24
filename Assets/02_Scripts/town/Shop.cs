using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public GameObject shopPanel;
    public Button selBtn;
    public Button selHPBtn;


    private bool endStore = false;
    private int dmgPrice = 1;
    private int HPPrice = 2;
    public Text dmgPriceText;
    public Text HpPriceText;


    GameManager gameManager;
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

  /*  private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(collision.gameObject.tag == "Player")
            {
                gameManager.isPanelOpen = true;
                shopPanel.SetActive(true);
            }
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("���� ��");
            StopCoroutine("OpenStore");
            endStore = false;
            gameManager.isPanelOpen = false;
            shopPanel.SetActive(false);
        }
    }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                endStore = true;
                StartCoroutine("OpenStore");
            }
        }

        private IEnumerator OpenStore() 
        {
            while (endStore)
            {
                
                if (Input.GetKeyDown(KeyCode.G))
                {
                    gameManager.isPanelOpen = true;
                    shopPanel.SetActive(true);
                    selBtn.Select();
                    selHPBtn.Select();

                }
                yield return null;
            }
        }

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
            Debug.Log("���� ���׷��̵�!");
            dmgPrice += 2;
            dmgPriceText.text = dmgPrice.ToString() + " ����";
        }
        else
        {
            ToastMsg.Instance.showMessage("���� �����մϴ�!", 0.5f);
            Debug.Log("���� �����մϴ�!");
        }
    }

    public void UpgradeHP()
    {
        if (gameManager.coin >= HPPrice && player.maxHp < 600)
        {
            gameManager.coin -= HPPrice;
            gameManager.UpdateCoinCnt();
            player.maxHp += 100;
            Debug.Log("ü�� ��ĭ ���׷��̵�!");
            HPPrice += 2;
            HpPriceText.text = HPPrice.ToString() + " ����";
        }
        else if(player.maxHp >= 600)
        {
            Debug.Log("�ִ�ü���Դϴ�");
        }
        else
        {
            ToastMsg.Instance.showMessage("���� �����մϴ�!", 0.5f);
            Debug.Log("���� �����մϴ�!");
        }
    }



}
