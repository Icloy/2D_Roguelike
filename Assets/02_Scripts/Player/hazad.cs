using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazad : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Player.instance.curHp > 0)
        {
            collision.gameObject.GetComponent<TimeStop>().StopTime(0.1f, 5, 0.1f);
        }
    }
}
