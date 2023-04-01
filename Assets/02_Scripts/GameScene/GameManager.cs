using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class GameManager : MonoBehaviour
{
    #region
    public Camera mainCamera;
    public Camera subCamera;

    public Transform alivePos;  //��Ƴ� ��ġ
    public GameObject soul;     //�÷��̾ �׾����� ����� ������Ʈ

    public int coin;    //�ΰ��� ��ȭ

    public bool isGameOver = false; //���ӿ��� ����
    public bool isPanelOpen = false; //�г� ���� ����
    public bool isShopOpen = false; //���� ���� ����
    public bool isMapOpen = false; //�� ���� ����
    public bool remainSoul = false; //�ذ��� ���� �ʿ��ִ���

    GameUI gameUI;
    Shop shop;

    public static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        gameUI = GameObject.Find("Canvas").GetComponent<GameUI>();
        shop = GameObject.Find("Shop").GetComponent<Shop>();
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        //�ʱ� ī�޶� ��
        subCamera.enabled = false;
        mainCamera.enabled = true;

        UpdateCoinCnt(0); // ���� �ε�� �ʿ�
    }

    void Update()
    {
        //esc�� �ԷµǸ� ������ ������Ű�� �ɼ�â�� ����.
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isShopOpen)
            {
                shop.CloseStore();
                return;
            }
            else if (isMapOpen)
            {
                gameUI.Map();
                return;
            }
            else if (gameUI.optionPanel.activeSelf)
            {
                gameUI.OptionBtn();
                return;
            }
            gameUI.PasueGame();
        }

        if (Input.GetKeyDown(KeyCode.M) && !isGameOver &&!isShopOpen)
        {
            gameUI.Map();
        }
    }

    public void UpdateCoinCnt(int inc) //���� �� ������Ʈ
    {
        coin += inc;

        //������ ����� ������ �ƴ϶��
        if(coin >= 0)
        {
            gameUI.coinCnt.text = coin.ToString();
        }
        else
        {
            coin = 0;
            gameUI.coinCnt.text = coin.ToString();
        }
    }

    public void PlayerDead()
    {
        if (!isGameOver)
        {
            return;
        }

        if (remainSoul == true) //�̹� �׾��� ���� �ִٸ�
        {
            DeadSoul deadSoul = GameObject.Find("soul(Clone)").GetComponent<DeadSoul>();
            deadSoul.duplicate();
        }

        //���� �����
        Instantiate(soul, Player.instance.transform.position, Quaternion.identity);
        remainSoul = true;

        isPanelOpen = true;
        gameUI.gameoverPanel.SetActive(true);
    }
}
