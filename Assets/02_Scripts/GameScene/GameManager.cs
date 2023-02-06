using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; //게임정지패널
    public GameObject optionPanel; //게임정지패널


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

}
