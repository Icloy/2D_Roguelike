using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Singleton>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //���⿡ ���� ����Ǿ ������ �͵�
    public int slotNum = -1; //�ϴ� �׽�Ʈ�� �⺻�� 0 ���߿� ����
    public bool newGame;
}
