using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject mapPanel; //�� �г�(m)
    public GameObject pausePanel; //���� �г�(esc)
    public GameObject optionPanel; //�ɼ� �г�()
    public GameObject gameoverPanel; //���� ���� �г�()

    public GameObject optionGraphic; //�ɼ� �׷��� �г�
    public GameObject optionSound; //�ɼ� ���� �г�

    public Button pausePanelSelBtn; //�ʱ� ���õǾ��ִ� ��ư(Ű�����)
    public Button optionPanelSelBtn; //�ʱ� ���õǾ��ִ� ��ư(Ű�����)

    public Dropdown resolutionDropdown; //�ػ� ������ ����ٿ�
    public Text coinCnt;    //���� ī��Ʈ �ؽ�Ʈ

    List<Resolution> resolutions = new List<Resolution>(); //����Ͱ� �����ϴ� �ػ󵵸� ������ �迭
    FullScreenMode screenMode;
    int resolutionNum;

    private void Start()
    {
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
    public void FullScreenBtn(bool isFull) //��üȭ�� ��� 
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }


    public void DropboxOptionChange(int x) //�ػ� ����ڽ����� �����Ѱ� �����
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

    public void Map()
    {
        if (!mapPanel.activeSelf && !GameManager.instance.isPanelOpen)
        {
            mapPanel.SetActive(true);
            GameManager.instance.isMapOpen = true;
        }
        else
        {
            mapPanel.SetActive(false);
            GameManager.instance.isMapOpen = false;
        }
    }

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

    private void Searchpanel() //���� �ִ� �г� �˻���
    {
        if (optionGraphic.activeSelf || optionSound.activeSelf)
        {
            optionGraphic.SetActive(false);
            optionSound.SetActive(false);
        }
    }

}
