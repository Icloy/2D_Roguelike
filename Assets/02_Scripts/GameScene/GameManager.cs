using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region
    public Camera mainCamera;
    public Camera subCamera;

    public GameObject pausePanel; //���������г�
    public Button pausePanelSelBtn; //�ʱ� ���õǾ��ִ� ��ư(Ű�����)
    public GameObject optionPanel; //���������г�
    public Button optionPanelSelBtn; //�ʱ� ���õǾ��ִ� ��ư(Ű�����)
    public GameObject GameOverPanel; //���ӿ����г�
    public GameObject MapPanel; //�� �г�
    public GameObject graphicOptionPanel;
    public GameObject soundOptionPanel;

    public Dropdown resolutionDropdown;

    public Transform alivePos;  //��Ƴ� ��ġ
    public GameObject soul;     //�÷��̾ �׾����� ����� ������Ʈ
    public bool remainSoul = false; //�ذ��� ���� �ʿ��ִ���

    public Text coinCnt;    //���� ī��Ʈ �ؽ�Ʈ
    public int coin;    //�ΰ��� ��ȭ

    List<Resolution> resolutions = new List<Resolution>(); //����Ͱ� �����ϴ� �ػ󵵸� ������ �迭
    int resolutionNum;
    FullScreenMode screenMode;

    public bool isGameOver; //���ӿ������� �Ǵ�
    public bool isPanelOpen = false; //�г� ���� ���� �Ǵ�
    public bool isShopOpen = false; //���� ���� ���� �Ǵ�


    public static GameManager instance = null;
    #endregion




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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        isGameOver = false;
       
        //�ʱ� ī�޶� ��
        subCamera.enabled = false;
        mainCamera.enabled = true;

        UpdateCoinCnt(0); // ���� �ε�� �ʿ�
    }

    void Update()
    {
        //esc�� �ԷµǸ� ������ ������Ű�� �ɼ�â�� ����.
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !isShopOpen)
        {
            if (optionPanel.activeSelf)
            {
                optionBtn();
                return;
            }
            pasueGame();
        }
        if (Input.GetKeyDown(KeyCode.M) && !isGameOver)
        {
            Map();
        }
    }

    void Map()
    {
        if (!MapPanel.activeSelf) //�г��� �������� �ʴٸ�
        {
            MapPanel.SetActive(true);
        }
        else
        {
            MapPanel.SetActive(false);
        }
    }

    void pasueGame() //esc�� �������� ����Ǵ� �Լ�
    {
        if (!pausePanel.activeSelf)
        {
            Time.timeScale = 0; //���� �ð� ����
            isPanelOpen = true;
            pausePanel.SetActive(true);
            pausePanelSelBtn.Select();
        }
        else //������ �̹� ���� �Ǿ��ִٸ�
        {
            Time.timeScale = 1.0f; //�ð� ���� ����
            isPanelOpen = false;
            pausePanel.SetActive(false);
        }
    }

    public void resumeBtn() //�ٽ� ����
    {
        pasueGame();
    }

    public void optionBtn() //�ɼ� ����â
    {
        if (!optionPanel.activeSelf)
        {
            pausePanel.SetActive(false);
            optionPanel.SetActive(true);
            optionPanelSelBtn.Select();
        }
        else 
        {
            optionPanel.SetActive(false);
            pausePanel.SetActive(true);
            pausePanelSelBtn.Select();
        }
    }

    public void ExitBtn() //���� ȭ������
    {
        SceneManager.LoadScene("Menu_Scene");
    }

    public void backBtn()
    {
        searchpanel();
        optionBtn();
    }

    public void graphicBtn()
    {
        searchpanel();
        graphicOptionPanel.SetActive(true);

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i]);
        }

        resolutionDropdown.options.Clear();
        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " X " + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
    }

    public void FullScreenBtn(bool isFull) //��üȭ�� ��� 
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }


    public void DropboxOptionChange(int x) //�ػ� ����ڽ����� �����Ѱ� �����
    {
        resolutionNum = x;
    }

    public void soundBtn()
    {
        searchpanel();
        soundOptionPanel.SetActive(true);
    }

    public void applyBtn()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
        searchpanel();
    }

    public void continueBtn()   //GameOver�г� ������ ��ư
    {
        isGameOver = false;
        Player.instance.gameObject.SetActive(true);
        Player.instance.transform.position = alivePos.position;
        Player.instance.curHp = 3;
        Player.instance.maxHp = 3;
        Hp.instance.udtHp(Player.instance.curHp, Player.instance.maxHp);
        isPanelOpen = false;
        GameOverPanel.SetActive(false);
    }

    private void searchpanel() //���� �ִ� �г� �˻���
    {
        if (graphicOptionPanel.activeSelf || soundOptionPanel.activeSelf)
        {
            graphicOptionPanel.SetActive(false);
            soundOptionPanel.SetActive(false);
        }
    }

    public void UpdateCoinCnt(int inc) //���� �� ������Ʈ
    {
        coin += inc;

        //������ ����� ������ �ƴ϶��
        if(coin >= 0)
        {
            coinCnt.text = coin.ToString();
        }
        else
        {
            coin = 0;
            coinCnt.text = coin.ToString();
        }
    }
    public void PlayerDead()
    {
        if (!isGameOver)
        {
            return;
        }
        

        //���� �����
        Instantiate(soul, Player.instance.transform.position, Quaternion.identity);
        remainSoul = true;
        //Time.timeScale = 0; //���� �ð� ���� - ���� ����

        isPanelOpen = true;
        GameOverPanel.SetActive(true);
    }
}
