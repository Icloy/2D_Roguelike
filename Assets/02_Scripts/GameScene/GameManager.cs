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

    public Transform alivePos;  //살아날 위치
    public GameObject soul;     //플레이어가 죽었을때 드랍할 오브젝트

    public int coin;    //인게임 재화

    public bool isGameOver = false; //게임오버 여부
    public bool isPanelOpen = false; //패널 오픈 여부
    public bool isShopOpen = false; //상점 오픈 여부
    public bool isMapOpen = false; //맵 오픈 여부
    public bool remainSoul = false; //해골이 아직 맵에있는지

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
        //초기 카메라 값
        subCamera.enabled = false;
        mainCamera.enabled = true;

        UpdateCoinCnt(0); // 게임 로드시 필요
    }

    void Update()
    {
        //esc가 입력되면 게임을 정지시키고 옵션창을 띄운다.
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

    public void UpdateCoinCnt(int inc) //코인 수 업데이트
    {
        coin += inc;

        //코인의 계수가 음수가 아니라면
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

        if (remainSoul == true) //이미 죽었던 적이 있다면
        {
            DeadSoul deadSoul = GameObject.Find("soul(Clone)").GetComponent<DeadSoul>();
            deadSoul.duplicate();
        }

        //흔적 남기기
        Instantiate(soul, Player.instance.transform.position, Quaternion.identity);
        remainSoul = true;

        isPanelOpen = true;
        gameUI.gameoverPanel.SetActive(true);
    }
}
