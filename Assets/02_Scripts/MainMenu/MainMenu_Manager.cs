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

    public TMP_Text slot0;
    public TMP_Text slot1;
    public TMP_Text slot2;

    List<Resolution> resolutions = new List<Resolution>(); //����Ͱ� �����ϴ� �ػ󵵸� ������ �迭
    FullScreenMode screenMode;
    public Toggle fullscreenBtn;
    public Dropdown resolutionDropdown;
    public int resolutionNum;

    GetData getdata;

    private void Start()
    {
        Resolution();
        SelBtn.Select();
    }
    public void GameBtnClick() //Game��ư Ŭ����
    {
        slotPanel.SetActive(true);
        RefreshSlot();
    }

    public void Gamestart()
    {
        LoadingScene.LoadScene("Game01_Scene");
    }

    private void RefreshSlot()
    {
        slot0.text = "";
        slot1.text = "";
        slot2.text = "";
    }

    public void Slot(string a)
    {
        getdata.GetDbData(a);
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
