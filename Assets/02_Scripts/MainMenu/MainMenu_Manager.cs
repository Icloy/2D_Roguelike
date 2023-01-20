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

    public void GameBtnClick() //Game��ư Ŭ����
    {
        SceneManager.LoadScene("Loading_Scene"); 
    }

    public void OptionBtnClick() //Option��ư Ŭ���ÿ� Apply��ư Ŭ���� 
    {
        if (optionpanel.activeSelf == true) //�ɼ��г��� �����ִٸ� ���� �����ִٸ� Ų��.
        {
            optionpanel.SetActive(false);
        }
        else
        {
            optionpanel.SetActive(true);
        }
    }

    public void exitBtnClick() // exit��ư Ŭ���� ���� ���� ó��
    {
        Application.Quit();
    }
}
