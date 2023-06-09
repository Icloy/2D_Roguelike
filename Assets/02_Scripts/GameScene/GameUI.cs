using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameUI : MonoBehaviour
{
    #region
    public GameObject mapPanel; //�� �г�(m)
    public GameObject pausePanel; //���� �г�(esc)
    public GameObject optionPanel; //�ɼ� �г�()
    public GameObject gameoverPanel; //���� ���� �г�()
    public GameObject clearPanel; //���� ���� �г�()
    public GameObject bspPanel; //bsp ���� �г�()
    public GameObject map;

    public GameObject optionGraphic; //�ɼ� �׷��� �г�
    public GameObject optionSound; //�ɼ� ���� �г�

    public Button pausePanelSelBtn; //�ʱ� ���õǾ��ִ� ��ư(Ű�����)
    public Button optionPanelSelBtn; //�ʱ� ���õǾ��ִ� ��ư(Ű�����)

    public Dropdown resolutionDropdown; //�ػ� ������ ����ٿ�
    public Text coinCnt;    //���� ī��Ʈ �ؽ�Ʈ

    public Image fadeImage;
    public float fadeSpeed = 0.5f;
    public GameObject EndingText;
    private float currentAlpha = 0.0f;

    List<Resolution> resolutions = new List<Resolution>(); //����Ͱ� �����ϴ� �ػ󵵸� ������ �迭
    FullScreenMode screenMode;
    int resolutionNum;
    public TMP_InputField[] bspInputfield;

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

    private void Update()
    {
        if (!clearPanel.activeSelf)
        {
            return;
        }
        currentAlpha += fadeSpeed * Time.deltaTime;
        fadeImage.color = new Color(1.0f, 1.0f, 1.0f, currentAlpha);

        if (currentAlpha >= 1.0f)
        {
            EndingText.gameObject.SetActive(true);
        }
    }
    public void PauseGame() //pausePanel (esc)
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
        PauseGame();
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
        OptionBtn();
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
        CameraManager.instance._allvirtualcameras[1].enabled = false;
        CameraManager.instance._allvirtualcameras[0].enabled = true;
        gameoverPanel.SetActive(false);
    }
    public void Clear()
    {
        clearPanel.SetActive(true);
    }

    private void Searchpanel() //���� �ִ� �г� �˻���
    {
        if (optionGraphic.activeSelf || optionSound.activeSelf)
        {
            optionGraphic.SetActive(false);
            optionSound.SetActive(false);
        }
    }
    #region Store
    public void StoreBtn()
    {
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
    }
    #endregion
    #region Bsp
    public void BspPanel() //bspPanel ���� ���� ��
    {
        if (!bspPanel.activeSelf)
        {
            bspPanel.SetActive(true);
            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
            }
        }
        else
        {
            bspPanel.SetActive(false);
            if(Time.timeScale == 0)
            {
                PauseGame();
            }
        }
    }

    public void BspApplyBtn() //bspPanel Apply��ư Ŭ���� �� ���۰� ���õ� �����͸� ������
    {
        //inputfield�� ���� �������� �˻�
        int n1, n2, n3;
        if(!int.TryParse(bspInputfield[0].text, out n1) || !int.TryParse(bspInputfield[1].text, out n2) || !int.TryParse(bspInputfield[2].text, out n3))
        {
            //���ڰ� �ƴϸ� ó���� �ڵ�
            ToastMsg.Instance.showMessage("�Է��Ͻ� ���� �߸��Ǿ����ϴ�. ���ڷ� �Է��Ͽ��ּ���", 1f);
            return;
        }
        string send = bspInputfield[0].text + "," + bspInputfield[1].text + "," + bspInputfield[2].text;
        
        //�ٸ������� �� ���� �Լ� ����
        BspMapValue(send);
        //�г� ����
        BspPanel();
    }
    
    public void BspMapValue(string val) //�ٸ� ��ũ��Ʈ�� �Űܼ� �� ���� ���� �� ���� 
    {
        string[] values = val.Split(',');

        int x = int.Parse(values[0]);
        int y = int.Parse(values[1]);
        int z = int.Parse(values[2]);
        Debug.Log("x : " + x + " y : " + y + " z : " + z);
        map.GetComponent<MapGenerator>().SetValue(x, y, z);
    }

    #endregion
}
