using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Manager : MonoBehaviour
{
    //메인메뉴 버튼들
    public GameObject exitBtn;




    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void exitBtnClick() // exit버튼 클릭시 게임 종료 처리
    {
        Application.Quit();
    }
}
