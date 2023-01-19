using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScene : MonoBehaviour
{
    public Slider loadbar; //로딩바
    public Text loadtext;  //로딩중 텍스트
    public Text helptext;  //도움말

    private void Start()
    {
        StartCoroutine(LoadScene());
    }


    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game01_Scene");
        operation.allowSceneActivation= false;

        while (!operation.isDone) //로딩이 끝날때까지 돌아가는 반복문
        {
            yield return null;
            if (loadbar.value < 1f)
            {
                loadbar.value = Mathf.MoveTowards(loadbar.value, 1f, Time.deltaTime); //로딩바 값주는 코드
            }
            else
            {
                loadtext.text = "시작하려면 아무키나 입력하세요";
            }

            if (Input.anyKeyDown && loadbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation= true;
            }
        }
    }
}
