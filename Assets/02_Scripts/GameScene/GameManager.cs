using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; //게임정지패널
    public GameObject optionPanel; //게임정지패널
    public GameObject graphicOptionPanel;
    public GameObject soundOptionPanel;

    public Dropdown resolutionDropdown;


    List<Resolution> resolutions = new List<Resolution>(); //모니터가 지원하는 해상도를 저장할 배열
    public int resolutionNum;
    FullScreenMode screenMode;

    void Update()
    {
        //esc가 입력되면 게임을 정지시키고 옵션창을 띄운다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.activeSelf)
            {
                optionBtn();
                return;
            }
            pasueGame();
        }
    }

    void pasueGame() //esc가 눌렸을때 실행되는 함수
    {
        if (!pausePanel.activeSelf)
        {
            Time.timeScale = 0; //게임 시간 정지
            pausePanel.SetActive(true);
        }
        else //게임이 이미 정지 되어있다면
        {
            Time.timeScale = 1.0f; //시간 원상 복귀
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
        }
        else 
        {
            optionPanel.SetActive(false);
            pausePanel.SetActive(true);
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

}
