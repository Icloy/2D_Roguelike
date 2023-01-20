using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    public GameObject optionpanel;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void GameBtnClick() //Game버튼 클릭시
    {
        SceneManager.LoadScene("Loading_Scene"); 
    }

    public void OptionBtnClick() //Option버튼 클릭시와 Apply버튼 클릭시 
    {
        if (optionpanel.activeSelf == true) //옵션패널이 켜져있다면 끄고 꺼져있다면 킨다.
        {
            optionpanel.SetActive(false);
        }
        else
        {
            optionpanel.SetActive(true);
        }
    }

    public void exitBtnClick() // exit버튼 클릭시 게임 종료 처리
    {
        Application.Quit();
    }
}
