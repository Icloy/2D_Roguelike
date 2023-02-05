using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; //���������г�

    void Update()
    {
        //esc�� �ԷµǸ� ������ ������Ű�� �ɼ�â�� ����.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //�ش� �Լ�����
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
}
