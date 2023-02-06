using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; //���������г�
    public GameObject optionPanel; //���������г�


    void Update()
    {
        //esc�� �ԷµǸ� ������ ������Ű�� �ɼ�â�� ����.
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

    void pasueGame() //esc�� �������� ����Ǵ� �Լ�
    {
        if (!pausePanel.activeSelf)
        {
            Time.timeScale = 0; //���� �ð� ����
            pausePanel.SetActive(true);
        }
        else //������ �̹� ���� �Ǿ��ִٸ�
        {
            Time.timeScale = 1.0f; //�ð� ���� ����
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

}
