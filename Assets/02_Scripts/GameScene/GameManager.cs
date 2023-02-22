using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; //���������г�
    public GameObject optionPanel; //���������г�
    public GameObject GameOverPanel; //���ӿ����г�
    public GameObject graphicOptionPanel;
    public GameObject soundOptionPanel;

    public Dropdown resolutionDropdown;
    public Image hpGage;

    public Text coinCnt;
    public int coin; //�ΰ��� ��ȭ

    List<Resolution> resolutions = new List<Resolution>(); //����Ͱ� �����ϴ� �ػ󵵸� ������ �迭
    public int resolutionNum;
    FullScreenMode screenMode;
    private bool isGameOver; //���ӿ������� �Ǵ�
    public bool isPanelOpen = false;
    Player player;

    private void Start()
    {
        isGameOver = false;
        player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine("hpBar");
    }

    void Update()
    {
        //esc�� �ԷµǸ� ������ ������Ű�� �ɼ�â�� ����.
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (optionPanel.activeSelf)
            {
                optionBtn();
                return;
            }
            pasueGame();
        }

        if(player.curHp <= 0) //���ӿ���ó��
        {
            isGameOver = true;
            Time.timeScale = 0;
            isPanelOpen = true;
            GameOverPanel.SetActive(true);
        }
    }

    void pasueGame() //esc�� �������� ����Ǵ� �Լ�
    {
        if (!pausePanel.activeSelf)
        {
            Time.timeScale = 0; //���� �ð� ����
            isPanelOpen = true;
            pausePanel.SetActive(true);
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
        }
        else 
        {
            optionPanel.SetActive(false);
            pausePanel.SetActive(true);
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

    void searchpanel() //���� �ִ� �г� �˻���
    {
        if (graphicOptionPanel.activeSelf || soundOptionPanel.activeSelf)
        {
            graphicOptionPanel.SetActive(false);
            soundOptionPanel.SetActive(false);
        }
    }
    IEnumerator hpBar()
    {
        while (hpGage.fillAmount >= 0 ) //�÷��̾ ����ִ� ���� ���ѷ���
        {
            hpGage.fillAmount = (float)player.curHp / (float)player.maxHp;
            yield return null;
        }
    }

    public void UpdateCoinCnt() //���� �� ������Ʈ
    {
        //������ ����� ������ �ƴ϶��
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
