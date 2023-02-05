using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; //게임정지패널

    void Update()
    {
        //esc가 입력되면 게임을 정지시키고 옵션창을 띄운다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //해당 함수실행
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
}
