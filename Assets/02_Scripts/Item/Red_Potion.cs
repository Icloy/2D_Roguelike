using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Potion : MonoBehaviour
{
    public int heal;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player.instance.Damaged(+heal);
            Destroy(this.gameObject);
        }
    }
}
