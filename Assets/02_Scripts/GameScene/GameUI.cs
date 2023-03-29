using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEditor.Experimental.GraphView;

public class GameUI : MonoBehaviour
{
    #region
    public GameObject mapPanel; //맵 패널(m)
    public GameObject pausePanel; //퍼즈 패널(esc)
    public GameObject optionPanel; //옵션 패널()
    public GameObject gameoverPanel; //게임 오버 패널()

    public GameObject optionGraphic; //옵션 그래픽 패널
    public GameObject optionSound; //옵션 사운드 패널

    public Button pausePanelSelBtn; //초기 선택되어있는 버튼(키보드용)
    public Button optionPanelSelBtn; //초기 선택되어있는 버튼(키보드용)

    public Dropdown resolutionDropdown; //해상도 저장할 드랍다운
    public Text coinCnt;    //코인 카운트 텍스트

    List<Resolution> resolutions = new List<Resolution>(); //모니터가 지원하는 해상도를 저장할 배열
    FullScreenMode screenMode;
    int resolutionNum;
    
    Animator anim;
    GameObject minimap;
    RectTransform rectminimap;
    #endregion
    private void Awake()
    {
        minimap = transform.GetChild(1).gameObject;
        anim = minimap.GetComponent<Animator>();
        rectminimap = minimap.GetComponent<RectTransform>();
    }

    public void PasueGame() //pausePanel (esc)
    {
        if (!pausePanel.activeSelf)
        {
            Time.timeScale = 0;
            GameManager.instance.isPanelOpen = true;
            pausePanel.SetActive(true);
            pausePanelSelBtn.Select();
        }
        else 
        {
            Time.timeScale = 1.0f;
            GameManager.instance.isPanelOpen = false;
            pausePanel.SetActive(false);
        }
    }

    public void ResumeBtn() //optionpanel 
    {
        PasueGame();
    }

    public void ExitBtn() //optionpanel
    {
        SceneManager.LoadScene("Menu_Scene");
    }

    public void OptionBtn() //Option Panel
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

    public void BackBtn() //Option
    {
        Searchpanel();
        OptionBtn();
    }

    #region graphic
    public void GraphicBtn()
    {
        Searchpanel();
        optionGraphic.SetActive(true);

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
    #endregion graphic

    public void SoundBtn() //Option
    {
        Searchpanel();
        optionSound.SetActive(true);
    }
    public void ApplyBtn() //Option
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
        Searchpanel();
    }

    #region Map
    public void Map()
    {
        if (!mapPanel.activeSelf && !GameManager.instance.isPanelOpen)
        {
            //anim.Play("MiniMap");
            StartCoroutine("Map2");
        }
        else
        {
            mapPanel.SetActive(false);
            //minimap.SetActive(true);
            GameManager.instance.isMapOpen = false;
        }
    }
    IEnumerator Map2()
    {
            yield return new WaitForSeconds(0.1f);
            mapPanel.SetActive(true);
            //rectminimap.localScale = new Vector3(1f, 1f, 1f);
            //minimap.SetActive(false);
            GameManager.instance.isMapOpen = true;
    }
    #endregion
    public void ContinueBtn()   //GameOver
    {
        GameManager.instance.isGameOver = false;
        Player.instance.gameObject.SetActive(true);
        Player.instance.transform.position = GameManager.instance.alivePos.position;
        Player.instance.curHp = 3;
        Player.instance.maxHp = 3;
        Hp.instance.udtHp(Player.instance.curHp, Player.instance.maxHp);
        GameManager.instance.isPanelOpen = false;
        gameoverPanel.SetActive(false);
    }

    private void Searchpanel() //켜져 있는 패널 검색용
    {
        if (optionGraphic.activeSelf || optionSound.activeSelf)
        {
            optionGraphic.SetActive(false);
            optionSound.SetActive(false);
        }
    }
}
