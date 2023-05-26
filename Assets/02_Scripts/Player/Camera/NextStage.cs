using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    public enum NextPositionType
    {
        InitPosition,
        SomePosition,
    };

    public NextPositionType nextPositionType;

    public Transform DestinationPoint;
    public bool fadeInOut;
    public bool SmoothMoving;
    public bool IfStage;
    public bool DStage;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IfStage == true)
        {
            if (collision.transform.CompareTag("Player"))
            {
                if (nextPositionType == NextPositionType.InitPosition)
                {
                    StartCoroutine(StageMgr.Instance.MoveNext(collision, DestinationPoint.position, fadeInOut, SmoothMoving));
                }

                else if (nextPositionType == NextPositionType.SomePosition)
                {
                    StartCoroutine(StageMgr.Instance.MoveNext(collision, DestinationPoint.position, fadeInOut, SmoothMoving));
                }
                else { }
            }
        }
        else
        {
            if (collision.transform.CompareTag("Player"))
            {
                if (nextPositionType == NextPositionType.InitPosition)
                {
                    StartCoroutine(StageMgr.Instance.MoveNext2(collision, Vector3.zero, fadeInOut, SmoothMoving));
                }

                else if (nextPositionType == NextPositionType.SomePosition)
                {
                    StartCoroutine(StageMgr.Instance.MoveNext2(collision, DestinationPoint.position, fadeInOut, SmoothMoving));
                }
                else { }
            }
        }

        if(DStage == true)
        {
            if (collision.transform.CompareTag("Player"))
            {
                SceneManager.LoadScene("DDD");
            }
        }
    }

}
