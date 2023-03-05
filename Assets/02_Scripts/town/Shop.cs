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

    public Text dmgPriceText;
    public int dmgLvl = 1;
    private int dmgPrice = 1;

    public Text hpPriceText;
    public int hpLvl = 1;
    private int hpPrice = 2;


    GameManager gameManager;
    Player player;
    Hp hp;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        hp = GameObject.Find("Hp").GetComponent<Hp>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& gameManager.isShopOpen)
        {
            CloseStore();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            endStore = true;
            StartCoroutine("OpenStore");
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
        if (gameManager.remainSoul)
        {
            ToastMsg.Instance.showMessage("영혼을 흡수하셔야 강화를 할 수 있습니다!", 0.5f);
            return;
        }
        if (gameManager.coin >= dmgPrice)
        {
            gameManager.UpdateCoinCnt(-dmgPrice);
            player.AtDmg++;
            dmgLvl++;
            dmgPrice += 2;
            ToastMsg.Instance.showMessage("무기 업그레이드!", 0.5f);
            dmgPriceText.text = dmgPrice.ToString() + " 코인";
        }
        else
        {
            ToastMsg.Instance.showMessage("돈이 부족합니다!", 0.5f);
        }
    }

    public void UpgradeHP()
    {
        if (gameManager.remainSoul)
        {
            ToastMsg.Instance.showMessage("영혼을 흡수하셔야 강화를 할 수 있습니다!", 0.5f);
            return;
        }
        if (gameManager.coin >= hpPrice && player.maxHp < 7)
        {
            gameManager.UpdateCoinCnt(-hpPrice);
            player.maxHp++;
            hp.buyHp(player.maxHp);
            player.Damaged(1);
            hpLvl++;
            hpPrice += 2;
            ToastMsg.Instance.showMessage("체력 한칸 증가!", 0.5f);
            hpPriceText.text = hpPrice.ToString() + " 코인";
        }
        else if (player.maxHp >= 7)
        {
            Debug.Log("최대체력입니다");
        }
        else
        {
            ToastMsg.Instance.showMessage("돈이 부족합니다!", 0.5f);
        }
    }
}
