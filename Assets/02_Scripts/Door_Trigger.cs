using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Trigger : MonoBehaviour
{
    public GameObject Left_Door;
    public GameObject Right_Door;
    public GameObject Boss;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Left_Door.gameObject.SetActive(true);
            Right_Door.gameObject.SetActive(true);
            Boss.gameObject.SetActive(true);
        }
    }
}
