using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Camera mainCamera;
    public Camera subCamera;
    public Transform target; //���� �� ���

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


    private void Awake()
    {
    }


    private void Start()
    {
    }

    private void Update()
    {
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
                GameManager.instance.isShopOpen = true;
                shopPanel.SetActive(true);
                selBtn.Select();
                selHPBtn.Select();
                mainCamera.enabled = false;
                subCamera.enabled= true;
                subCamera.transform.position = new Vector3(target.transform.position.x,target.transform.position.y ,-10);
                subCamera.orthographicSize = 4.7f;
            }
            yield return null;
        }
    }

    public void CloseStore()
    {
        endStore = true;
        GameManager.instance.isShopOpen = false;
        shopPanel.SetActive(false);
        subCamera.enabled = false;
        mainCamera.enabled = true;
        subCamera.transform.position = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y, -10);
        subCamera.orthographicSize = 9f;
    }

    public void UpgradeDmg()
    {
        if (GameManager.instance.remainSoul)
        {
            ToastMsg.Instance.showMessage("��ȥ�� ����ϼž� ��ȭ�� �� �� �ֽ��ϴ�!", 0.5f);
            return;
        }
        if (GameManager.instance.coin >= dmgPrice)
        {
            GameManager.instance.UpdateCoinCnt(-dmgPrice);
            Player.instance.AtDmg++;
            dmgLvl++;
            dmgPrice += 2;
            ToastMsg.Instance.showMessage("���� ���׷��̵�!", 0.5f);
            dmgPriceText.text = dmgPrice.ToString() + " ����";
        }
        else
        {
            ToastMsg.Instance.showMessage("���� �����մϴ�!", 0.5f);
        }
    }

    public void UpgradeHP()
    {
        if (GameManager.instance.remainSoul)
        {
            ToastMsg.Instance.showMessage("��ȥ�� ����ϼž� ��ȭ�� �� �� �ֽ��ϴ�!", 0.5f);
            return;
        }
        if (GameManager.instance.coin >= hpPrice && Player.instance.maxHp < 7)
        {
            GameManager.instance.UpdateCoinCnt(-hpPrice);
            Player.instance.maxHp++;
            Hp.instance.buyHp(Player.instance.maxHp);
            Player.instance.Damaged(1);
            hpLvl++;
            hpPrice += 2;
            ToastMsg.Instance.showMessage("ü�� ��ĭ ����!", 0.5f);
            hpPriceText.text = hpPrice.ToString() + " ����";
        }
        else if (Player.instance.maxHp >= 7)
        {
            ToastMsg.Instance.showMessage("�ִ�ü���Դϴ�!", 0.5f);
        }
        else
        {
            ToastMsg.Instance.showMessage("���� �����մϴ�!", 0.5f);
        }
    }
}
