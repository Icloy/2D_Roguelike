using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    public enum NextPositionTypee
    {
        InitPositionn,
        SomePositionn,
    };

    public NextPositionTypee nextPositionType;

    public Transform DestinationPoint;
    public bool fadeInOut;
    public bool SmoothMoving;
    public bool NextStage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (NextStage == true)
        {
            if (collision.transform.CompareTag("Player"))
            {
                if (nextPositionType == NextPositionTypee.InitPositionn)
                {
                    StartCoroutine(StageMgr.Instance.MoveNext(collision, Vector3.zero, fadeInOut, SmoothMoving));
                }

                else if (nextPositionType == NextPositionTypee.SomePositionn)
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
                if (nextPositionType == NextPositionTypee.InitPositionn)
                {
                    StartCoroutine(StageMgr.Instance.MoveNext2(collision, Vector3.zero, fadeInOut, SmoothMoving));
                }

                else if (nextPositionType == NextPositionTypee.SomePositionn)
                {
                    StartCoroutine(StageMgr.Instance.MoveNext2(collision, DestinationPoint.position, fadeInOut, SmoothMoving));
                }
                else { }
            }
        }

    }
}
