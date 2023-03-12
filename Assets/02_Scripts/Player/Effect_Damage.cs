using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Damage : MonoBehaviour
{

    public int AtDmg; //공격 데미지
    [HideInInspector] public GameObject Stat;


    private void OnEnable()
    {
        AtDmg = Player.instance.AtDmg;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(AtDmg);
            Stat.GetComponent<Stat>().MP += 30;
            // 데미지 계산 및 적용
            other.gameObject.GetComponent<Enemy>().TakeDamage(AtDmg);
        }
    }
}
