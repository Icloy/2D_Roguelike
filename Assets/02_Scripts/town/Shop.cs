using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Camera mainCamera;
    public Camera subCamera;
    public Transform target; //줌인 할 대상

    public GameObject shopPanel;
    public Button selBtn;
    public Button selHPBtn;

    //public GameObject Stat;

    private bool endStore = false;
    private int dmgPrice = 1;
    public Text dmgPriceText;

    private int hpPrice = 2;
    public Text hpPriceText;

    GameManager gameManager;
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& gameManager.isShopOpen)
        {
            CloseStore();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine("OpenStore");
            CloseStore();
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
                gameManager.isShopOpen = true;
                shopPanel.SetActive(true);
                selBtn.Select();
                selHPBtn.Select();
                mainCamera.enabled = false;
                subCamera.enabled= true;
                subCamera.transform.position = new Vector3(target.transform.position.x,target.transform.position.y ,-10);
                subCamera.orthographicSize = 3.6f;
            }
            yield return null;
        }
    }

    private void CloseStore()
    {
        endStore = true;
        gameManager.isShopOpen = false;
        shopPanel.SetActive(false);
        subCamera.enabled = false;
        mainCamera.enabled = true;
        subCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        subCamera.orthographicSize = 9f;
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
            ToastMsg.Instance.showMessage("돈이 부족합니다!", 0.5f);
            Debug.Log("돈이 부족합니다!");
        }
    }

    public void UpgradeHP()
    {
        if (gameManager.coin >= hpPrice && player.maxHp < 600)
        {
            gameManager.coin -= hpPrice;
            gameManager.UpdateCoinCnt();
            player.maxHp += 100;
            Debug.Log("체력 한칸 증가!");
            hpPrice += 2;
            hpPriceText.text = hpPrice.ToString() + " 코인";
        }
        else if (player.maxHp >= 600)
        {
            Debug.Log("최대체력입니다");
        }
        else
        {
            ToastMsg.Instance.showMessage("돈이 부족합니다!", 0.5f);
            Debug.Log("돈이 부족합니다!");
        }
    }
}
