using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu_Manager : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject slotPanel;
    public Button SelBtn;
    public TMP_Text[] Slot;

    List<Resolution> resolutions = new List<Resolution>(); //모니터가 지원하는 해상도를 저장할 배열
    FullScreenMode screenMode;
    public Toggle fullscreenBtn;
    public Dropdown resolutionDropdown;
    public int resolutionNum;

    public GetName getname;

    private void Awake()
    {
        getname = GetComponent<GetName>();
    }

    private void Start()
    {
        Resolution();
        SelBtn.Select();
        Singleton.Instance.slotNum = -1;
    }

    public void GameBtnClick() //Game버튼 클릭시 slot패널 오픈
    {
        slotPanel.SetActive(true);
        getname.Refresh();  //패널 새로고침 DB불러옴
    }

    public void SlotBtnClick(int a) //a가 슬롯 번호
    {
        Singleton.Instance.slotNum = a;
        if (Slot[a].text == "플레이 기록 없음!")
        {
            Singleton.Instance.newGame = true;
        }
        else
        {
            Singleton.Instance.newGame = false;
        }
        LoadingScene.LoadScene("Game01_Scene");
    }

    public void Gamestart()
    {
        LoadingScene.LoadScene("Game01_Scene");
    }


    public void SlotExit()
    {
        slotPanel.SetActive(false);
    }


    #region option
    void Resolution() //해상도 설정
    {
        if (screenMode == FullScreenMode.FullScreenWindow)  //전체화면일 경우  토글 체크용
        {
            fullscreenBtn.isOn = true;
        }
        else
        {
            fullscreenBtn.isOn = false;
        }

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
             resolutions.Add(Screen.resolutions[i]);
        }

        resolutionDropdown.options.Clear();
        int optionNum = 0;

        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " X " + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
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
    #endregion


    public void OptionBtnClick() //Option버튼 클릭시와 Back버튼 클릭시 
    {
        if (optionPanel.activeSelf == true) //옵션패널이 켜져있다면 끄고 꺼져있다면 킨다.
        {
            optionPanel.SetActive(false); //옵션창 종료
        }
        else
        {
            Resolution(); //해상도 새로고침
            optionPanel.SetActive(true);
        }
    }

    public void OptionApplyBtnClick() //옵션창의 Apply버튼 클릭시
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode); //변경된 옵션을 설정한다.
        optionPanel.SetActive(false);
    }

    

    public void exitBtnClick() // exit버튼 클릭시 게임 종료 처리
    {
        Application.Quit();
    }
}
