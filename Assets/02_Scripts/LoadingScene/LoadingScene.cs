using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScene : MonoBehaviour
{
    public Slider loadbar; //�ε���
    public Text loadtext;  //�ε��� �ؽ�Ʈ
    public Text helptext;  //����

    private void Start()
    {
        StartCoroutine(LoadScene());
    }


    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game01_Scene");
        operation.allowSceneActivation= false;

        while (!operation.isDone) //�ε��� ���������� ���ư��� �ݺ���
        {
            yield return null;
            if (loadbar.value < 1f)
            {
                loadbar.value = Mathf.MoveTowards(loadbar.value, 1f, Time.deltaTime); //�ε��� ���ִ� �ڵ�
            }
            else
            {
                loadtext.text = "�����Ϸ��� �ƹ�Ű�� �Է��ϼ���";
            }

            if (Input.anyKeyDown && loadbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation= true;
            }
        }
    }
}
