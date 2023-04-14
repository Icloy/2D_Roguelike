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

    List<Resolution> resolutions = new List<Resolution>(); //����Ͱ� �����ϴ� �ػ󵵸� ������ �迭
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

    public void GameBtnClick() //Game��ư Ŭ���� slot�г� ����
    {
        slotPanel.SetActive(true);
        getname.Refresh();  //�г� ���ΰ�ħ DB�ҷ���
    }

    public void SlotBtnClick(int a) //a�� ���� ��ȣ
    {
        Singleton.Instance.slotNum = a;
        if (Slot[a].text == "�÷��� ��� ����!")
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
    void Resolution() //�ػ� ����
    {
        if (screenMode == FullScreenMode.FullScreenWindow)  //��üȭ���� ���  ��� üũ��
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

    public void FullScreenBtn(bool isFull) //��üȭ�� ��� 
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void DropboxOptionChange(int x) //�ػ� ����ڽ����� �����Ѱ� �����
    {
        resolutionNum = x;
    }
    #endregion


    public void OptionBtnClick() //Option��ư Ŭ���ÿ� Back��ư Ŭ���� 
    {
        if (optionPanel.activeSelf == true) //�ɼ��г��� �����ִٸ� ���� �����ִٸ� Ų��.
        {
            optionPanel.SetActive(false); //�ɼ�â ����
        }
        else
        {
            Resolution(); //�ػ� ���ΰ�ħ
            optionPanel.SetActive(true);
        }
    }

    public void OptionApplyBtnClick() //�ɼ�â�� Apply��ư Ŭ����
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode); //����� �ɼ��� �����Ѵ�.
        optionPanel.SetActive(false);
    }

    

    public void exitBtnClick() // exit��ư Ŭ���� ���� ���� ó��
    {
        Application.Quit();
    }
}
