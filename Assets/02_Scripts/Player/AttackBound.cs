using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBound : MonoBehaviour
{
    Rigidbody2D rigid;
    bool platformrepeat;
    bool enemyrepeat;

    void OnEnable()
    {
        enemyrepeat = platformrepeat = false;
        rigid = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            if(!platformrepeat && !enemyrepeat)
            {
                platformrepeat = true;
                rigid.velocity = Vector2.zero;
                Player.Instance.upforce();
            }
        }
        //else if (collision.CompareTag("Enemy"))
        //{
        //    if (!platformrepeat && !enemyrepeat)
        //    {
        //        enemyrepeat = true;
        //        rigid.velocity = Vector2.zero;
        //        Player.Instance.upforce();
        //    }
        //}
    }
}
