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

    public GameObject pausePanel; //게임정지패널
    public Button pausePanelSelBtn; //초기 선택되어있는 버튼(키보드용)
    public GameObject optionPanel; //게임정지패널
    public Button optionPanelSelBtn; //초기 선택되어있는 버튼(키보드용)
    public GameObject GameOverPanel; //게임오버패널
    public GameObject MapPanel; //맵 패널
    public GameObject graphicOptionPanel;
    public GameObject soundOptionPanel;

    public Dropdown resolutionDropdown;

    public Transform alivePos;  //살아날 위치
    public GameObject soul;     //플레이어가 죽었을때 드랍할 오브젝트
    public bool remainSoul = false; //해골이 아직 맵에있는지

    public Text coinCnt;    //코인 카운트 텍스트
    public int coin;    //인게임 재화

    List<Resolution> resolutions = new List<Resolution>(); //모니터가 지원하는 해상도를 저장할 배열
    int resolutionNum;
    FullScreenMode screenMode;

    public bool isGameOver; //게임오버여부 판단
    public bool isPanelOpen = false; //패널 오픈 여부 판단
    public bool isShopOpen = false; //상점 오픈 여부 판단


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
       
        //초기 카메라 값
        subCamera.enabled = false;
        mainCamera.enabled = true;

        UpdateCoinCnt(0); // 게임 로드시 필요
    }

    void Update()
    {
        //esc가 입력되면 게임을 정지시키고 옵션창을 띄운다.
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
        if (!MapPanel.activeSelf) //패널이 열려있지 않다면
        {
            MapPanel.SetActive(true);
        }
        else
        {
            MapPanel.SetActive(false);
        }
    }

    void pasueGame() //esc가 눌렸을때 실행되는 함수
    {
        if (!pausePanel.activeSelf)
        {
            Time.timeScale = 0; //게임 시간 정지
            isPanelOpen = true;
            pausePanel.SetActive(true);
            pausePanelSelBtn.Select();
        }
        else //게임이 이미 정지 되어있다면
        {
            Time.timeScale = 1.0f; //시간 원상 복귀
            isPanelOpen = false;
            pausePanel.SetActive(false);
        }
    }

    public void resumeBtn() //다시 시작
    {
        pasueGame();
    }

    public void optionBtn() //옵션 세부창
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

    public void ExitBtn() //메인 화면으로
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

    public void FullScreenBtn(bool isFull) //전체화면 토글 
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }


    public void DropboxOptionChange(int x) //해상도 드랍박스에서 설정한값 저장용
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

    public void continueBtn()   //GameOver패널 마을로 버튼
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

    private void searchpanel() //켜져 있는 패널 검색용
    {
        if (graphicOptionPanel.activeSelf || soundOptionPanel.activeSelf)
        {
            graphicOptionPanel.SetActive(false);
            soundOptionPanel.SetActive(false);
        }
    }

    public void UpdateCoinCnt(int inc) //코인 수 업데이트
    {
        coin += inc;

        //코인의 계수가 음수가 아니라면
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
        

        //흔적 남기기
        Instantiate(soul, Player.instance.transform.position, Quaternion.identity);
        remainSoul = true;
        //Time.timeScale = 0; //게임 시간 정지 - 삭제 예정

        isPanelOpen = true;
        GameOverPanel.SetActive(true);
    }
}
