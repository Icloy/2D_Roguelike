using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; //게임정지패널
    public Button pausePanelSelBtn; //초기 선택되어있는 버튼(키보드용)
    public GameObject optionPanel; //게임정지패널
    public Button optionPanelSelBtn; //초기 선택되어있는 버튼(키보드용)
    public GameObject GameOverPanel; //게임오버패널
    public GameObject graphicOptionPanel;
    public GameObject soundOptionPanel;

    public Dropdown resolutionDropdown;

    public Text coinCnt;
    public int coin; //인게임 재화

    List<Resolution> resolutions = new List<Resolution>(); //모니터가 지원하는 해상도를 저장할 배열
    public int resolutionNum;
    FullScreenMode screenMode;

    private bool isGameOver; //게임오버여부 판단
    
    public bool isPanelOpen = false;
    public bool isShopOpen = false;


    Player player;

    private void Start()
    {
        isGameOver = false;
        player = GameObject.Find("Player").GetComponent<Player>();
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

        if(player.curHp <= 0) //게임오버처리
        {
            isGameOver = true;
            Time.timeScale = 0;
            isPanelOpen = true;
            GameOverPanel.SetActive(true);
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

    void searchpanel() //켜져 있는 패널 검색용
    {
        if (graphicOptionPanel.activeSelf || soundOptionPanel.activeSelf)
        {
            graphicOptionPanel.SetActive(false);
            soundOptionPanel.SetActive(false);
        }
    }

    public void UpdateCoinCnt() //코인 수 업데이트
    {
        //코인의 계수가 음수가 아니라면
        if(coin >= 0)
        {
            coinCnt.text = coin.ToString();
        }
        else
        {
            coinCnt.text = "0";
        }
    }
}
