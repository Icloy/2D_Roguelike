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
<<<<<<< Updated upstream
            Debug.Log(AtDmg);
            Stat.GetComponent<Stat>().MP += 30;
            // ������ ��� �� ����
            other.gameObject.GetComponent<Enemy>().TakeDamage(AtDmg);
            ShakeCamera.instance.StartShake(0.05f, 0.05f);
=======
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log(AtDmg);
                Stat.GetComponent<Stat>().MP += 30;
                // ������ ��� �� ����
                enemy.TakeDamage(AtDmg);
            }
>>>>>>> Stashed changes
        }
    }
}
