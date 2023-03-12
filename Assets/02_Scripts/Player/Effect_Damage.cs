using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Damage : MonoBehaviour
{

    public int AtDmg; //���� ������
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
            // ������ ��� �� ����
            other.gameObject.GetComponent<Enemy>().TakeDamage(AtDmg);
        }
    }
}
