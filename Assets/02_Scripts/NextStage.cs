using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public enum NextPositionType
    {
        InitPosition,
        SomePosition,
    };

    public NextPositionType nextPositionType;

    public Transform DestinationPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            if (nextPositionType == NextPositionType.InitPosition)
            {
                collision.transform.position = Vector3.zero;
            }

            else if (nextPositionType == NextPositionType.SomePosition)
            {
                collision.transform.position = DestinationPoint.position;
            }
            else { }
        }
            
    }
}
